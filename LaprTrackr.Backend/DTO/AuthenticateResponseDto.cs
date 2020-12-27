using LaprTrackr.Backend.Models;

namespace LaprTrackr.Backend.DTO
{
    public class AuthenticateResponseDto
    {
        public string Token { get; set; }

        public User User { get; set; }
    }
}
