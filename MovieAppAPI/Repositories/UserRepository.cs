using MovieAppAPI.Data;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public User GetUser(int id) 
        {
            return _context.Users.Where(u => u.UserId == id).FirstOrDefault();
        }

        public User GetUser(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(u => u.UserId).ToList();
        }

        public bool UserExists(int id) 
        {
            return _context.Users.Any(u => u.UserId == id);
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();

        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public string CheckPasswordStrength(string password)
        {
            StringBuilder messages = new StringBuilder();
            if (password.Length < 6)
                messages.Append("Password length should be at least 6 characthers." + Environment.NewLine);
            if ((!Regex.IsMatch(password, "[a-z]") && !Regex.IsMatch(password, "[A-Z]")) || !Regex.IsMatch(password, "[0-9]"))
                messages.Append("Password should contain letters and numbers." + Environment.NewLine);

            return messages.ToString();
        }

       
    }
}
