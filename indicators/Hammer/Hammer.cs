using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Skender.Stock.Indicators;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetHammer<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 3, bool shouldOpenWithASmallGap = true, decimal minimumRatioLowerToBody = 3.0M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IQuote
        {
            //https://en.wikipedia.org/wiki/Hammer_(candlestick_pattern)
            //https://tradistats.com/kerzenchart-hammer-2/
            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();


            string name = "Hammer";
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
                    if (IsInDowntrend(historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList()))
                    {
                        //check if the candle open under the previous close after a close of the stock exchange. In crypto there is no close of the broker
                        if (!shouldOpenWithASmallGap || current.Open < previous.Close)
                        {
                            if (((current.BodyPercent * minimumRatioLowerToBody) < current.LowerWickPercent) && current.BodyPercent <= maxBodySizeInPercent)
                            {
                                result.Point = current.High;
                                result.Long = true;
                                result.Confirmed = ConfiramtionType.NotConfirmableMissingData;

                                int nextIndex = i + 1;
                                //checked for confirmation
                                if (nextIndex < historyList.Count)
                                {
                                    PatternQuote next = historyList[nextIndex];
                                    //there are 2 ways, confirm after close or confirm if the next High is higher then the current low
                                    if (next.Close > current.Low)
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
