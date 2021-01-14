

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public class DragonflyDoji
    {
        //https://www.investopedia.com/terms/d/doji.asp
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 0.10M, decimal maxUpperRegionInPercent = 10, decimal minimumCandleSizeInPercent = 0.5M)
             where TQuote : IPatternQuote
        {
            //https://www.youtube.com/watch?v=fY-j26ozA2w
            //if(and(abs(Open[i]-Close[i])<Close*(maxBodySizeInPercent/100), High[i]-Low[i]>Close[i]*(minimumCandleSizeInPercent/100)

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // initialize
            List<SignalResult> results = new List<SignalResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                var maxBodyCandleMatch = Math.Abs(h.Open - h.Close) < h.Close * (maxBodySizeInPercent / 100);
                var minimumCandleMatch = h.High - h.Low > h.Close * (minimumCandleSizeInPercent / 100);
                if (maxBodyCandleMatch && minimumCandleMatch)
                {
                    //Is in range for DragonFlyDoji

                    if (h.UpperWickPercent < maxUpperRegionInPercent)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            //Source = "DragonFlyDoji"
                        };
                        results.Add(result);
                    }
                }
            }

            return results;
        }


    }
}

