using LaprTrackr.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Services
{
    public interface IFoodService
    {
        Task<Food> Create(Food food);
        Task Delete(Food food);
        Task<Food> FindByBarcode(string barcode);
        Task<Food> GetById(int id);
        Task<Food> Update(Food food);
    }

    public class FoodService : IFoodService
    {
        private readonly LaprTrackrContext _context;

        public FoodService(LaprTrackrContext context)
        {
            _context = context;
        }

        public async Task<Food> GetById(int id)
        {
            return await _context.Foods.FindAsync((object)id);
        }

        public async Task<Food> FindByBarcode(string barcode)
        {
            return await _context.Foods.SingleOrDefaultAsync(x => x.Barcode == barcode);
        }

        public async Task<Food> Create(Food food)
        {
            food.CreatedAt = DateTime.Now;
            food.LastUpdatedAt = DateTime.Now;

            await _context.Foods.AddAsync(food);
            await _context.SaveChangesAsync();

            return food;
        }

        public async Task Delete(Food food)
        {
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
        }

        public async Task<Food> Update(Food food)
        {
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();

            return food;
        }
    }
}
