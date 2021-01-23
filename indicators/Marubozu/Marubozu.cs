using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetMarubozu<TQuote>(
     IEnumerable<TQuote> history,
     decimal minBodySizeInPercent = 60.0M)
     where TQuote : IPatternQuote
        {

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // initialize
            List<PatternResult> results = new List<PatternResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                string name = "";
                TQuote h = historyList[i];

                //Is in range for Long-Legged
                if (h.BodyPercent > minBodySizeInPercent)
                {
                    if (h.Open == h.High && h.Close != h.Low)
                    {
                        name = "Marubozu open bearish";
                    }
                    if (h.Open == h.Low && h.Close != h.High)
                    {
                        name = "Marubozu open bullish";
                    }
                    if (h.Close == h.Low && h.Open != h.High)
                    {
                        name = "Marubozu close bearish";
                    }
                    if (h.Close == h.High && h.Open != h.Low)
                    {
                        name = "Marubozu close bullish";
                    }

                    if (h.Open == h.Low && h.Close == h.High)
                    {
                        name = "Marubozu full bullish";
                    }
                    if (h.Open == h.High && h.Close == h.Low)
                    {
                        name = "Marubozu full bearish";
                    }
                    if (!String.IsNullOrEmpty(name))
                    {
                        PatternResult result = new PatternResult(h, name)
                        {
                            Date = h.Date
                        };
                        results.Add(result);
                    }
                }

            }

            return results;
        }
    }
}
