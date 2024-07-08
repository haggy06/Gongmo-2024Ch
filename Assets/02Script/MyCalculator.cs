using UnityEngine;
public static class MyCalculator
{
    public static bool CompareFlag(int flag1, int flag2)
    {
        return (flag1 & flag2) != 0 ? true : false;
    }

    public static float Distance(Vector2 pos1, Vector2 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
    }

    public static Vector2 Deg2Vec(float deg)
    {
        return new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad)).normalized;
    }
    public static float Vec2Deg(Vector2 vec)
    {
        vec = vec.normalized;
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    public static float SinWave(float time, float peek1, float peek2)
    {
        return Mathf.Cos(time * Mathf.PI) * (peek1 - peek2) / 2f + (peek1 + peek2) / 2f;
    }
    public static Color SinWave(float time, Color peek1, Color peek2)
    {
        Color value;

        value.r = Mathf.Cos(time * Mathf.PI) * (peek1.r - peek2.r) / 2f + (peek1.r + peek2.r) / 2f;
        value.g = Mathf.Cos(time * Mathf.PI) * (peek1.g - peek2.g) / 2f + (peek1.g + peek2.g) / 2f;
        value.b = Mathf.Cos(time * Mathf.PI) * (peek1.b - peek2.b) / 2f + (peek1.b + peek2.b) / 2f;
        value.a = Mathf.Cos(time * Mathf.PI) * (peek1.a - peek2.a) / 2f + (peek1.a + peek2.a) / 2f;

        return value;
    }
}