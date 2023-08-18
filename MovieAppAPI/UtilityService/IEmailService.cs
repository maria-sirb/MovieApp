using MovieAppAPI.Models;

namespace MovieAppAPI.UtilityService
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
}
