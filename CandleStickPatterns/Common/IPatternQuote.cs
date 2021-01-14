using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.CandleStickPatterns
{
    public interface IPatternQuote: IQuote
    {
        decimal UpperWickSize { get; }
        decimal LowerWickSize { get; }
        decimal BodySize { get; }
        decimal CandleSize { get; }

        decimal UpperWickPercent { get; }
        decimal LowerWickPercent { get; }
        decimal BodyPercent { get; }

        bool IsBullish { get; }

        bool IsBearish { get; }
    }
}
