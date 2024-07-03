using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : PoolObject
{
    [SerializeField]
    protected int patternNumber = 1;
    
    [Space(5)]
    [SerializeField, Tooltip("패턴 리스트 사용 여부. 미사용 시 하나의 패턴이 랜덤하게 나옴")]
    protected bool usePatternList = false;
    [SerializeField]
    protected List<int[]> patternList = new List<int[]>();

    protected EnemyInteract enemyInteract;
    protected virtual void Awake()
    {
        enemyInteract = GetComponentInChildren<EnemyInteract>();

        
    }

    protected abstract void HalfHP();
    protected abstract void MoribundHP();
    protected abstract void Dead();
    protected abstract void Pattern(int caseNumber);
}
