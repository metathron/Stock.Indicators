using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetPiercingLine<TQuote>(
           IEnumerable<TQuote> history,
           int lookbackPeriod = 3)
           where TQuote : IPatternQuote
        {
            //http://tutorials.topstockresearch.com/candlestick/Bullish/BullishPiercing/TutotrialOnBullishPiercingChartPattern.html
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "PiercingLine";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];
                if (i > lookbackPeriod)
                {
                    var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                    if (AllCandlesBearish(previousCandles))
                    {
                        var lastCandle = historyList[i - 1];
                        if (lastCandle.Close > h.Open && h.IsBullish)
                        {
                            var fiftyPercentOfLastBody = lastCandle.BodySize / 2 + lastCandle.Low + lastCandle.LowerWickSize;
                            if (h.Close > fiftyPercentOfLastBody)
                            {
                                PatternResult result = new PatternResult(h, name)
                                {
                                    Date = h.Date,
                                    IsBull = true
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
