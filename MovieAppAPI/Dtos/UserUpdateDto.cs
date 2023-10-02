﻿using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Dtos
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? Token { get; set; }
        public string? ImageName { get; set; }
        public string? ImageSource { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
