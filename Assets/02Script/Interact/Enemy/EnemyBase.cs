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
    protected List<string> patternList = new List<string>();
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

        /*
        int raw = 0;
        patternList.Add(new List<int>());
        foreach (char c in pattern)
        {
            if ('1' <= c && c <= '9') // ���ڰ� ������ ���
            {
                print(c - '0' + "�߰�");
                patternList[raw].Add(c - '0');
            }
            else if (c == ' ') // ������ ������ ���
            {
                print("�� ����");
                raw++;
                patternList.Add(new List<int>());
            }
            else // �̿��� ���ڰ� ������ ���
            {
                Debug.LogWarning(c + "�� ���� ����Ʈ�� �־ �� ��");
            }
        }
        */

        /*
        #region _Print PatternList_
        List<string> strArr = new();
        foreach (List<int> c1 in patternList)
        {
            string str = "";
            foreach (char c2 in c1)
            {
                str = str + c2;
            }
            strArr.Add(str);
        }

        string arr = "";
        foreach (string s in strArr)
        {
            arr = arr + s + ", ";
        }
        print(arr);
        #endregion
        */
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
        enemyInteract.Init();

        if (usePattern)
        {
            StabilizePattern();
            StartCoroutine("PatternRepeat");
        }
        enemyInteract.GetComponent<SpriteRenderer>().color = enemyInteract.originalColor;
    }
    protected IEnumerator PatternRepeat()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return YieldReturn.WaitForSeconds(patternTerm);

            if (usePatternList) // ���� ����Ʈ�� ����� ���
            {
                string patternArray = patternList[Random.Range(0, patternList.Count)];

                foreach (char c in patternArray)
                {
                    Pattern(c - 48, true);

                    yield return YieldReturn.WaitForSeconds(patternTermInList);
                }
            }
            else // ���� ����Ʈ�� ������� ���� ���
            {
                Pattern(Random.Range(1, patternNumber)); // ������ ���� ����
            }
        }
    }

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
    public static bool ShortDistance(Vector2 detectionCenter, float detectionRadius)
    {
        return MyCalculator.Distance(detectionCenter, PlayerController.Player.transform.position) <= detectionRadius;
    }
    public static bool LongDistance(Vector2 detectionCenter, float detectionRadius)
    {
        return MyCalculator.Distance(detectionCenter, PlayerController.Player.transform.position) > detectionRadius;
    }
}