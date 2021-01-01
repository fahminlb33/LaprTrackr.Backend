using System;
using System.ComponentModel.DataAnnotations;

namespace LaprTrackr.Backend.Models
{
    public class RefreshToken
    {
        [Key]
        public int RefreshTokenId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
