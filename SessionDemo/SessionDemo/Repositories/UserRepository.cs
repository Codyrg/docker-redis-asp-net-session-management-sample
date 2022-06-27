using SessionDemo.Entities;
using SessionDemo.Services;

namespace SessionDemo.Repositories;

public class UserRepository
{
    private MySqlService _mySqlService;
    
    public UserRepository(MySqlService mySqlService)
    {
        _mySqlService = mySqlService;
    }

    public bool Create(User user)
    {
        // TODO
        return false;
    }

    public User GetByCredentials(string username, string hash)
    {
        // TODO; implement
        return new User(Guid.Empty, "", "");
    }
}