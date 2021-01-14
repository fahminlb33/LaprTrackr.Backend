using LaprTrackr.Backend.DTO;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LaprTrackr.Backend.Infrastructure
{
    public class LaprTrackrException : Exception
    {
        public LaprTrackrStatusCodes AppCode { get; set; }

        public object ExtraData { get; set; }

        public LaprTrackrException()
        {
        }

        public LaprTrackrException(string message)
          : base(message)
        {
        }

        public LaprTrackrException(string message, Exception inner)
          : base(message, inner)
        {
        }

        public LaprTrackrException(LaprTrackrStatusCodes code, string message)
          : base(message)
          => AppCode = code;

        public LaprTrackrException(LaprTrackrStatusCodes code, string message, object extraData)
          : base(message)
        {
            AppCode = code;
            ExtraData = extraData;
        }

        public ErrorResponse GetResponse() => new ErrorResponse()
        {
            StatusCode = AppCode,
            Data = ExtraData,
            Message = Message
        };

        public ActionResult GetActionResult()
        {
            ErrorResponse response = GetResponse();
            switch (AppCode)
            {
                case LaprTrackrStatusCodes.BodyNotValid:
                    return new BadRequestObjectResult(response);
                case LaprTrackrStatusCodes.NotFound:
                    return new NotFoundObjectResult(response);
                case LaprTrackrStatusCodes.AuthNotAuhenticated:
                case LaprTrackrStatusCodes.AuthNotAuthorized:
                    return new UnauthorizedObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }
    }
}
