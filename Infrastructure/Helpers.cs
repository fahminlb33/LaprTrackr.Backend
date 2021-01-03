using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                sb.Append(_randomLazy.Value.Next(10).ToString());
            }

            return sb.ToString();
        }
    }
}
