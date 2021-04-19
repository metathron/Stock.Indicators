
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Skender.Stock.Indicators
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetBearishEngulfing<TQuote>(
            IEnumerable<TQuote> history)
            where TQuote : IQuote
        {
            // clean quotes
            List<PatternQuote> historyList = history.ConvertToPattern();

            string name = "BearishEngulfing";
            // check parameters
            ValidateDataForPattern(history, 2, name);

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
                var firstCandle = previous.Close > previous.Open;
                var secondCandle = current.Close < current.Open;

                // check the first wholeCandle fitts in secondCandleBody
                var firstCandleHighIsLowerThanSecondCandleOpen = previous.High < current.Open;
                var firstCandleLowIsHigherThanSecondCandleLow = previous.Low > current.Close;
                if (firstCandle && secondCandle && firstCandleHighIsLowerThanSecondCandleOpen && firstCandleLowIsHigherThanSecondCandleLow)
                {
                    result.Point = current.High;
                    result.Short = true;
                }

            }

            return results;
        }
    }
}
