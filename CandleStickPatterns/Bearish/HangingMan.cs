
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.CandleStickPatterns
{
    public class HangingMan
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 5, decimal minimumRatioLowerToBody = 2.2M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IPatternQuote
        {
            //https://en.wikipedia.org/wiki/Hanging_man_(candlestick_pattern)

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // check parameters
            ValidateData(history, lookbackPeriod);

            // initialize
            List<SignalResult> results = new List<SignalResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                if (i > lookbackPeriod)
                {
                    if (IsInUptrend(historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList()))
                    {
                        if (((h.BodyPercent * minimumRatioLowerToBody) < h.LowerWickPercent) && h.BodyPercent <= maxBodySizeInPercent && h.UpperWickPercent < h.LowerWickPercent)
                        {
                            //check Boby is in upper region, Upper Region is maxBodySizeInPercent plus 20%
                            if (h.UpperWickPercent < (maxBodySizeInPercent * 1.2M))
                            {
                                SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                                {
                                    Date = h.Date,
                                    // Source = "HangingMan"
                                };
                                results.Add(result);
                            }
                        }
                    }
                }
            }

            return results;
        }

        private static bool IsInUptrend<TQuote>(IList<TQuote> enumerable) where TQuote : IPatternQuote
        {
            bool result = false;
            if (enumerable.Count > 1)
                for (int i = 1; i < enumerable.Count; i++)
                {
                    result = enumerable[i - 1].Close < enumerable[i].Close;
                    if (!result)
                        break;
                }
            return result;
        }

        private static void ValidateData<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    "Lookback period must be greater than 0 for HangingMan.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = "Insufficient history provided for HangingMan.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

        }

    }
}
