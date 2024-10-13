using System;
using SecureWebApp.Models;

namespace SecureWebApp.Data;

public interface IUser
{
    User Registration (User user);
    User Login (User user);
    void UpdateUserPassword(User user); // New method for updating password
}
