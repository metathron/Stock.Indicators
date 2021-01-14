using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public class PiercingLine
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
           IEnumerable<TQuote> history,
           int lookbackPeriod = 3)
           where TQuote : IPatternQuote
        {
            //http://tutorials.topstockresearch.com/candlestick/Bullish/BullishPiercing/TutotrialOnBullishPiercingChartPattern.html
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // check parameters
            ValidatePiercingLine(history, lookbackPeriod);

            // initialize
            List<SignalResult> results = new List<SignalResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];
                if (i > lookbackPeriod)
                {
                    var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                    if (AllCandlesBearish(previousCandles))
                    {
                        var lastCandle = historyList[i - 1];
                        if (lastCandle.Close > h.Open && h.IsBullish)
                        {
                            var fiftyPercentOfLastBody = lastCandle.BodySize / 2 + lastCandle.Low + lastCandle.LowerWickSize;
                            if (h.Close > fiftyPercentOfLastBody)
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
            }

            return results;
        }

        private static bool AllCandlesBearish<TQuote>(List<TQuote> previousCandles) where TQuote : IPatternQuote
        {
            return previousCandles.All(c => c.IsBearish);
        }

        private static void ValidatePiercingLine<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    "Lookback period must be greater than 0 for PiercingLine.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = "Insufficient history provided for PiercingLine.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

        }
    }
}
