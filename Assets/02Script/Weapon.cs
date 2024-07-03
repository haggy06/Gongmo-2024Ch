using UnityEngine;

[System.Serializable]
public struct Weapon
{
    public string name;

    public Sprite icon;
    public PoolObject bullet;
    public PoolObject skill;

    public float coolDown;
    public int skillGauge;
    public Weapon(string name = "?", Sprite icon = null, PoolObject bullet = null, PoolObject skill = null, float coolDown = 0.1f, int skillGauge = 100)
    {
        this.name = name;
        this.icon = icon;
        this.bullet = bullet;
        this.skill = skill;
        this.coolDown = coolDown;
        this.skillGauge = skillGauge;
    }
}