using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public class PatternResult : ResultBase
    {
        public PatternResult(IPatternQuote signal, string source)
        {
            Signal = signal;
            Source = source;
        }
        public IPatternQuote Signal { get; set; }
        public bool IsBull { get; set; }
        public bool IsBear { get; set; }
        public string Source { get; set; }
    }
}
