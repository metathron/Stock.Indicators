
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetHangingMan<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 3, bool shouldOpenWithASmallGap = true, decimal minimumRatioLowerToBody = 2.2M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IQuote
        {
            //https://en.wikipedia.org/wiki/Hanging_man_(candlestick_pattern)
            //https://tradistats.com/hanging-man-und-inverted-hammer/
            // clean quotes


            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "HangingMan";
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
                        //check if the candle open above the previous close after a close of the stock exchange. In cryptoe there is no close of the broker
                        if (!shouldOpenWithASmallGap || previous.Close < current.Open)
                        {
                            if (((current.BodyPercent * minimumRatioLowerToBody) < current.LowerWickPercent) && current.BodyPercent <= maxBodySizeInPercent && current.UpperWickPercent < current.LowerWickPercent)
                            {
                                //check Boby is in upper region, Upper Region is maxBodySizeInPercent plus 20%
                                if (current.UpperWickPercent < (maxBodySizeInPercent * 1.2M))
                                {
                                    result.Point = current.High;
                                    result.Short = true;
                                    result.Confirmed = ConfiramtionType.NotConfirmableMissingData;
                                }

                                int nextIndex = i + 1;
                                //checked for confirmation
                                if (nextIndex < historyList.Count)
                                {
                                    PatternQuote next = historyList[nextIndex];
                                    //there are 2 ways, confirm if the next close ends in the lower shadow  or next opens under current close
                                    if (next.Close < current.Close)
                                        result.Confirmed = ConfiramtionType.Confirmed;
                                    else
                                        result.Confirmed = ConfiramtionType.NotConfirmed;
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

