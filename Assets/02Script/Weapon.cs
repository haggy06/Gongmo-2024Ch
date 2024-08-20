﻿using UnityEngine;

[System.Serializable]
public struct Weapon
{
    public string name;

    public PoolObject bullet;
    public PoolObject skill;

    public float coolDown;
    public int skillGauge;
}