using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skender.Stock.Indicators;

namespace Stock.CandleStickPatterns
{
    public class PatternQuote : Quote, IPatternQuote
    {

        public decimal UpperWickSize
        {
            get
            {
                return High - (Open > Close ? Open : Close);
            }
        }
        public decimal LowerWickSize
        {
            get
            {
                return (Open > Close ? Close : Open) - Low;
            }
        }
        public decimal BodySize
        {
            get
            {
                return (Open > Close) ? (Open - Close) : (Close - Open);
            }
        }
        public decimal CandleSize
        {
            get
            {
                return High - Low;
            }
        }

        public decimal UpperWickPercent
        {
            get
            {
                if (CandleSize > 0)
                    return UpperWickSize * (100 / CandleSize);
                else
                    return 0;
            }
        }
        public decimal LowerWickPercent
        {
            get
            {
                if (CandleSize > 0)
                    return LowerWickSize * (100 / CandleSize);
                else
                    return 0;
            }
        }
        public decimal BodyPercent
        {
            get
            {
                if (CandleSize > 0)
                    return BodySize * (100 / CandleSize);
                else
                    return 0;
            }
        }

        public bool IsBullish
        {
            get
            {
                return Close > Open;
            }
        }

        public bool IsBearish
        {
            get
            {
                return Close < Open;
            }
        }

    }
}
