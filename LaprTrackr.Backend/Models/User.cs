using System;
using System.Text.Json.Serialization;

namespace LaprTrackr.Backend.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Email { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
