using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
   public class ThreeWhiteSoldiers
    {
        public const string SOURCE = "ThreeBlackCrows";
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
        IEnumerable<TQuote> history, decimal minimunBodyInPercent = 50.0M)
        where TQuote : IPatternQuote
        {
            //https://www.investopedia.com/terms/t/three_black_crows.asp
            int lookbackPeriod = 3;
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
                    var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                    if (AllCandlesBullish(previousCandles, minimunBodyInPercent))
                    {
                        var lastCandle = historyList[i - 1];
                        if (lastCandle.Close < h.Close)
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

            return results;
        }
        private static bool AllCandlesBullish<TQuote>(List<TQuote> previousCandles, decimal minimunBodyInPercent) where TQuote : IPatternQuote
        {
            return previousCandles.All(c => c.IsBullish && c.BodyPercent >= minimunBodyInPercent);
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
