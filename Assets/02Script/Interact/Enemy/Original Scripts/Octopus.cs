using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScrollWithBackground))]
public class Octopus : EnemyBase
{
    [SerializeField]
    private bool hiding;


    protected override void ObjectReturned()
    {
        base.ObjectReturned();

        
    }
    protected override void HalfHP()
    {

    }
    protected override void MoribundHP()
    {

    }
    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {

    }
}
