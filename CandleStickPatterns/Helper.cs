using Skender.Stock.Indicators;
using Stock.CandleStickPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public static partial class Indicator
    {
        private static void ValidateDataForPattern<TQuote>(IEnumerable<TQuote> history, int lookbackPeriod, string name) where TQuote : IPatternQuote
        {

            // check parameters
            if (lookbackPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lookbackPeriod), lookbackPeriod,
                    $"Lookback period must be greater than 0 for {name}.");
            }

            // check history
            int qtyHistory = history.Count();
            int minHistory = lookbackPeriod;
            if (qtyHistory < minHistory)
            {
                string message = $"Insufficient history provided for {name}.  " +
                    string.Format("You provided {0} periods of history when at least {1} is required.",
                    qtyHistory, minHistory);

                throw new BadHistoryException(nameof(history), message);
            }

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
        private static bool AllCandlesBearish<TQuote>(List<TQuote> previousCandles, decimal minimunBodyInPercent) where TQuote : IPatternQuote
        {
            return previousCandles.All(c => c.IsBearish && c.BodyPercent >= minimunBodyInPercent);
        }
        private static bool AllCandlesBearish<TQuote>(List<TQuote> previousCandles) where TQuote : IPatternQuote
        {
            return previousCandles.All(c => c.IsBearish);
        }
        private static bool AllCandlesBullish<TQuote>(List<TQuote> previousCandles, decimal minimunBodyInPercent) where TQuote : IPatternQuote
        {
            return previousCandles.All(c => c.IsBullish && c.BodyPercent >= minimunBodyInPercent);
        }
        private static bool AllCandlesBullish<TQuote>(List<TQuote> previousCandles) where TQuote : IPatternQuote
        {
            return previousCandles.All(c => c.IsBullish);
        }
    }
}
