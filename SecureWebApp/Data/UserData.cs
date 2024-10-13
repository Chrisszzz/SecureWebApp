using System;
using SecureWebApp.Models;
using BC = BCrypt.Net.BCrypt;

namespace SecureWebApp.Data;

public class UserData : IUser
{
    private readonly ApplicationDbContext _db;
    public UserData(ApplicationDbContext db)
    {
        _db = db;
    }

    public User Login(User user)
    {
        var _user = _db.Users.FirstOrDefault(u => u.Username == user.Username);
        if (_user == null)
        {
            throw new Exception("User not Found");
        }
        if (!BCrypt.Net.BCrypt.Verify(user.Password, _user.Password))
        {
            throw new Exception("Password is Incorrect");
        }
        return _user;
    }

    public User Registration(User user)
    {
        try
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void UpdateUserPassword(User user)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser != null)
        {
            existingUser.Password = user.Password; // New hashed password
            _db.SaveChanges();
        }
        else
        {
            throw new Exception("User not found");
        }
    }
}
