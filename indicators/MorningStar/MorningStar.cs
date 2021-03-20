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
        public static IEnumerable<PatternResult> GetMorningStar<TQuote>(
        IEnumerable<TQuote> history,
        int lookbackPeriod = 5, decimal previousMinimunBodyInPercent = 70.0M, decimal maximumStarBodyInPercent = 20.0M)
        where TQuote : IPatternQuote
        {
            //https://www.investopedia.com/terms/m/morningstar.asp
            //https://tradistats.com/morning-doji-star-und-evening-doji-star/

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

                    TQuote current = historyList[i];
                    TQuote next = historyList[i + 1];

                    if (i > lookbackPeriod)
                    {
                        var isLowestValue = (current.Open > current.Close ? current.Open : current.Close) < historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).Min(m => m.Open > m.Close ? m.Open : m.Close);
                        if (isLowestValue)
                        {
                            if (previous.IsBearish && previous.BodyPercent > previousMinimunBodyInPercent && IsDoji(current, maximumStarBodyInPercent) && next.IsBullish)
                            {
                                // check for Gap's
                                if (previous.Close > current.Open && next.Open > current.Close)
                                {
                                    //check if end in or above Body of previous
                                    if (next.Close > previous.Close)
                                    {
                                        PatternResult result = new PatternResult(current, name)
                                        {
                                            Date = current.Date,
                                            Long = true
                                        };
                                        results.Add(result);
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
