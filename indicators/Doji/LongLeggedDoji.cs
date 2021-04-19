
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        //https://www.investopedia.com/terms/d/doji.asp
        public static IEnumerable<PatternResult> GetLongLeggedDoji<TQuote>(
             IEnumerable<TQuote> history,
             decimal maxBodySizeInPercent = 10.0M, decimal longLegerRegionInPercent = 30.0M)
             where TQuote : IQuote
        {
            //https://www.youtube.com/watch?v=fY-j26ozA2w
            //if(and(abs(Open[i]-Close[i])<Close*(maxBodySizeInPercent/100), High[i]-Low[i]>Close[i]*(minimumCandleSizeInPercent/100)

            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "LeggedDoji";
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

                //Is in range for Long-Legged
                if (IsLongLegedDoji(current, maxBodySizeInPercent, longLegerRegionInPercent))
                {
                    result.Point = current.High;
                }
            }
            return results;
        }


        internal static bool IsLongLegedDoji(PatternQuote h, decimal maxBodySizeInPercent = 10.0M, decimal longLegerRegionInPercent = 30.0M)
        {
            bool result = false;

            if (IsDoji(h, maxBodySizeInPercent))
            {
                //Is in range for Long-Legged
                if (h.UpperWickPercent > longLegerRegionInPercent && h.LowerWickPercent > longLegerRegionInPercent)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}

