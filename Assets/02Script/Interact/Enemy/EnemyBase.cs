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
    [SerializeField, Tooltip("���� ����Ʈ ��� ����. �̻�� �� �ϳ��� ������ �����ϰ� ����")]
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

        StopCoroutine("PatternRepeat");
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        StartCoroutine("PatternRepeat");
    }
    private IEnumerator PatternRepeat()
    {
        while (gameObject.activeSelf)
        {
            yield return YieldReturn.WaitForSeconds(patternTerm);

            if (usePatternList) // ���� ����Ʈ�� ����� ���
            {
                int[] patternArray = patternList[Random.Range(0, patternList.Count)];

                foreach (int caseNumber in patternArray)
                {
                    this.caseNumber = caseNumber;
                    Pattern(this.caseNumber, true);

                    yield return YieldReturn.WaitForSeconds(patternTermInList);
                }
            }
            else // ���� ����Ʈ�� ������� ���� ���
            {
                caseNumber = Random.Range(0, patternNumber);
                Pattern(caseNumber); // ������ ���� ����
            }
        }
    }

    protected int caseNumber;
    protected abstract void HalfHP();
    protected abstract void MoribundHP();
    protected abstract void Dead(AttackBase attack);
    protected abstract void Pattern(int caseNumber, bool isListPattern = false);
}

public static class PatternCheck
{
    public static bool shortDistance(Vector2 detectionCenter, float detectionRadius)
    {
        return MyCalculator.Distance(detectionCenter, PlayerController.Player.transform.position) <= detectionRadius;
    }
    public static bool LongDistance(Vector2 detectionCenter, float detectionRadius)
    {
        return MyCalculator.Distance(detectionCenter, PlayerController.Player.transform.position) > detectionRadius;
    }
}