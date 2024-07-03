using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : PoolObject
{
    [SerializeField]
    protected int patternNumber = 1;
    [SerializeField]
    protected float patternTerm = 2f;
    
    [Space(5)]
    [SerializeField, Tooltip("패턴 리스트 사용 여부. 미사용 시 하나의 패턴이 랜덤하게 나옴")]
    protected bool usePatternList = false;
    [SerializeField]
    protected List<int[]> patternList = new List<int[]>();
    [SerializeField]
    protected float patternTermInList = 1f;

    protected Rigidbody2D rigid2D;
    protected EnemyInteract enemyInteract;
    protected virtual void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        enemyInteract = GetComponentInChildren<EnemyInteract>();

        enemyInteract.HalfHPEvent += HalfHP;
        enemyInteract.MoribundHPEvent += MoribundHP;
        enemyInteract.DeadEvent += Dead;
    }
    protected override void ObjectReturned()
    {
        base.ObjectReturned();


    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);


    }
    private IEnumerator PatternRepeat()
    {
        while (gameObject.activeSelf)
        {
            yield return YieldReturn.WaitForSeconds(patternTerm);

            if (usePatternList) // 패턴 리스트를 사용할 경우
            {
                int[] patternArray = patternList[Random.Range(0, patternList.Count)];

                foreach (int caseNumber in patternArray)
                {
                    Pattern(caseNumber);

                    yield return YieldReturn.WaitForSeconds(patternTermInList);
                }
            }
            else // 패턴 리스트를 사용하지 않을 경우
            {
                Pattern(Random.Range(0, patternNumber)); // 랜덤한 패턴 실행
            }
        }
    }

    protected abstract void HalfHP();
    protected abstract void MoribundHP();
    protected abstract void Dead(AttackBase attack);
    protected abstract void Pattern(int caseNumber);
}
