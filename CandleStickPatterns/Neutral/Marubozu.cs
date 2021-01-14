using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public class Marubozu
    {
        public static IEnumerable<SignalResult> GetSignals<TQuote>(
     IEnumerable<TQuote> history,
     decimal minBodySizeInPercent = 60.0M)
     where TQuote : IPatternQuote
        {

            // clean quotes
            List<TQuote> historyList = history.OrderBy(x => x.Date).ToList();

            // initialize
            List<SignalResult> results = new List<SignalResult>();

            // roll through history
            for (int i = 0; i < historyList.Count; i++)
            {
                TQuote h = historyList[i];

                //Is in range for Long-Legged
                if (h.BodyPercent > minBodySizeInPercent)
                {
                    if (h.Open == h.High && h.Close != h.Low)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            Source = "Marubozu open bearish"
                        };
                        results.Add(result);
                    }
                    if (h.Open == h.Low && h.Close != h.High)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            Source = "Marubozu open bullish"
                        };
                        results.Add(result);
                    }

                    if (h.Close == h.Low && h.Open != h.High)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            Source = "Marubozu close bearish"
                        };
                        results.Add(result);
                    }
                    if (h.Close == h.High && h.Open != h.Low)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            Source = "Marubozu open bullish"
                        };
                        results.Add(result);
                    }

                    if ( h.Open == h.Low && h.Close == h.High)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            Source = "Marubozu full bullish"
                        };
                        results.Add(result);
                    }
                    if (h.Open == h.High && h.Close == h.Low)
                    {
                        SignalResult result = new SignalResult(h, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name)
                        {
                            Date = h.Date,
                            Source = "Marubozu full bearish"
                        };
                        results.Add(result);
                    }
                }

            }

            return results;
        }
    }
}
