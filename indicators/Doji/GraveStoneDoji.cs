
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        //https://www.investopedia.com/terms/d/doji.asp
        public static IEnumerable<PatternResult> GetGraveStoneDoji<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 0.10M, decimal maxLowerRegionInPercent = 10, decimal minimumCandleSizeInPercent = 0.5M)
             where TQuote : IPatternQuote
        {
            //https://www.youtube.com/watch?v=fY-j26ozA2w
            //if(and(abs(Open[i]-Close[i])<Close*(maxBodySizeInPercent/100), High[i]-Low[i]>Close[i]*(minimumCandleSizeInPercent/100)

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "GraveStoneDoji";
            // initialize
            List<PatternResult> results = new List<PatternResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                var maxBodyCandleMatch = Math.Abs(h.Open - h.Close) < h.Close * (maxBodySizeInPercent / 100);
                var minimumCandleMatch = h.High - h.Low > h.Close * (minimumCandleSizeInPercent / 100);
                if (maxBodyCandleMatch && minimumCandleMatch)
                {

                    //Is in range for GraveStoneDoji

                    if (h.LowerWickPercent < maxLowerRegionInPercent)
                    {
                        PatternResult result = new PatternResult(h, name)
                        {
                            Date = h.Date,
                        };
                        results.Add(result);
                    }
                }
            }

            return results;
        }


    }
}

