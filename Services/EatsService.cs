using LaprTrackr.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            return await _context.EatRecords.FindAsync(id);
        }

        public async Task<Eat> Create(Eat eat)
        {
            eat.CreatedAt = DateTime.Now;
            eat.LastUpdatedAt = DateTime.Now;

            var food = await _context.Foods.Where(x => x.Barcode == eat.Barcode).FirstOrDefaultAsync();
            if (food is null)
            {
                food = new Food
                {
                    Barcode = eat.Barcode,
                    Calories = eat.Calories,
                    Name = eat.Name,
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    Notes = "User submitted data. Info may be inaccurrate."
                };

                await _context.Foods.AddAsync(food);
                await _context.SaveChangesAsync();
            }

            eat.FoodId = food.FoodId;
            
            await _context.EatRecords.AddAsync(eat);
            await _context.SaveChangesAsync();

            return eat;
        }

        public async Task Delete(Eat eat)
        {
            _context.EatRecords.Remove(eat);
            await _context.SaveChangesAsync();
        }

        public async Task<Eat> Update(Eat eat)
        {
            _context.EatRecords.Update(eat);
            await _context.SaveChangesAsync();

            return eat;
        }
    }
}
