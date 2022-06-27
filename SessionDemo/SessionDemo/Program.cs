using System;
using System.Security.Cryptography;
using dotenv.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SessionDemo.Dtos;
using SessionDemo.Repositories;
using SessionDemo.Services;
using StackExchange.Redis;

// Load environmental variables
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true, probeLevelsToSearch: 6));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Redis
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
var connectionString = $"localhost:{redisPort},password={redisPassword}";

var multiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

builder.Services.AddScoped<MySqlService>();
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/login", ([FromServices] SessionService sessionService, [FromServices] UserRepository userRepository, PostLoginDto postLoginDto) =>
{
    // TODO: validate input DTO

    var username = postLoginDto.Username;
    var hash = SHA512.HashData(postLoginDto.Password.Select(x => (byte) x).ToArray())
        .Select(x => (char) x).ToString();

    var user = userRepository.GetByCredentials(username, hash);

    return sessionService.StartSession(user.UserId);
});

app.UseHttpsRedirection();

app.Run();