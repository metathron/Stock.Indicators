

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        //https://www.investopedia.com/terms/d/doji.asp
        public static IEnumerable<PatternResult> GetDragonflyDoji<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 10.0M, decimal maxUpperRegionInPercent = 10.0M)
             where TQuote : IQuote
        {
            //https://www.youtube.com/watch?v=fY-j26ozA2w
            //if(and(abs(Open[i]-Close[i])<Close*(maxBodySizeInPercent/100), High[i]-Low[i]>Close[i]*(minimumCandleSizeInPercent/100)

            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "DragonflyDoji";
            // initialize
            List<PatternResult> results = new List<PatternResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                PatternQuote current = historyList[i];

                PatternResult result = new PatternResult(current.Date, name)
                {
                    Date = current.Date,
                };
                results.Add(result);

                if (IsDoji(current, maxBodySizeInPercent))
                {
                    //Is in range for DragonFlyDoji
                    if (current.UpperWickPercent < maxUpperRegionInPercent)
                    {
                        result.Point = current.High;
                    }
                }
            }

            return results;
        }


    }
}

