using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Stock.CandleStickPatterns
{
    //https://www.investopedia.com/terms/s/spinning-top.asp
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetSpinningTop<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 0.10M, decimal longLegerRegionInPercent = 30, decimal minimumCandleSizeInPercent = 0.5M)
             where TQuote : IPatternQuote
        {
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // initialize
            List<PatternResult> results = new List<PatternResult>();
            string name = "SpinningTop";
            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];
                int nextIndex = i + 1;
                if (nextIndex < historyList.Count)
                {
                    TQuote next = historyList[nextIndex];
                    if ((Math.Abs(h.High - next.High) / 100) < 0.5M) // the differenz between this two High should only be 0.5%
                    {
                        if ((Math.Abs(h.Low - next.Low) / 100) < 0.5M) // the differenz between this two Lows should only be 0.5%
                        {
                            if (IsLongLegedDoji(maxBodySizeInPercent, longLegerRegionInPercent, minimumCandleSizeInPercent, h) && IsLongLegedDoji(maxBodySizeInPercent, longLegerRegionInPercent, minimumCandleSizeInPercent, next))
                            {
                                PatternResult result = new PatternResult(h, name)
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

    }
}
