using LaprTrackr.Backend.Models;
using System;
using System.Linq;

namespace LaprTrackr.Backend.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(LaprTrackrContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
                return;

            context.Users.AddRange(new User()
            {
                Username = "admin",
                Password = "$2y$12$ewP5rtPuWysYrLdOSqzZ/uDC0gK/Dleq7BvFP32N5VS1bUvT.om.2 ",
                Email = "support@kodesiana.com",
                Role = UserRoles.Admin,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            });

            context.SaveChanges();
        }
    }
}
