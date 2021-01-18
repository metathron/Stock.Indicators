using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetDarkCloudCover<TQuote>(
          IEnumerable<TQuote> history,
          int lookbackPeriod = 5)
          where TQuote : IPatternQuote
        {
            //https://admiralmarkets.com/de/wissen/articles/forex-basics/alles-was-sie-uber-candlesticks-wissen-mussen

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();


            string name = "DarkCloudCover";
            // check parameters
            ValidateDataForPattern(history, 2, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 1; i < historyList.Count; i++)
            {
                TQuote previous = historyList[i - 1];
                TQuote current = historyList[i];

                if (i > lookbackPeriod + 1)
                {
                    var isHighestValue = previous.High > historyList.Skip((i - 1) - (lookbackPeriod)).Take(lookbackPeriod).Max(m => m.High);
                    if (isHighestValue)
                    {
                        //is current bullish
                        if (previous.IsBullish)
                            //check is next bearish
                            if (current.IsBearish && previous.Close < current.Open)
                            {

                                var fiftyPercentOfLastBody = previous.BodySize / 2 + previous.Low + previous.LowerWickSize;
                                if (current.Close < fiftyPercentOfLastBody) 
                                {
                                    PatternResult result = new PatternResult(current, name)
                                    {
                                        Date = current.Date,
                                        IsBear = true
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
