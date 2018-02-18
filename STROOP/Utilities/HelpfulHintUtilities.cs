using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class HelpfulHintUtilities
    {
        private static readonly List<string> helpfulHints =
            new List<string>()
            {
                "Hint 1",
                "Hint 2",
            };

        public static string GetRandomHelpfulHint()
        {
            Random random = new Random();
            int randomIndex = (int)(helpfulHints.Count * random.NextDouble());
            return helpfulHints[randomIndex];
        }
    }
}
