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
        bool UsernameExistsUpdate(int updatedUserId, string username);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        string CheckPasswordStrength(string password);
        public string CreateJwt(User user);

    }
}
