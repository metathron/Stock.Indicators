using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetDarkCloudCover<TQuote>(
          IEnumerable<TQuote> history,
          int lookbackPeriod = 3, bool shouldOpenWithABigGap = false, decimal minBodySizeInPercent = 50.0M)
          where TQuote : IPatternQuote
        {
            //https://admiralmarkets.com/de/wissen/articles/forex-basics/alles-was-sie-uber-candlesticks-wissen-mussen
            //https://tradistats.com/dark-cloud-cover-und-piercing-pattern/

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
                if (previous.BodyPercent >= minBodySizeInPercent)
                    if (i > lookbackPeriod + 1)
                    {
                        var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                        if (AllCandlesBullish(previousCandles))
                        {
                            if (!shouldOpenWithABigGap || current.Open > previous.High)
                            {
                                //is previous bullish
                                if (previous.IsBullish)
                                    //check is next bearish
                                    if (current.IsBearish && previous.Close < current.Open)
                                    {
                                        var fiftyPercentOfLastBody = previous.BodySize / 2 + previous.Low + previous.LowerWickSize;
                                        if (current.Close < fiftyPercentOfLastBody)
                                        {
                                            ConfiramtionType confirmed = ConfiramtionType.NotConfirmableMissingData;
                                            //Check is Confirmation possible
                                            if (i < (historyList.Count - 1))
                                            {
                                                var confirmationCandle = historyList[i + 1];
                                                if (confirmationCandle.Close < current.Close)
                                                    confirmed = ConfiramtionType.Confirmed;
                                                else
                                                    confirmed = ConfiramtionType.NotConfirmed;
                                            }

                                            PatternResult result = new PatternResult(current, name)
                                            {
                                                Date = current.Date,
                                                Short = true,
                                                Confirmed = confirmed
                                            };
                                            results.Add(result);
                                        }
                                    }
                            }
                        }
                    }

            }

            return results;
        }
    }
}
