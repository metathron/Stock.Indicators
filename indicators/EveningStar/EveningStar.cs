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
        int lookbackPeriod = 5, decimal previousMinimunBodyInPercent = 80.0M, decimal maximumStarBodyInPercent = 20.0M)
        where TQuote : IQuote
        {
            //https://www.investopedia.com/terms/e/eveningstar.asp

            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();


            string name = "EveningStar";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {

                PatternQuote current = historyList[i];
                PatternResult result = new PatternResult(current.Date, name)
                {
                    Date = current.Date,
                    Long = true
                };
                results.Add(result);

                if (i == 0)
                    continue;

                if (historyList.Count > i + 1)
                {
                    PatternQuote previous = historyList[i - 1];

                    PatternQuote next = historyList[i + 1];

                    if (i > lookbackPeriod)
                    {
                        var isHighestValue = current.High > historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).Max(m => m.High);
                        if (isHighestValue)
                        {
                            if (previous.IsBullish && previous.BodyPercent > previousMinimunBodyInPercent && IsDoji(current, maximumStarBodyInPercent) && next.IsBearish)
                            {
                                // check for Gap's
                                if (previous.Close < current.Open && next.Open < current.Close)
                                {
                                    //check if end in or above Body of previous
                                    if (next.Close < previous.Close)
                                    {
                                        result.Point = current.High;
                                    }
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
