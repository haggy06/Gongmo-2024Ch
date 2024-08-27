[System.Flags]
public enum EntityType
{
    Nothing = 1 << 0,

    Enemy = 1 << 1,
    Player = 1 << 2,
    Barrier = 1 << 3,
}