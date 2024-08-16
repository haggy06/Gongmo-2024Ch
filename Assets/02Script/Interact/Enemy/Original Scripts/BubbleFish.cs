using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFish : StraightMoveEnemy
{
    [Header("Bubble Projectile")]
    [SerializeField]
    private AudioClip bubbleSound;
    [SerializeField]
    private Projectile bubble;
    [SerializeField]
    private Transform attackPosition;
    public override void Pattern(int caseNumber)
    {
        anim.SetInteger(EntityAnimHash.Pattern, 1);
    }

    public void SpitBubble()
    {
        AudioManager.Inst.PlaySFX(bubbleSound);

        parentPool.GetPoolObject(bubble).Init(attackPosition.position, transform.eulerAngles.z);

        StabilizePattern();
    }
}
