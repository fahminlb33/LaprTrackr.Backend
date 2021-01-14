 using LaprTrackr.Backend.Models;
using System;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Services
{
    public interface IUserService
    {
        Task<User> Create(User user);
        Task Delete(User user);
        Task<User> GetById(int id);
        Task<User> Update(User user);
    }

    public class UserService : IUserService
    {
        private readonly LaprTrackrContext _context;

        public UserService(LaprTrackrContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> Create(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.LastUpdatedAt = DateTime.Now;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
