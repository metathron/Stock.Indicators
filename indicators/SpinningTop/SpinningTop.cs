using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Skender.Stock.Indicators
{
    //https://www.investopedia.com/terms/s/spinning-top.asp
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetSpinningTop<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 10.0M, decimal longLegerRegionInPercent = 30.0M)
             where TQuote : IQuote
        {
            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            // initialize
            List<PatternResult> results = new List<PatternResult>();
            string name = "SpinningTop";
            // roll through history
            for (int i = 1; i < historyList.Count; i++)
            {
                PatternQuote current = historyList[i];

                var result = new PatternResult(current.Date, name);
                results.Add(result);

                if (i == 0)
                {
                    continue;
                }
                PatternQuote previous = historyList[i - 1];

                if ((Math.Abs(current.High - previous.High) / 100) < 0.5M) // the differenz between this two High should only be 0.5%
                {
                    if ((Math.Abs(current.Low - previous.Low) / 100) < 0.5M) // the differenz between this two Lows should only be 0.5%
                    {
                        if (IsLongLegedDoji(current, maxBodySizeInPercent, longLegerRegionInPercent) && IsLongLegedDoji(previous, maxBodySizeInPercent, longLegerRegionInPercent))
                        {
                            result.Point = current.High;
                        }
                    }
                }
            }
            return results;
        }

    }
}
