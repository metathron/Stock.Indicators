using System;

namespace Skender.Stock.Indicators
{
    // MODELS

    public interface IQuote
    {
        public DateTime Date { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public decimal Volume { get; }
    }

    [Serializable]
    public class Quote : IQuote
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }

    [Serializable]
    public class ResultBase
    {
        public DateTime Date { get; set; }
    }

    [Serializable]
    internal class BasicData
    {
        internal DateTime Date { get; set; }
        internal decimal Value { get; set; }
    }


    // ENUMERATIONS

    public enum PeriodSize
    {
        //
        // Summary:
        //     1m
        OneMinute = 0,
        //
        // Summary:
        //     3m
        ThreeMinutes = 1,
        //
        // Summary:
        //     5m
        FiveMinutes = 2,
        //
        // Summary:
        //     15m
        FifteenMinutes = 3,
        //
        // Summary:
        //     30m
        ThirtyMinutes = 4,
        //
        // Summary:
        //     1h
        Hour = 5,
        //
        // Summary:
        //     2h
        TwoHour = 6,
        //
        // Summary:
        //     4h
        FourHour = 7,
        //
        // Summary:
        //     6h
        SixHour = 8,
        //
        // Summary:
        //     8h
        EightHour = 9,
        //
        // Summary:
        //     12h
        TwelveHour = 10,
        //
        // Summary:
        //     1d
        Day = 11,
        //
        // Summary:
        //     3d
        ThreeDay = 12,
        //
        // Summary:
        //     1w
        Week = 13,
        //
        // Summary:
        //     1M
        Month = 14
    }
}
