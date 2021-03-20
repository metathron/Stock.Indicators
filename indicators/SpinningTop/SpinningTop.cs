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
             where TQuote : IPatternQuote
        {
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // initialize
            List<PatternResult> results = new List<PatternResult>();
            string name = "SpinningTop";
            // roll through history
            for (int i = 1; i < historyList.Count; i++)
            {
                TQuote previous = historyList[i - 1];
                TQuote current = historyList[i];

                if ((Math.Abs(current.High - previous.High) / 100) < 0.5M) // the differenz between this two High should only be 0.5%
                {
                    if ((Math.Abs(current.Low - previous.Low) / 100) < 0.5M) // the differenz between this two Lows should only be 0.5%
                    {
                        if (IsLongLegedDoji(current, maxBodySizeInPercent, longLegerRegionInPercent) && IsLongLegedDoji(previous, maxBodySizeInPercent, longLegerRegionInPercent))
                        {
                            PatternResult result = new PatternResult(current, name)
                            {
                                Date = current.Date,
                            };
                            results.Add(result);
                        }
                    }
                }
            }
            return results;
        }

    }
}
