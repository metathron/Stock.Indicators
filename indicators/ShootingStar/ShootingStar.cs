
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetShootingStar<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 5, decimal minimumRatioUpperToBody = 2.2M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IPatternQuote
        {
            //https://en.wikipedia.org/wiki/Shooting_star_(candlestick_pattern)

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "ShootingStar";
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
                    if (IsInUptrend(historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList()))
                    {
                        if (((h.BodyPercent * minimumRatioUpperToBody) < h.UpperWickPercent) && h.BodyPercent <= maxBodySizeInPercent && h.UpperWickPercent>h.LowerWickPercent)
                        {
                            //check Boby is in upper region, Upper Region is maxBodySizeInPercent plus 20%
                            if (h.LowerWickPercent < (maxBodySizeInPercent * 1.2M))
                            {
                                PatternResult result = new PatternResult(h, name)
                                {
                                    Date = h.Date,
                                    Short = true
                                };
                                results.Add(result);
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}
