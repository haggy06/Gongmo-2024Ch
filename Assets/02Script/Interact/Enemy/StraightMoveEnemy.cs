using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMoveEnemy : EnemyBase
{
    [Space(5)]
    public bool lookPlayer = true;
    public float speed;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        if (lookPlayer)
            transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - transform.position);

        rigid2D.velocity = MyCalculator.Deg2Vec(transform.eulerAngles.z) * speed;
    }

    protected override void HalfHP()
    {

    }

    public override void Pattern(int caseNumber)
    {

    }
}
