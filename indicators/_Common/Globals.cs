using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Tests.Indicators")]
[assembly: InternalsVisibleTo("Tests.Performance")]

namespace Skender.Stock.Indicators
{
    /// <summary>Technical indicators and overlays.  See
    /// <see href = "https://daveskender.github.io/Stock.Indicators/docs/GUIDE.html">
    ///  the Guide</see> for more information.</summary>
    public static partial class Indicator
    {
        private static readonly CultureInfo EnglishCulture = new("en-US", false);
        private static readonly Calendar EnglishCalendar = EnglishCulture.Calendar;

        // Gets the DTFI properties required by GetWeekOfYear.
        private static readonly CalendarWeekRule EnglishCalendarWeekRule
            = EnglishCulture.DateTimeFormat.CalendarWeekRule;

        private static readonly DayOfWeek EnglishFirstDayOfWeek
            = EnglishCulture.DateTimeFormat.FirstDayOfWeek;


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
