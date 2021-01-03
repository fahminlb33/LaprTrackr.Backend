// Decompiled with JetBrains decompiler
// Type: LaprTrackr.Backend.Infrastructure.DbInitializer
// Assembly: LaprTrackr.Backend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2187E3DC-0415-44B5-A28F-1DDDDA295D12
// Assembly location: D:\Workspace\Personal\LaprTrackr.Backend\LaprTrackr.Backend\bin\Debug\net5.0\LaprTrackr.Backend.dll

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
