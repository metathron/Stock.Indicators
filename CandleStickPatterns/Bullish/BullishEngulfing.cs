
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Stock.CandleStickPatterns
{
    public class BullishEngulfing
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
            IEnumerable<TQuote> history)
            where TQuote : IPatternQuote
        {


            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // check parameters
            ValidateEngulfing(history, 2);

            // initialize
            List<SignalResult> results = new List<SignalResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                if (i >= 1)
                {
                    TQuote previous = historyList[i - 1];
                    var firstCandle = previous.Close < previous.Open;
                    var secondCandle = h.Close > h.Open;

                    // check the first wholeCandle fitts in secondCandleBody
                    var firstCandleHighIsLowerThanSecondCandleOpen = previous.High < h.Close;
                    var firstCandleLowIsHigherThanSecondCandleLow = previous.Low > h.Open;
                    if (firstCandle && secondCandle && firstCandleHighIsLowerThanSecondCandleOpen && firstCandleLowIsHigherThanSecondCandleLow)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                           // Source = "BullishEngulfing"
                        };
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        private static void ValidateEngulfing<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    "Lookback period must be greater than 0 for BullishEngulfing.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = "Insufficient history provided for BullishEngulfing.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

        }

    }
}
