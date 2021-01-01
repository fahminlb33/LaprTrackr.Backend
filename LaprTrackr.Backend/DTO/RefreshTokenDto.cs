using System.ComponentModel.DataAnnotations;

namespace LaprTrackr.Backend.DTO
{
    public class RefreshTokenDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
