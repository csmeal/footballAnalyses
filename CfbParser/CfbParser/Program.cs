using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfbParser
{
    class Program
    {
        /// <summary>
        /// Note that a positive "line" indicates that the home team is the favorite.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
            var allGameCounter = new List<HomeVisitorCounting>();
            var closeGameCounter= new List<HomeVisitorCounting>();
            var allGames = new List<GameResult>();
           
            string rankingFile = Directory.GetFiles(args[0]).FirstOrDefault(s => s.ToLower().Contains("rank"));
            var years = Enumerable.Range(1978, 2013 - 1978 + 1).ToList();
            List<RankingList> rankings = GetRankings(years, GameResult.TeamReplace(File.ReadAllText(rankingFile)).Split(Environment.NewLine.ToCharArray()).ToList());

            foreach (string filename in Directory.GetFiles(args[0]).Where(s => Path.GetExtension(s).ToLower() == ".csv")) {
                var results = File.ReadLines(filename).Skip(1);
                var year = Convert.ToInt32(new string(Path.GetFileNameWithoutExtension(filename).Where(s => char.IsDigit(s)).ToArray()));
                var ranking = rankings.FirstOrDefault(r => r.Year == year);
                var splitLines = results.Select(s => s.Split(','));
              
                var allGameResult = results
                            .Select(s => new GameResult(s.Split(','))).ToList();
                //var vegasSub3 = allGameResult.Where(s => s.Line != null && s.Line.Value  < 0 && s.Line.Value > -3.5);
                //var vegasSub3 = allGameResult.Where(s =>
                //                    ranking.Ranking.FirstOrDefault(r => r.Team == s.HomeTeam) != null &&
                //                    ranking.Ranking.FirstOrDefault(r => r.Team == s.VisitingTeam) != null &&
                //                    Math.Abs(ranking.Ranking.FirstOrDefault(r => r.Team == s.HomeTeam).Rank -
                //                            ranking.Ranking.FirstOrDefault(r => r.Team == s.VisitingTeam).Rank) <= 5);
                var vegasSub3 = allGameResult.Where(s => Math.Abs(s.HomeScore - s.VisitingScore) <= 3);


                 allGameCounter.Add( new HomeVisitorCounting() {
                    Year = year,
                    HomeWinCounter = allGameResult.Count(s => s.HomeScore > s.VisitingScore),
                    VisitorCounter = allGameResult.Count(s => s.VisitingScore > s.HomeScore)
                });

                closeGameCounter.Add(new HomeVisitorCounting() {
                    Year = year,
                    HomeWinCounter = vegasSub3.Count(s => s.HomeScore > s.VisitingScore),
                    VisitorCounter = vegasSub3.Count(s => s.VisitingScore > s.HomeScore)
                });
                allGames.AddRange(allGameResult);
            }
            closeGameCounter.Add(new HomeVisitorCounting() {
                HomeWinCounter = closeGameCounter.Sum(s => s.HomeWinCounter),
                VisitorCounter = closeGameCounter.Sum(s => s.VisitorCounter),
                Year = 70741
            });

            foreach (var team in rankings.SelectMany(s => s.Ranking.Select(r => r.Team))){
                if (allGames.Count(a => team == a.HomeTeam) == 0) {
                    throw new Exception(team + " was not found");
                }
            }
            //allGameCounter.OrderBy(s => s.Year).ToList().ForEach(s => Console.WriteLine(s.ToString()));
            closeGameCounter.OrderBy(s => s.Year).ToList().ForEach(s=>Console.WriteLine(s.ToString()));
        }


        private static List<RankingList> GetRankings(List<int> years, List<string> rankStrings) {
            List<RankingList> result = new List<RankingList>();
            for (int i = 0; i < years.Count; i++) {
                RankingList list = new RankingList();
                list.Year = years[i];
                int nextStop = rankStrings.Count();
                if (i < years.Count - 1) {
                    nextStop = rankStrings.IndexOf(years[i + 1].ToString());
                }
                var oneYearRank = rankStrings
                    .Skip(rankStrings.IndexOf(years[i].ToString()) + 1)
                    .Take(
                        nextStop -
                        rankStrings.IndexOf(years[i].ToString())
                         - 1);
                foreach (string rank in oneYearRank.Where(s => !string.IsNullOrWhiteSpace(s) && !s.Contains("(tie)"))) {
                    var split = rank.IndexOf('.');
                    list.Ranking.Add(new Ranking() {
                        Rank = Convert.ToInt32(rank.Substring(0, split)),
                        Team = rank.Substring(split + 2, rank.Length - split - 2).Trim()
                    });
                }
                result.Add(list);
            }

            return result;
        }
    }
}
