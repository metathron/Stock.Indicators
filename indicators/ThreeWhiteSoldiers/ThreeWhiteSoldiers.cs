using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetThreeWhiteSoldiers<TQuote>(
        IEnumerable<TQuote> history, decimal minimunBodyInPercent = 50.0M)
        where TQuote : IQuote
        {
            //https://www.investopedia.com/terms/t/three_black_crows.asp
            int lookbackPeriod = 3;
            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "ThreeWhiteSoldiers";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                PatternQuote current = historyList[i];
                var result = new PatternResult(current.Date, name);
                results.Add(result);
                if (i > lookbackPeriod)
                {
                    var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                    if (AllCandlesBullish(previousCandles, minimunBodyInPercent))
                    {
                        var lastCandle = historyList[i - 1];
                        if (lastCandle.Close < current.Close)
                        {
                            result.Point = current.High;
                            result.Long = true;
                        }
                    }
                }
            }

            return results;
        }
    }
}
