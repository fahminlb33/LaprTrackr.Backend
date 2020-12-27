using System;

namespace LaprTrackr.Backend.Models
{
    public class Food
    {
        public int FoodId { get; set; }

        public string Barcode { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public double Calories { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
