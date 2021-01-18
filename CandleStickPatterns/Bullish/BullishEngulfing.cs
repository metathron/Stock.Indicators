﻿
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Stock.CandleStickPatterns
{
    public static partial class Indicator
    {
        public static IEnumerable<PatternResult> GetBullishEngulfing<TQuote>(
            IEnumerable<TQuote> history)
            where TQuote : IPatternQuote
        {
            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            string name = "BullishEngulfing";
            // check parameters
            ValidateDataForPattern(history, 2, name);

            // initialize
            List<PatternResult> results = new List<PatternResult>();// (historyList.Count);

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                if (i >= 1)
                {
                    TQuote previous = historyList[i - 1];
                    var firstCandle = previous.Close < previous.Open;
                    var secondCandle = h.Close > h.Open;

                    // check the first wholeCandle fitts in secondCandleBody
                    var firstCandleHighIsLowerThanSecondCandleOpen = previous.High < h.Close;
                    var firstCandleLowIsHigherThanSecondCandleLow = previous.Low > h.Open;
                    if (firstCandle && secondCandle && firstCandleHighIsLowerThanSecondCandleOpen && firstCandleLowIsHigherThanSecondCandleLow)
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
            return results;
        }
    }
}
