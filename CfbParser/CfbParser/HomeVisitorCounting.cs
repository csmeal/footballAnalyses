using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfbParser
{
    public class HomeVisitorCounting
    {
        public override string ToString() {
            return Year.ToString() + "\t" + HomeWinCounter.ToString() + "\t" + VisitorCounter.ToString() + "\t" + ((int)(HomeWinPercentage * 100)).ToString();
        }

        public int Year { get; set; }
        public int HomeWinCounter { get; set; }

        public int VisitorCounter { get; set; }

        public int TotalGames { get { return HomeWinCounter + VisitorCounter; } }

        public double HomeWinPercentage {
            get {
                return (double)HomeWinCounter / (double)TotalGames;
            }
        }
    }
}
