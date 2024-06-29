using UnityEngine;
public static class MyCalculator
{
    public static bool CompareEntity(EntityType entity1, EntityType entity2)
    {
        return ((int)entity1 & (int)entity2) != 0 ? true : false;
    }

    public static float Distance(Vector2 pos1, Vector2 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
    }

    public static Vector2 Deg2Vec(float deg)
    {
        return new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad));
    }
    public static float Vec2Deg(Vector2 vec)
    {
        vec = vec.normalized;
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
}