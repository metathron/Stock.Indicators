using System;
using System.Collections.Generic;
using System.Linq;

namespace Skender.Stock.Indicators
{
    // RESULTS

    public interface IResultBase
    {
        public DateTime Date { get; }
    }

    [Serializable]
    public class ResultBase : IResultBase
    {
        public DateTime Date { get; set; }
    }


    public static class IndicatorResults
    {

        public static TResult Find<TResult>(
            this IEnumerable<TResult> results,
            DateTime lookupDate)
            where TResult : IResultBase
        {
            return results
                .Where(x => x.Date == lookupDate)
                .FirstOrDefault();
        }
    }
    public class PatternResult : ResultBase
    {
        public PatternResult(IPatternQuote candle, string source)
        {
            Candle = candle;
            Source = source;
        }
        public IQuote Candle { get; set; }
        public bool Long { get; set; }
        public bool Short { get; set; }
        public string Source { get; set; }
        public ConfiramtionType Confirmed {get;set;}
    }
}
