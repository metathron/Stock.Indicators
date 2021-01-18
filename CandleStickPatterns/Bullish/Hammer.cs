using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Skender.Stock.Indicators;

namespace Stock.CandleStickPatterns
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetHammer<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 10, decimal minimumRatioLowerToBody = 3.0M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IPatternQuote
        {
            //https://en.wikipedia.org/wiki/Hammer_(candlestick_pattern)
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();


            string name = "Hammer";
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
                    if (IsInDowntrend(historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList()))
                    {
                        if (((h.BodyPercent * minimumRatioLowerToBody) < h.LowerWickPercent) && h.BodyPercent <= maxBodySizeInPercent)
                        {
                            PatternResult result = new PatternResult(h, name)
                            {
                                Date = h.Date,
                                IsBull = true
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