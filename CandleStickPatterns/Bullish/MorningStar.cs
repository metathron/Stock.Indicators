using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    //https://www.youtube.com/watch?v=o65YF_xbmPs
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetMorningStar<TQuote>(
        IEnumerable<TQuote> history,
        int lookbackPeriod = 10, decimal previousMinimunBodyInPercent = 80.0M, decimal minimumRatioStarBodyToPreviousBody = 3.0M)
        where TQuote : IPatternQuote
        {
            //https://www.investopedia.com/terms/m/morningstar.asp

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "MorningStar";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 1; i < historyList.Count; i++)
            {
                if (historyList.Count > i + 1)
                {
                    TQuote previous = historyList[i - 1];

                    TQuote h = historyList[i];
                    TQuote next = historyList[i + 1];

                    if (i > lookbackPeriod)
                    {
                        var isLowestValue = (h.Open > h.Close ? h.Open : h.Close) < historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).Min(m => m.Open > m.Close ? m.Open : m.Close);
                        if (isLowestValue)
                        {
                            //is current bearish && previous is bearish && previous x times bigger
                            if (previous.IsBearish && h.IsBearish && previous.BodyPercent > previousMinimunBodyInPercent && (h.BodySize * minimumRatioStarBodyToPreviousBody < previous.BodySize))
                                //check is next bullish 
                                if (next.IsBullish && previous.Close > h.High && h.Close > next.Open)
                                {
                                    //nextDay close higher than middle of first day
                                    if ((previous.Low + (previous.CandleSize / 2)) < next.Close)
                                    {
                                        PatternResult result = new PatternResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                                        {
                                            Date = h.Date,
                                            IsBull = true
                                        };
                                        results.Add(result);
                                    }
                                }
                        }
                    }
                }
            }

            return results;
        }
    }
}
