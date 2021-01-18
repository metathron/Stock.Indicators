
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public static partial class Indicator
    {
        //https://www.investopedia.com/terms/d/doji.asp
        public static IEnumerable<PatternResult> GetLongLeggedDoji<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 0.10M, decimal longLegerRegionInPercent = 30, decimal minimumCandleSizeInPercent = 0.5M)
             where TQuote : IPatternQuote
        {
            //https://www.youtube.com/watch?v=fY-j26ozA2w
            //if(and(abs(Open[i]-Close[i])<Close*(maxBodySizeInPercent/100), High[i]-Low[i]>Close[i]*(minimumCandleSizeInPercent/100)

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "LeggedDoji";
            // initialize
            List<PatternResult> results = new List<PatternResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                //Is in range for Long-Legged
                if (IsLongLegedDoji(maxBodySizeInPercent, longLegerRegionInPercent, minimumCandleSizeInPercent, h))
                {
                    PatternResult result = new PatternResult(h, name)
                    {
                        Date = h.Date,
                    };
                    results.Add(result);
                }

            }

            return results;
        }


        internal static bool IsLongLegedDoji<TQuote>(decimal maxBodySizeInPercent, decimal longLegerRegionInPercent, decimal minimumCandleSizeInPercent, TQuote h) where TQuote : IPatternQuote
        {
            bool result = false;
            var maxBodyCandleMatch = Math.Abs(h.Open - h.Close) < h.Close * (maxBodySizeInPercent / 100);
            var minimumCandleMatch = h.High - h.Low > h.Close * (minimumCandleSizeInPercent / 100);
            if (maxBodyCandleMatch && minimumCandleMatch)
            {
                //Is in range for Long-Legged
                if (h.UpperWickPercent > (30 - longLegerRegionInPercent))
                {
                    result = true;
                }
            }
            return result;
        }

    }
}

