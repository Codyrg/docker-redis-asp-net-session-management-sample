namespace SessionDemo.Services
{
    using System;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Models;
    using StackExchange.Redis;
    
    /// <summary>
    /// Service for managing user sessions
    /// </summary>
    public class SessionService
    {
        private readonly ILogger<SessionService> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        /// <summary>
        /// Create new SessionService
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="connectionMultiplexer">Redis Connection Multiplexer</param>
        public SessionService(ILogger<SessionService> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;
        }

        /// <summary>
        /// Start a session for user with the provided ID
        /// </summary>
        /// <param name="userId">Guid user ID to start session for</param>
        /// <returns>string Session ID of newly created session</returns>
        public string StartSession(Guid userId)
        {
            var startTime = DateTime.Now;
            // TODO: go to database to get the user's access level
            var accessLevel = AccessLevels.Admin;
            var sessionState = new SessionState(startTime, accessLevel);

            // TODO: generate a secure session id
            var sessionId = "my-session-id";
            
            // Generate the session in Redis:
            var database = _connectionMultiplexer.GetDatabase();
            database.StringSet(sessionId, JsonSerializer.Serialize(sessionState));

            return sessionId;
        }

        /// <summary>
        /// Get the stored session state for the provided session key
        /// </summary>
        /// <param name="sessionKey">string session key to get the state of</param>
        /// <returns>SessionState stored in Redis, null on error</returns>
        public SessionState? GetSessionState(string sessionKey)
        {
            var database = _connectionMultiplexer.GetDatabase();
            var sessionJson = database.StringGet(sessionKey).ToString();

            return JsonSerializer.Deserialize<SessionState>(sessionJson);
        }

        /// <summary>
        /// End the session for the provided session key
        /// </summary>
        /// <param name="sessionKey">string key for the current session</param>
        /// <returns>bool true if successfully ended session</returns>
        public bool EndSession(string sessionKey)
        {
            var database = _connectionMultiplexer.GetDatabase();
            return database.KeyDelete(sessionKey);
        } 
    }
}