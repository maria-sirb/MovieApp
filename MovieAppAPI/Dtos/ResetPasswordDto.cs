﻿namespace MovieAppAPI.Dtos
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string EmailToken { get; set; }
        public string NewPassword { get; set; }
    }
}
