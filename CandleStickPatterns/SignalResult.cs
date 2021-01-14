using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public class SignalResult : ResultBase
    {
        public SignalResult(IPatternQuote signal, string source)
        {
            Signal = signal;
            Source = source;
        }
        public IPatternQuote Signal { get; set; }
        public string Source { get; set; }
    }
}
