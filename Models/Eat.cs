using System;

namespace LaprTrackr.Backend.Models
{
    public class Eat
    {
        public int EatId { get; set; }

        public int UserId { get; set; }

        public int FoodId { get; set; }

        public DateTime Date { get; set; }

        public string Barcode { get; set; }

        public string Name { get; set; }

        public double Calories { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
