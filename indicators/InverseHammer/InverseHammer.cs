using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Skender.Stock.Indicators;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetInverseHammer<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 5, decimal minimumRatioLowerToBody = 3.0M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IPatternQuote
        {

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "InverseHammer";
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
                        if (((h.LowerWickPercent * minimumRatioLowerToBody) < h.UpperWickPercent) && h.BodyPercent <= maxBodySizeInPercent)
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