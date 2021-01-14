using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    //https://www.youtube.com/watch?v=o65YF_xbmPs
    public class EveningStar
    {
        public const string SOURCE= "EveningStar";
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
        IEnumerable<TQuote> history,
        int lookbackPeriod = 10, decimal previousMinimunBodyInPercent = 80.0M, decimal minimumRatioStarBodyToPreviousBody = 3.0M)
        where TQuote : IPatternQuote
        {
            //https://www.investopedia.com/terms/e/eveningstar.asp

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // check parameters
            ValidateData(history, lookbackPeriod);

            // initialize
            List<SignalResult> results = new List<SignalResult>();// (historyList.Count);

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
                                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                                        {
                                            Date = h.Date,
                                            //Source = SOURCE
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

        private static void ValidateData<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    $"Lookback period must be greater than 0 for {SOURCE}.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = $"Insufficient history provided for {SOURCE}.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

        }
    }
}
