using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnchovy : StraightMoveEnemy
{
    [Header("Summon Enchovy")]
    [SerializeField]
    private PoolObject followerAnchovy;
    [SerializeField]
    private Transform followerPositions;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        for (int i = 0; i < followerPositions.childCount; i++)
        {
            parentPool.GetPoolObject(followerAnchovy).Init(followerPositions.GetChild(i).position, transform.eulerAngles.z);
        }
    }

    protected override void HalfHP()
    {

    }

    public override void Pattern(int caseNumber)
    {

    }
}
