using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    //https://www.youtube.com/watch?v=o65YF_xbmPs
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetEveningStar<TQuote>(
        IEnumerable<TQuote> history,
        int lookbackPeriod = 10, decimal previousMinimunBodyInPercent = 80.0M, decimal minimumRatioStarBodyToPreviousBody = 3.0M)
        where TQuote : IPatternQuote
        {
            //https://www.investopedia.com/terms/e/eveningstar.asp

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();


            string name = "EveningStar";
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
                        var isHighestValue = h.High > historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).Max(m => m.High);
                        if (isHighestValue)
                        {
                            //is current bullish && previous is bullish && previous x times bigger
                            if (previous.IsBullish && h.IsBullish && previous.BodyPercent > previousMinimunBodyInPercent && (h.BodySize * minimumRatioStarBodyToPreviousBody < previous.BodySize))
                                //check is next bearish  && 
                                if (next.IsBearish && previous.High < h.Low && h.Open > next.Open)
                                {
                                    //nextDay close lower than middle of first day
                                    if ((previous.Low + (previous.CandleSize / 2)) > next.Close)
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
            }

            return results;
        }
    }
}
