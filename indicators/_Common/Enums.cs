namespace Skender.Stock.Indicators
{
    // ENUMERATIONS

    public enum MaType
    {
        ALMA,
        DEMA,
        EMA,
        HMA,
        KAMA,
        MAMA,
        SMA,
        TEMA,
        WMA
    }
    
    public enum ConfiramtionType
    {
        NotConfirmable,
        NotConfirmableMissingData,
        Confirmed,
        NotConfirmed
    }


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
