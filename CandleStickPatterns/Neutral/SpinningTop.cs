using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Stock.CandleStickPatterns
{
    //https://www.investopedia.com/terms/s/spinning-top.asp
    public class SpinningTop
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 0.10M, decimal longLegerRegionInPercent = 30, decimal minimumCandleSizeInPercent = 0.5M)
             where TQuote : IPatternQuote
        {
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // initialize
            List<SignalResult> results = new List<SignalResult>();

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
                            if (LongLeggedDoji.IsLongLegedDoji(maxBodySizeInPercent, longLegerRegionInPercent, minimumCandleSizeInPercent, h) && LongLeggedDoji.IsLongLegedDoji(maxBodySizeInPercent, longLegerRegionInPercent, minimumCandleSizeInPercent, next))
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

    }
}
