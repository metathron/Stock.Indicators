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
        where TQuote : IPatternQuote
        {
            //https://www.investopedia.com/terms/t/three_black_crows.asp
            int lookbackPeriod = 3;
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "ThreeWhiteSoldiers";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];
                if (i > lookbackPeriod)
                {
                    var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                    if (AllCandlesBullish(previousCandles, minimunBodyInPercent))
                    {
                        var lastCandle = historyList[i - 1];
                        if (lastCandle.Close < h.Close)
                        {
                            PatternResult result = new PatternResult(h, name)
                            {
                                Date = h.Date,
                                Long = true
                            };
                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }
    }
}
