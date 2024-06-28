public static class MyCalculator
{
    public static bool CompareEntity(EntityType entity1, EntityType entity2)
    {
        return ((int)entity1 & (int)entity2) != 0 ? true : false;
    }
}