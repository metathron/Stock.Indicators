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
     where TQuote : IQuote
        {

            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            // initialize
            List<PatternResult> results = new List<PatternResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                string name = "";
                PatternQuote current = historyList[i];

                PatternResult result = new PatternResult(current.Date, name);
                results.Add(result);
                //Is in range for Long-Legged
                if (current.BodyPercent > minBodySizeInPercent)
                {
                    if (current.Open == current.High && current.Close != current.Low)
                    {
                        name = "Marubozu open bearish";
                    }
                    if (current.Open == current.Low && current.Close != current.High)
                    {
                        name = "Marubozu open bullish";
                    }
                    if (current.Close == current.Low && current.Open != current.High)
                    {
                        name = "Marubozu close bearish";
                    }
                    if (current.Close == current.High && current.Open != current.Low)
                    {
                        name = "Marubozu close bullish";
                    }

                    if (current.Open == current.Low && current.Close == current.High)
                    {
                        name = "Marubozu full bullish";
                    }
                    if (current.Open == current.High && current.Close == current.Low)
                    {
                        name = "Marubozu full bearish";
                    }
                    if (!String.IsNullOrWhiteSpace(name))
                    {
                        result.Point = current.High;
                        result.Source = name;
                    }
                }

            }

            return results;
        }
    }
}
