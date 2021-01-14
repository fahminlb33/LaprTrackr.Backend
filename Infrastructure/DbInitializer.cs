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
                Username = "Fahmi",
                Password = "f4hm1",
                Email = "support@kodesiana.com",
                Role = "Admin",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            });

            context.SaveChanges();
        }
    }
}
