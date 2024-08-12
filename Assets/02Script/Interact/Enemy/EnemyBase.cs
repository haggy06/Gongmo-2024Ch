using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyBase : PoolObject
{
    [SerializeField]
    private Color[] awakeColor = { Color.white };
    public Color[] AwakeColor => awakeColor;
    #region _Default Pattern Setting_
    [Header("Default Pattern Setting")]
    [SerializeField]
    private bool usePattern = true;

    [SerializeField]
    protected int patternNumber = 1;
    [SerializeField]
    protected float patternTerm = 2f;
    #endregion

    #region _Distance Pattern Setting_
    [Header("Distance Pattern Setting")]
    [SerializeField, Tooltip("플레이어와의 거리에 따른 패턴을 사용할 경우 true")]
    private bool isDistancePattern = false;
    [SerializeField]
    private float detectionRadius;
    #endregion

    #region _Pattern List Setting_
    [Header("Pattern List Setting")]
    [SerializeField, Tooltip("패턴 리스트 사용 여부. 미사용 시 하나의 패턴이 랜덤하게 나옴")]
    protected bool usePatternList = false;
    [SerializeField]
    protected List<int> patternList = new List<int>();
    [SerializeField]
    protected float patternTermInList = 1f;
    #endregion

    #region _HP Pattern Setting_
    [Header("HP Pattern Setting")]
    [SerializeField]
    private bool useHalfHPPattern = false;
    [SerializeField]
    private UnityEvent HalfHPPattern;

    [Space(10)]

    [SerializeField]
    private bool useDeadPattern = false;
    [SerializeField]
    private UnityEvent DeadPattern;
    #endregion

    #region _Attack Setting_
    [Header("Attack Setting")]
    [SerializeField]
    protected Transform attackPivot;
    [SerializeField]
    protected AudioClip attackSound;

    [Space(5)]
    [SerializeField, Tooltip("패턴의 caseNumber 순서에 맞게 이벤트 삽입")]
    private UnityEvent[] patternAttacks;

    [Space(5)]
    [SerializeField]
    private int randomNumber;
    [SerializeField]
    private Vector2 rotationRange;
    [SerializeField]
    private Vector2 positionRange;

    public int RandomNumber { set => randomNumber = value; }
    public Vector2 RotationRange { set => rotationRange = value; }
    public Vector2 PositionRange { set => positionRange = value; }


    [Space(5)]
    [SerializeField]
    private Transform spreadPositions;
    [SerializeField]
    private Transform spreadPivot;

    public Transform SpreadPositions { set => spreadPositions = value; }
    public Transform SpreadPivot { set => spreadPivot = value; }
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

        GetComponentInChildren<SpriteRenderer>().color = awakeColor[Random.Range(0, awakeColor.Length)];
        enemyInteract.Init();

        if (usePattern)
        {
            StabilizePattern();
            StartCoroutine("PatternCor");
        }
        enemyInteract.GetComponent<SpriteRenderer>().color = enemyInteract.originalColor;
    }

    private bool patternRunning = false;
    protected IEnumerator PatternCor()
    {
        yield return YieldReturn.WaitForSeconds(patternTerm);

        patternRunning = true;
        if (usePatternList) // 패턴 리스트를 사용할 경우
        {
            int patternArray = patternList[Random.Range(0, patternList.Count)];

            while (patternArray > 0)
            {
                Pattern(patternArray % 10);
                patternArray /= 10;

                yield return YieldReturn.WaitForSeconds(patternTermInList);
            }
        }
        else if (isDistancePattern) // 거리별 패턴을 사용할 경우
        {
            int caseNumber = MyCalculator.Distance(attackPivot.position, PlayerController.Inst.transform.position) <= detectionRadius ? 1 : 2;
            
            Pattern(caseNumber); // 근접 시 1번, 원거리일 시 2번 패턴 실행
        }
        else
        {
            Pattern(Random.Range(1, patternNumber)); // 랜덤한 패턴 실행
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
            HalfHPPattern.Invoke();
        }
    }
    protected virtual void Dead(EntityType killer)
    {
        if (useDeadPattern)
        {
            DeadPattern.Invoke();
        }

        ReturnToPool();
    }

    #region _Spawn Methods_
    public void SpawnObject_Down(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject.PoolObjectID);

        pObject.Init(attackPivot.position, 90f);
        if (attackSound && !pObject.AwakeSound) // enemy엔 attackSound가 있지만 pObject엔 awakeSound가 없을 경우
        {
            AudioManager.Inst.PlaySFX(attackSound);
        }
    }

    public void SpawnObject_Forward(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject.PoolObjectID);

        pObject.Init(attackPivot.position, transform.eulerAngles.z);
        if (attackSound && !pObject.AwakeSound) // enemy엔 attackSound가 있지만 pObject엔 awakeSound가 없을 경우
        {
            AudioManager.Inst.PlaySFX(attackSound);
        }
    }

    public void SpawnObject_Player(PoolObject poolObject)
    {
        PoolObject pObject = parentPool.GetPoolObject(poolObject.PoolObjectID);

        pObject.Init(attackPivot.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - attackPivot.position));
        if (attackSound && !pObject.AwakeSound) // enemy엔 attackSound가 있지만 pObject엔 awakeSound가 없을 경우
        {
            AudioManager.Inst.PlaySFX(attackSound);
        }
    }

    public void SpawnObject_Random(PoolObject poolObject)
    {
        PoolObject pObject;
        for (int i = 0; i < randomNumber; i++)
        {
            pObject = parentPool.GetPoolObject(poolObject.PoolObjectID);

            pObject.Init(attackPivot.position + Vector3.one * Random.Range(positionRange.x, positionRange.y), Random.Range(rotationRange.x, rotationRange.y));
            if (attackSound && !pObject.AwakeSound) // enemy엔 attackSound가 있지만 pObject엔 awakeSound가 없을 경우
            {
                AudioManager.Inst.PlaySFX(attackSound);
            }
        }
    }
    public void SpawnObject_Spread(PoolObject poolObject)
    {
        Transform attackPos;
        PoolObject pObject;
        for (int i = 0; i < spreadPositions.childCount; i++)
        {
            attackPos = spreadPositions.GetChild(i);
            pObject = parentPool.GetPoolObject(poolObject.PoolObjectID);

            pObject.Init(spreadPositions.GetChild(i).position, MyCalculator.Vec2Deg(spreadPivot.position - attackPos.position));
            if (attackSound && !pObject.AwakeSound) // enemy엔 attackSound가 있지만 pObject엔 awakeSound가 없을 경우
            {
                AudioManager.Inst.PlaySFX(attackSound);
            }
        }
    }
    #endregion
    #region _Ect Methods_
    public void ChangeSprite(Color color, Sprite sprite = null, AudioClip sfx = null)
    {
        SpriteRenderer sRenderer = GetComponentInChildren<SpriteRenderer>();

        sRenderer.color = color;

        if (sprite)
        {
            sRenderer.sprite = sprite;
        }
        if (sfx)
        {
            AudioManager.Inst.PlaySFX(sfx);
        }
    }

    public void PlayParticle(ParticleSystem particle)
    {
        particle.Play();
    }
    #endregion
    public void PatternInvoke()
    {
        try
        {
            patternAttacks[anim.GetInteger(EntityAnimHash.Pattern) - 1].Invoke();
        }
        catch (System.Exception)
        {
            Debug.LogError("현재 패턴에 해당하는 UnityEvent가 없거나 Pattern 상태가 0임.");
        }
    }

    public void StabilizePattern()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 0); // 패턴 안정화

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