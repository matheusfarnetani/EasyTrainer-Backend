﻿namespace Application.DTOs.Auth
{
    public class LoginRequestDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "user", "instructor", "admin"
    }
}