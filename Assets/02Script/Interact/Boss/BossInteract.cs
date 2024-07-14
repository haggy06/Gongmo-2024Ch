using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInteract : EnemyInteract
{
    public override void Hit(EntityType victim, float damage, bool isSkill = false)
    {
        base.Hit(victim, damage);

        PopupManager.Inst.ChangeBossHP(curHP, maxHP);
    }
}
