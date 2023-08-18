using MovieAppAPI.Models;

namespace MovieAppAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string username);
        User GetUserByEmail(string email);
        bool UserExists(int id);
        bool UserExists(string email);
        public bool UsernameExists(string username);
        public bool UserExists(string email, string password);
        bool UsernameExistsUpdate(int updatedUserId, string username);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        string CheckPasswordStrength(string password);
        public string CreateJwt(User user);

    }
}
