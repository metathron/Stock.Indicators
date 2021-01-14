using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Skender.Stock.Indicators;

namespace Stock.CandleStickPatterns
{
    public class InverseHammer
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 10, decimal minimumRatioLowerToBody = 3.0M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IPatternQuote
        {

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // check parameters
            ValidateShootingStar(history, lookbackPeriod);

            // initialize
            List<SignalResult> results = new List<SignalResult>();// (historyList.Count);

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
                            SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                            {
                                Date = h.Date,
                            };
                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }
        private static bool IsInDowntrend<TQuote>(IList<TQuote> enumerable) where TQuote : IPatternQuote
        {
            bool result = false;
            if (enumerable.Count > 1)
                for (int i = 1; i < enumerable.Count; i++)
                {
                    result = enumerable[i].Close < enumerable[i - 1].Close;
                    if (!result)
                        break;
                }
            return result;
        }
        private static void ValidateShootingStar<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    "Lookback period must be greater than 0 for ShootingStar.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = "Insufficient history provided for ShootingStar.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

        }

    }
}