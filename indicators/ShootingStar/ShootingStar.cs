
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetShootingStar<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 3, bool shouldOpenWithASmallGap = true, decimal minimumRatioUpperToBody = 2.2M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IQuote
        {
            //https://en.wikipedia.org/wiki/Shooting_star_(candlestick_pattern)
            //https://tradistats.com/shooting-star/

            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "ShootingStar";
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

                if (i > lookbackPeriod)
                {
                    if (IsInUptrend(historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList()))
                    {
                        //check if the candle open above the previous close after a close of the broker. In cryptoe there is no close of the broker
                        if (!shouldOpenWithASmallGap || previous.Close < current.Open)
                        {
                            if (((current.BodyPercent * minimumRatioUpperToBody) < current.UpperWickPercent) && current.BodyPercent <= maxBodySizeInPercent && current.UpperWickPercent > current.LowerWickPercent)
                            {
                                //check Boby is in lower region
                                if (current.LowerWickPercent < (maxBodySizeInPercent * 1.2M))
                                {
                                    result.Point = current.High;
                                    result.Short = true;
                                    result.Confirmed = ConfiramtionType.NotConfirmableMissingData;
                                    

                                    int nextIndex = i + 1;
                                    //checked for confirmation
                                    if (nextIndex < historyList.Count)
                                    {
                                        PatternQuote next = historyList[nextIndex];
                                        //there are 2 ways, confirm after close or confirm if the next low is lower then the current low
                                        if (next.Close < current.Low)
                                            result.Confirmed = ConfiramtionType.Confirmed;
                                        else
                                            result.Confirmed = ConfiramtionType.NotConfirmed;
                                    }

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
