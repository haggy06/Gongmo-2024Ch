using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class EnemyBase : PoolObject
{
    [SerializeField]
    private bool usePattern = true;
    [SerializeField]
    protected int patternNumber = 1;
    [SerializeField]
    protected float patternTerm = 2f;
    [SerializeField, Range(0f, 1f)]
    protected float itemDropProbability = 0.1f;
    
    [Space(5)]
    [SerializeField, Tooltip("���� ����Ʈ ��� ����. �̻�� �� �ϳ��� ������ �����ϰ� ����")]
    protected bool usePatternList = false;
    [SerializeField]
    protected List<int[]> patternList = new List<int[]>();
    [SerializeField]
    protected float patternTermInList = 1f;


    protected Animator anim;
    protected Rigidbody2D rigid2D;
    protected EnemyInteract enemyInteract;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
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
    protected override void DestroyByBomb()
    {
        enemyInteract.InstantKill(EntityType.Player);
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        
        if (usePattern)
        {
            StabilizePattern();
            StartCoroutine("PatternRepeat");
        }
        enemyInteract.GetComponent<SpriteRenderer>().color = Color.white;
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
    protected virtual void Dead(EntityType killer)
    {
        if (Random.Range(0f, 1f) < itemDropProbability) // ������ ��� Ȯ���� ��÷�Ǿ��� ���
        {
            if (Random.Range(0, 3) <= 1) // 2/3 Ȯ���� �������� ���� 
            {
                PlayerController.Player.SpawnItem(transform.position);
            }
            else // 1/3 Ȯ���� ���Ⱑ ����
            {
                PlayerController.Player.SpawnWeapon(transform.position);
            }
        }

        ReturnToPool();
    }
    protected abstract void Pattern(int caseNumber, bool isListPattern = false);
    public void StabilizePattern()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 0); // ���� ����ȭ
    }
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