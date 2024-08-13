using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyBase : PoolObject
{
    #region _Init Setting_
    [Header("Init Setting")]
    [SerializeField]
    protected bool useAwakeColor = false;
    [SerializeField]
    protected Color[] awakeColor;
    [Space(5)]
    [SerializeField]
    protected UnityEvent initEvent;
    #endregion

    #region _Default Pattern Setting_
    [Header("Default Pattern Setting")]
    [SerializeField]
    protected bool usePattern = true;

    [SerializeField]
    protected int patternNumber = 1;
    [SerializeField]
    protected float patternTerm = 2f;
    #endregion

    #region _Distance Pattern Setting_
    [Header("Distance Pattern Setting")]
    [SerializeField, Tooltip("�÷��̾���� �Ÿ��� ���� ������ ����� ��� true")]
    protected bool isDistancePattern = false;
    [SerializeField]
    protected float detectionRadius;
    #endregion

    #region _Pattern List Setting_
    [Header("Pattern List Setting")]
    [SerializeField, Tooltip("���� ����Ʈ ��� ����. �̻�� �� �ϳ��� ������ �����ϰ� ����")]
    protected bool usePatternList = false;
    [SerializeField]
    protected List<int> patternList = new List<int>();
    [SerializeField]
    protected float patternTermInList = 1f;
    #endregion

    #region _HP Pattern Setting_
    [Header("HP Pattern Setting")]
    [SerializeField]
    protected bool useHalfHPPattern = false;
    [SerializeField]
    protected UnityEvent HalfHPPattern;

    [Space(10)]

    [SerializeField]
    protected bool useDeadPattern = false;
    [SerializeField]
    protected UnityEvent DeadPattern;
    #endregion

    #region _Attack Setting_
    [Header("Attack Setting")]
    [SerializeField]
    protected Transform attackPivot;

    [Space(5)]
    [SerializeField, Tooltip("������ caseNumber ������ �°� �̺�Ʈ ����")]
    protected UnityEvent[] patternAttacks;

    [Space(5)]
    [SerializeField]
    protected int randomNumber;
    [SerializeField]
    protected Vector2 rotationRange;
    [SerializeField]
    protected Vector2 positionRange;

    public int RandomNumber { set => randomNumber = value; }
    public float RotationRange { set => rotationRange = new Vector2(-value, value); }
    public float PositionRange { set => positionRange = new Vector2(-value, value); }


    [Space(5)]
    [SerializeField]
    protected Transform scatterPositions;

    public Transform ScatterPositions { set => scatterPositions = value; }
    #endregion

    #region _Edge Case Setting_
    [Header("Edge Case Setting")]
    [SerializeField]
    protected UnityEvent[] edgeCasePatterns;
    #endregion

    protected Animator anim;
    protected Rigidbody2D rigid2D;
    protected EnemyInteract enemyInteract;
    

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        enemyInteract = GetComponentInChildren<EnemyInteract>();

        if (attackPivot == null)
        {
            attackPivot = transform;
        }
    }
    protected virtual void Start()
    {
        enemyInteract.HalfHPEvent += HalfHP;
        enemyInteract.DeadEvent += Dead;
    }
    protected override void ObjectReturned()
    {
        base.ObjectReturned();

        StopCoroutine("PatternCor");
    }
    protected override void DestroyByBomb()
    {
        enemyInteract.InstantKill(EntityType.Player);
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        if (initEvent != null)
            initEvent.Invoke();

        if (useAwakeColor)
            GetComponentInChildren<SpriteRenderer>().color = awakeColor[Random.Range(0, awakeColor.Length)];

        enemyInteract.Init();

        if (usePattern)
        {
            StabilizePattern();
        }
        enemyInteract.GetComponent<SpriteRenderer>().color = enemyInteract.originalColor;
    }

    protected bool patternRunning = false;
    protected IEnumerator PatternCor()
    {
        yield return YieldReturn.WaitForSeconds(patternTerm);

        patternRunning = true;
        if (usePatternList) // ���� ����Ʈ�� ����� ���
        {
            int patternArray = patternList[Random.Range(0, patternList.Count)];

            while (patternArray > 0)
            {
                Pattern(patternArray % 10);
                patternArray /= 10;

                yield return YieldReturn.WaitForSeconds(patternTermInList);
            }
        }
        else if (isDistancePattern) // �Ÿ��� ������ ����� ���
        {
            int caseNumber = MyCalculator.Distance(attackPivot.position, PlayerController.Inst.transform.position) <= detectionRadius ? 1 : 2;
            
            Pattern(caseNumber); // ���� �� 1��, ���Ÿ��� �� 2�� ���� ����
        }
        else
        {
            Pattern(Random.Range(1, patternNumber + 1)); // ������ ���� ����
        }
        patternRunning = false;
    }
    protected virtual void Pattern(int caseNumber)
    {
        anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
    }

    protected virtual void HalfHP()
    {
        if (useHalfHPPattern)
        {
            anim.SetInteger(EntityAnimHash.Pattern, 100);
            StopCoroutine("PatternCor");
        }
    }
    protected virtual void Dead(EntityType killer)
    {
        if (useDeadPattern)
        {
            DeadPattern.Invoke();
            StopCoroutine("PatternCor");
        }

        ReturnToPool();
    }

    #region _Spawn Methods_
    public void SpawnObject(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject);

        pObject.Init(attackPivot.position, 0f);
    }
    public void SpawnObject_Down(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject);

        pObject.Init(attackPivot.position, 90f);
    }

    public void SpawnObject_Forward(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject);

        pObject.Init(attackPivot.position, transform.eulerAngles.z);
    }

    public void SpawnObject_Player(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject);

        pObject.Init(attackPivot.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - attackPivot.position));
    }

    public void SpawnObject_Random(PoolObject poolObject)
    {
        PoolObject pObject;
        for (int i = 0; i < randomNumber; i++)
        {
            pObject = parentPool.GetPoolObject(poolObject);

            pObject.Init(attackPivot.position + new Vector3(Random.Range(positionRange.x, positionRange.y), Random.Range(positionRange.x, positionRange.y), 0f), Random.Range(rotationRange.x, rotationRange.y));
            pObject.GetComponentInChildren<SpriteRenderer>().color = enemyInteract.originalColor;
        }
    }
    public void SpawnObject_Scatter(PoolObject poolObject)
    {
        Transform attackPos;
        PoolObject pObject;
        for (int i = 0; i < scatterPositions.childCount; i++)
        {
            attackPos = scatterPositions.GetChild(i);
            pObject = parentPool.GetPoolObject(poolObject);

            pObject.Init(scatterPositions.GetChild(i).position, MyCalculator.Vec2Deg(attackPos.position - scatterPositions.position));
        }
    }
    #endregion
    #region _Ect Methods_
    public void ChangeSprite(Sprite sprite)
    {
        SpriteRenderer sRenderer = GetComponentInChildren<SpriteRenderer>();

        sRenderer.sprite = sprite;
    }

    public void PlaySound(AudioClip sfx)
    {
        AudioManager.Inst.PlaySFX(sfx);
    }
    public void PlayParticle(ParticleSystem particle)
    {
        particle.Play();
    }
    public void PlayCoroutine(string coroutineName)
    {
        StartCoroutine(coroutineName);
    }

    public void OFFScript(MonoBehaviour script)
    {
        script.enabled = false;
    }
    public void ONScript(MonoBehaviour script)
    {
        script.enabled = true;
    }

    public void PatternInvoke_EdgeCase(int index)
    {
        try
        {
            edgeCasePatterns[index].Invoke();
        }
        catch (System.Exception)
        {
            Debug.LogError("���� ���̽� index�� �ش��ϴ� UnityEvent�� ����.");
        }
    }
    #endregion

    public void PatternInvoke()
    {
        int patternIndex = anim.GetInteger(EntityAnimHash.Pattern);
        if (patternIndex < 1)
        {
            Debug.LogWarning("Pattern ���°� 1 ������.");
            return;
        }
        try
        {
                patternAttacks[patternIndex - 1].Invoke();
                StabilizePattern();
        }
        catch (System.Exception)
        {
            Debug.LogError(gameObject.name + "�� " + patternIndex + "�� ���Ͽ� �ش��ϴ� UnityEvent�� ����.");
        }
    }
    public void PatternInvoke_HalfHP()
    {
        try
        {
            HalfHPPattern.Invoke();
            StabilizePattern();
        }
        catch (System.Exception)
        {
            Debug.LogError("���� ���Ͽ� �ش��ϴ� UnityEvent�� ����.");
        }
    }

    public void StabilizePattern()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 0); // ���� ����ȭ

        if (usePattern && !patternRunning)
        {
            StopCoroutine("PatternCor");
            StartCoroutine("PatternCor");
        }
    }
}

public enum InitDirection
{
    None,

    Down,
    Up,

    Player
}

public static class PatternCheck
{
    public static bool ShortDistance(Vector2 detectionCenter, float detectionRadius)
    {
        return MyCalculator.Distance(detectionCenter, PlayerController.Inst.transform.position) <= detectionRadius;
    }
    public static bool LongDistance(Vector2 detectionCenter, float detectionRadius)
    {
        return MyCalculator.Distance(detectionCenter, PlayerController.Inst.transform.position) > detectionRadius;
    }
}