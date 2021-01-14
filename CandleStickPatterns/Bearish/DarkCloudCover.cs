using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public class DarkCloudCover
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
          IEnumerable<TQuote> history,
          int lookbackPeriod = 5)
          where TQuote : IPatternQuote
        {
            //https://admiralmarkets.com/de/wissen/articles/forex-basics/alles-was-sie-uber-candlesticks-wissen-mussen

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // check parameters
            ValidateDarkCloudCover(history, lookbackPeriod);

            // initialize
            List<SignalResult> results = new List<SignalResult>();// (historyList.Count);

            // roll through history
            for (int i = 1; i < historyList.Count; i++)
            {
                TQuote previous = historyList[i - 1];
                TQuote current = historyList[i];

                if (i > lookbackPeriod + 1)
                {
                    var isHighestValue = previous.High > historyList.Skip((i - 1) - (lookbackPeriod)).Take(lookbackPeriod).Max(m => m.High);
                    if (isHighestValue)
                    {
                        //is current bullish
                        if (previous.IsBullish)
                            //check is next bearish
                            if (current.IsBearish && previous.Close < current.Open)
                            {

                                var fiftyPercentOfLastBody = previous.BodySize / 2 + previous.Low + previous.LowerWickSize;
                                if (current.Close < fiftyPercentOfLastBody) 
                                {
                                    SignalResult result = new SignalResult(current, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                                    {
                                        Date = current.Date,
                                        //Source= "DarkCloudCover"
                                    };
                                    results.Add(result);
                                }
                            }
                    }
                }

            }

            return results;
        }

        private static void ValidateDarkCloudCover<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    "Lookback period must be greater than 0 for DarkCloudCover.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = "Insufficient history provided for DarkCloudCover.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

        }
    }
}
