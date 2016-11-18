using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfbParser
{
    public class Ranking
    {
        public int Rank { get; set; }

        public string Team { get; set; }
    }

    public class RankingList
    {
        public List<Ranking> Ranking { get; set; } = new List<Ranking>();

        public int Year { get; set; }
    }
}
