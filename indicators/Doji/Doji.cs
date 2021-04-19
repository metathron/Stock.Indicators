using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        //https://www.investopedia.com/terms/d/doji.asp
        public static IEnumerable<PatternResult> GetDoji<TQuote>(
         IEnumerable<TQuote> history, decimal maxBodySizeInPercent = 10.0M)
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
                PatternResult result = new PatternResult(current.Date, name);
                results.Add(result);
                //Is in range for Long-Legged
                if (IsDoji(current, maxBodySizeInPercent))
                {
                    result.Point = current.High;
                }
            }

            return results;
        }
        internal static bool IsDoji(PatternQuote h, decimal maxBodySizeInPercent = 10.0M)
        {
            bool result = false;

            if (h.BodyPercent <= maxBodySizeInPercent)
            {
                result = true;
            }
            return result;
        }
    }
}
