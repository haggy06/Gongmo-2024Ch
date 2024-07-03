[System.Flags]
public enum SpreadTiming
{
    Nothing = 1 << 0,

    Fire = 1 << 1,
    Attack = 1 << 2,
    TimeOut = 1 << 3,

}