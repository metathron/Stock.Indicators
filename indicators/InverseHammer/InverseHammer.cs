using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Skender.Stock.Indicators;

namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        //https://tradistats.com/hanging-man-und-inverted-hammer/
        public static IEnumerable<PatternResult> GetInverseHammer<TQuote>(
            IEnumerable<TQuote> history,
            int lookbackPeriod = 3, bool shouldOpenWithASmallGap = true, decimal minimumRatioLowerToBody = 3.0M, decimal maxBodySizeInPercent = 25.0M)
            where TQuote : IQuote
        {

            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "InverseHammer";
            // check parameters
            ValidateDataForPattern(history, lookbackPeriod, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                PatternQuote current = historyList[i];
                PatternResult result = new PatternResult(current.Date, name);
                results.Add(result);

                if (i == 0)
                    continue;

                PatternQuote previous = historyList[i - 1];

                if (i > lookbackPeriod)
                {
                    if (IsInDowntrend(historyList.Skip(i - (lookbackPeriod)).Take(lookbackPeriod).ToList()))
                    {
                        //check if the candle open under the previous close after a close of the broker. In crypto there is no close of the broker
                        if (!shouldOpenWithASmallGap || current.Open < previous.Close)
                        {
                            if (((current.LowerWickPercent * minimumRatioLowerToBody) < current.UpperWickPercent) && current.BodyPercent <= maxBodySizeInPercent)
                            {

                                result.Point = current.High;
                                result.Long = true;
                                result.Confirmed = ConfiramtionType.NotConfirmableMissingData;


                                int nextIndex = i + 1;
                                //checked for confirmation
                                if (nextIndex < historyList.Count)
                                {
                                    PatternQuote next = historyList[nextIndex];
                                    //there are 2 ways, confirm if the next close is above current close
                                    if (next.Close > current.Close)
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
