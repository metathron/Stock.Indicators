using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetPiercingLine<TQuote>(
           IEnumerable<TQuote> history,
           int lookbackPeriod = 3, bool shouldOpenWithABigGap = true, decimal minBodySizeInPercent = 50.0M)
           where TQuote : IQuote
        {
            //http://tutorials.topstockresearch.com/candlestick/Bullish/BullishPiercing/TutotrialOnBullishPiercingChartPattern.html
            //https://tradistats.com/dark-cloud-cover-und-piercing-pattern/
            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "PiercingLine";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                PatternQuote current = historyList[i];
                var result = new PatternResult(current.Date, name);
                results.Add(result);

                if (i == 0)
                {
                    continue;
                }

                PatternQuote previous = historyList[i - 1];
                if (previous.BodyPercent >= minBodySizeInPercent)
                    if (i > lookbackPeriod)
                    {
                        var previousCandles = historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList();
                        if (AllCandlesBearish(previousCandles))
                        {
                            if (!shouldOpenWithABigGap || current.Open < previous.Low)
                            {
                                if (previous.Close > current.Open && current.IsBullish)
                                {
                                    var fiftyPercentOfLastBody = previous.BodySize / 2 + previous.Low + previous.LowerWickSize;
                                    if (current.Close > fiftyPercentOfLastBody)
                                    {
                                        ConfiramtionType confirmed = ConfiramtionType.NotConfirmableMissingData;
                                        //Check is Confirmation possible
                                        if (i < (historyList.Count - 1))
                                        {
                                            var confirmationCandle = historyList[i + 1];
                                            if (confirmationCandle.Close > current.Close)
                                                confirmed = ConfiramtionType.Confirmed;
                                            else
                                                confirmed = ConfiramtionType.NotConfirmed;
                                        }

                                        result.Point = current.High;
                                        result.Long = true;
                                        result.Confirmed = confirmed;
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
