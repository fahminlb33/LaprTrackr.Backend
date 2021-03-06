﻿using System.ComponentModel.DataAnnotations;

namespace LaprTrackr.Backend.DTO
{
    public class AuthenticateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
