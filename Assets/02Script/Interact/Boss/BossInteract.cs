using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInteract : EnemyInteract
{
    [Header("Boss Info")]
    [SerializeField]
    private string bossName;
    public string BossName => bossName;
    public override void Hit(EntityType victim, float damage)
    {
        base.Hit(victim, damage);

        PopupManager.Inst.ChangeBossHP(curHP, maxHP);
    }
}
