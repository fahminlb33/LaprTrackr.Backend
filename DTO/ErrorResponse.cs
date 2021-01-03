using LaprTrackr.Backend.Infrastructure;

namespace LaprTrackr.Backend.DTO
{
    public class ErrorResponse
    {
        public LaprTrackrStatusCodes StatusCode { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
