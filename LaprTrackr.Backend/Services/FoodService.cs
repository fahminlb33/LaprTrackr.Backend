using LaprTrackr.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Services
{
    public interface IFoodService
    {
        Task<Food> Create(Food food);

        Task Delete(Food food);

        Task<IEnumerable<Food>> FindByBarcode(string barcode);

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
            Food async = await _context.Foods.FindAsync((object)id);
            return async;
        }

        public async Task<IEnumerable<Food>> FindByBarcode(string barcode)
        {
            List<Food> listAsync = await _context.Foods.Where(x => x.Barcode == barcode).ToListAsync();
            return listAsync;
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

            return await GetById(food.FoodId);
        }
    }
}
