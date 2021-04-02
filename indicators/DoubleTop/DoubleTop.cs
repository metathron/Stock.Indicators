using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetDoubleTop<TQuote>(
        IEnumerable<TQuote> history,
        int lookbackPeriod = 70, int minimumDistanceToRightRop = 10)
        where TQuote : IPatternQuote
        {
            //https://www.tradinformed.com/double-top-chart-patterns-using-excel/#:~:text=The%20double%20top%20is%20bearish,support%20and%20it%20falls%20back.

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "DoubleTop";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            //for (int i = 1; i < historyList.Count; i++)
            //{
            //    if (historyList.Count > i + 1)
            //    {
            //        TQuote current = historyList[i];

            //        //IF(AND($D76>MAX($D77:$D80),$D76>MAX($D5:$D75)),$D76,IF(OR($F79 > G79, G79 = G5),, G79))
            //        if (i > lookbackPeriod && (i + minimumDistanceToRightRop) < historyList.Count)
            //        {
            //            var isHighest = current.High > historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).Max(m => m.High);
            //            if (isHighest && !historyList.Skip(i).Take(minimumDistanceToRightRop / 2).Any(e => e.High > current.High))
            //            {
            //                //check for cancel  +5
            //                var nextEntries = historyList.Skip(i).Take(minimumDistanceToRightRop).ToList();
            //                var lowest = nextEntries.OrderBy(e => e.Low).First();
            //                var indexOfLowest = nextEntries.IndexOf(lowest);

            //                if (indexOfLowest > 4)
            //                {
            //                    ????
            //                    PatternResult result = new PatternResult(current, name)
            //                    {
            //                        Date = current.Date,
            //                        Long = true
            //                    };
            //                    results.Add(result);

            //                }

            //            }
            //        }
            //    }
            //}

            return results;
        }
    }
}
