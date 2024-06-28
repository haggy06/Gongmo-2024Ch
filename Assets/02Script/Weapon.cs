using UnityEngine;

[System.Serializable]
public struct Weapon
{
    public string name;

    public Sprite icon;
    public PoolObject bullet;

    public Weapon(string name = "?", Sprite icon = null, PoolObject bullet = null)
    {
        this.name = name;
        this.icon = icon;
        this.bullet = bullet;
    }
}