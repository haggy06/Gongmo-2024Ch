using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFish : StraightMoveEnemy
{
    [Header("Bubble Projectile")]
    [SerializeField]
    private Projectile bubble;
    [SerializeField]
    private Transform attackPosition;
    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        anim.SetInteger(EntityAnimHash.Pattern, 1);
    }

    public void SpitBubble()
    {
        parentPool.GetPoolObject(bubble).Init(attackPosition.position, transform.eulerAngles.z);
    }
}
