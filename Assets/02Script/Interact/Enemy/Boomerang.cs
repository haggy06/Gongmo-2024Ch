using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackingPlayer))]
public class Boomerang : EnemyBase
{
    [SerializeField]
    private float spinSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * (spinSpeed * Time.fixedDeltaTime));
    }

    protected override void HalfHP()
    {

    }

    protected override void Pattern(int caseNumber)
    {

    }
}
