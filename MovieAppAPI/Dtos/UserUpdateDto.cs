using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Dtos
{
    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string ConfirmPassword { get; set; }
        public bool DeleteCurrentImage { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
