using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class Extensions
    {
        /// <summary>
        /// Splice text with new line in every 'n' characters 
        /// </summary>
        /// <param name="text">The text to splice</param>
        /// <param name="n">Number of characters to put new line</param>
        /// <returns>This string with new lines in every 'n' characters</returns>
        public static string SpliceText(this string text, int n = 8)
        {
            text = string.Join(Environment.NewLine, text.Split()
                .Select((word, index) => new { word, index })
                .GroupBy(x => x.index / n)
                .Select(grp => string.Join(" ", grp.Select(x => x.word))));
            return text;
        }

        public static string FormatNumber(this double num)
        {
            if (num <= 1 && num >= 0)
                return string.Format("{0:0.###}", (num));
            // Ensure number has max 3 significant digits (no rounding up can happen)
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            if (i == 0)
                return num.ToString("0.##");
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }
    }
}
