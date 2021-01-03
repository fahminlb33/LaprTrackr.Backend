using LaprTrackr.Backend.Models;
using System;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Services
{
    public interface IEatsService
    {
        Task<Eat> Create(Eat model);
        Task Delete(Eat model);
        Task<Eat> GetById(int id);
        Task<Eat> Update(Eat model);
    }

    public class EatsService : IEatsService
    {
        private readonly LaprTrackrContext _context;

        public EatsService(LaprTrackrContext context)
        {
            _context = context;
        }

        public async Task<Eat> GetById(int id)
        {
            return await _context.EatRecords.FindAsync((object)id);
        }

        public async Task<Eat> Create(Eat model)
        {
            model.CreatedAt = DateTime.Now;
            model.LastUpdatedAt = DateTime.Now;

            await _context.EatRecords.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task Delete(Eat model)
        {
            _context.EatRecords.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Eat> Update(Eat model)
        {
            _context.EatRecords.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}
