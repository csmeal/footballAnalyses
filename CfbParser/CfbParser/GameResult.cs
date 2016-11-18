using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfbParser
{
    public class GameResult {
        public DateTime Date { get; set; }

        public string HomeTeam { get; set; }

        public int HomeScore { get; set; }

        public string VisitingTeam { get; set; }

        public int VisitingScore { get; set; }

        public double? Line { get; set; }

        public static string  TeamReplace(string bigString) {
            return bigString
                            .Replace("N. C. State", "No Carolina State")
                            .Replace("N.C. State", "No Carolina State")
                            .Replace("Texas A&M", "Texas A+M")
                            .Replace("Miami (Fla.)", "Miami-Florida")
                            .Replace("SMU", "S-M-U")
                            .Replace("St.", "State");
        }

        public GameResult(string[] parsedLine) {
            // Date = new DateTime(parsedLine[0]);
            VisitingTeam = parsedLine[1];
            VisitingScore = Convert.ToInt32(parsedLine[2]);
            HomeTeam = parsedLine[3];
            HomeScore = Convert.ToInt32(parsedLine[4]);

            if (!string.IsNullOrEmpty(parsedLine[5].Trim(' '))) {
                Line = Convert.ToDouble(parsedLine[5]);
            }
            else {
                Line = null;
            }
        }
    }
}
