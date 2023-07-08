using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string username);
        bool UserExists(int id);
        bool UserExists(string email);
        bool CreateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        string CheckPasswordStrength(string password);

    }
}
