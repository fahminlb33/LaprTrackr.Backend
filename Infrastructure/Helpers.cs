using System;
using System.Text;

namespace LaprTrackr.Backend.Infrastructure
{
    public class Helpers
    {
        private static Lazy<Random> _randomLazy = new Lazy<Random>();

        public static string GenerateRandomBarcode()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                sb.Append(_randomLazy.Value.Next(9).ToString());
            }

            return sb.ToString();
        }
    }
}
