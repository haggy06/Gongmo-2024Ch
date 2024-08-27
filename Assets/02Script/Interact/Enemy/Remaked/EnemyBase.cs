using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : PoolObject
{
    public int curPattern
    {
        get
        {
            if (anim)
            {
                return anim.GetInteger(EntityAnimHash.Pattern);
            }
            else
            {
                Debug.LogError(name + "에 Animator가 없어 Pattern 확인 불가");
                return 0;
            }
        }
    }
    #region _Init Setting_
    [Header("Init Setting")]
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
    public bool UsePattern { get => usePattern; set => usePattern = value; }

    [SerializeField]
    protected int patternNumber = 1;
    [SerializeField]
    protected float patternTerm = 2f;
    public float PatternTerm { set => patternTerm = value; }
    #endregion

    #region _Distance Pattern Setting_
    [Header("Distance Pattern Setting")]
    [SerializeField, Tooltip("근접 시 1번 패턴, 원거리 시 2번 패턴 실행")]
    protected bool isDistancePattern = false;
    [SerializeField]
    protected float detectionRadius;
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
    public Transform AttackPivot { set => attackPivot = value; }

    [SerializeField]
    protected bool leaveColor = false;
    public bool LeaveColor { set => leaveColor = value; }

    [Space(5)]
    [SerializeField, Tooltip("패턴의 caseNumber 순서에 맞게 이벤트 삽입")]
    protected UnityEvent[] patternAttacks;

    [Space(5)]
    [SerializeField]
    protected bool localPosition = true;
    [SerializeField]
    protected int randomNumber;
    [SerializeField]
    protected Vector2 rotationRange;
    [SerializeField]
    protected Vector2 positionRange1;
    [SerializeField]
    protected Vector2 positionRange2;

    public int RandomNumber { set => randomNumber = value; }
    public float RotationRange { set => rotationRange = new Vector2(-value, value); }

    public float PositionRange1_X { set => positionRange1.x = -value; }
    public float PositionRange1_Y { set => positionRange1.y = -value; }
    public float PositionRange2_X { set => positionRange2.x = -value; }
    public float PositionRange2_Y { set => positionRange2.y = -value; }


    [Space(5)]
    [SerializeField]
    protected Transform scatterPositions;

    public Transform ScatterPositions { set => scatterPositions = value; }
    #endregion

    protected SpriteRenderer sprite;
    protected Animator anim;
    protected Rigidbody2D rigid2D;
    protected EnemyInteract enemyInteract;


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        enemyInteract = GetComponentInChildren<EnemyInteract>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (attackPivot == null)
            attackPivot = transform;
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
        cPatternArray = 0;

        if (initEvent != null)
        {
            initEvent.Invoke();
        }

        if (awakeColor.Length > 0)
        {
            int i = Random.Range(0, awakeColor.Length);

            sprite.color = awakeColor[i];
            enemyInteract.SaveOriginalColor();
        }

        enemyInteract.Init();

        if (usePattern)
        {
            StabilizePattern();
        }
    }

    protected int cPatternArray;
    protected IEnumerator PatternCor()
    {
        yield return YieldReturn.WaitForSeconds(patternTerm);

        if (usePatternList) // 패턴 리스트를 사용할 경우
        {
            cPatternArray = patternList[Random.Range(0, patternList.Count)];
            string patternArr = cPatternArray.ToString();

            foreach (char c in patternArr)
            {
                cPatternArray /= 10;
                Pattern(c - '0');

                yield return YieldReturn.WaitForSeconds(patternTermInList);
            }

            cPatternArray = 0;
        }
        else if (isDistancePattern) // 거리별 패턴을 사용할 경우
        {
            int caseNumber = MyCalculator.Distance(attackPivot.position, PlayerController.Inst.transform.position) <= detectionRadius ? 1 : 2;

            Pattern(caseNumber); // 근접 시 1번, 원거리일 시 2번 패턴 실행
        }
        else
        {
            Pattern(Random.Range(1, patternNumber + 1)); // 랜덤한 패턴 실행
        }
    }
    public virtual void Pattern(int caseNumber)
    {
        if (anim)
            anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
        else
            PatternInvoke_Immediate(caseNumber);
    }

    protected virtual void HalfHP()
    {
        if (useHalfHPPattern)
        {
            if (usePatternList)
                cPatternArray = 0;

            if (anim)
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
        if (!gameObject.activeInHierarchy)
            return;

        PoolObject pObject = parentPool.GetPoolObject(poolObject);

        pObject.Init(attackPivot.position, transform.eulerAngles.z);
        if (leaveColor)
        {
            SpriteRenderer sRenderer = pObject.GetComponentInChildren<SpriteRenderer>();
            if (sRenderer)
                sRenderer.color = enemyInteract.OriginalColor;
        }
    }
    public void SpawnObject_Scatter(PoolObject poolObject)
    {
        if (!gameObject.activeInHierarchy)
            return;

        Transform attackPos;
        PoolObject pObject;
        for (int i = 0; i < scatterPositions.childCount; i++)
        {
            attackPos = scatterPositions.GetChild(i);
            pObject = parentPool.GetPoolObject(poolObject);

            pObject.Init(scatterPositions.GetChild(i).position, MyCalculator.Vec2Deg(attackPos.position - scatterPositions.position));
            if (leaveColor)
            {
                SpriteRenderer sRenderer = pObject.GetComponentInChildren<SpriteRenderer>();
                if (sRenderer)
                    sRenderer.color = enemyInteract.OriginalColor;
            }
        }
    }
    public void SpawnObject_Random(PoolObject poolObject)
    {
        if (!gameObject.activeInHierarchy)
            return;

        PoolObject pObject;
        for (int i = 0; i < randomNumber; i++)
        {
            pObject = parentPool.GetPoolObject(poolObject);
            Vector3 attackPosition = new Vector3(Random.Range(positionRange1.x, positionRange2.x), Random.Range(positionRange1.y, positionRange2.y));
            if (localPosition)
                attackPosition += attackPivot.position;

            pObject.Init(attackPosition, Random.Range(rotationRange.x, rotationRange.y));
            if (leaveColor)
            {
                SpriteRenderer sRenderer = pObject.GetComponentInChildren<SpriteRenderer>();
                if (sRenderer)
                    sRenderer.color = enemyInteract.OriginalColor;
            }
        }
    }
    #endregion
    #region _Ect Methods_
    public void ChangeVelo_Player(float moveSpeed)
    {
        if (!gameObject.activeInHierarchy)
            return;

        rigid2D.velocity = (PlayerController.Inst.transform.position - transform.position).normalized * moveSpeed;
        print("속도 변경 : " + (PlayerController.Inst.transform.position - transform.position).normalized * moveSpeed);
    }
    public void SetRotation(float euler)
    {
        if (!gameObject.activeInHierarchy)
            return;

        transform.eulerAngles = Vector3.forward * euler;
    }
    public void PlaySound(AudioClip sfx)
    {
        AudioManager.Inst.PlaySFX(sfx);
    }
    public void Debug_Log(string str)
    {
        Debug.Log(str + " (" +  curPattern + ")");
    }
    #endregion
    public void PatternInvoke_Immediate(int index)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("개체 비활성화 상태에선 PatternInvoke 불가능");
            return;
        }

        if (index < 1)
        {
            Debug.LogWarning("Pattern 상태가 1 이하임. (" + index + ")");
            return;
        }

        try
        {
            patternAttacks[index - 1].Invoke();
            StabilizePattern();
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogError(gameObject.name + "의 " + index + "번 패턴에 해당하는 UnityEvent가 없음.");
        }
    }
    public void PatternInvoke(int noStabilize)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("개체 비활성화 상태에선 PatternInvoke 불가능");
            return;
        }

        if (!anim)
        {
            Debug.LogError("Animator 없이 PatternInvoke 불가능");
            return;
        }

            int patternIndex = anim.GetInteger(EntityAnimHash.Pattern);
        if (patternIndex < 1)
        {
            Debug.LogWarning("Pattern 상태가 1 이하임.");
            return;
        }


        try
        {
            patternAttacks[patternIndex - 1].Invoke();
            if (noStabilize == 0)
                StabilizePattern();
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogError(gameObject.name + "의 " + patternIndex + "번 패턴에 해당하는 UnityEvent가 없음.");
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
            Debug.LogError("현재 패턴에 해당하는 UnityEvent가 없음.");
        }
    }

    public void StabilizePattern()
    {   
        if (anim)
            anim.SetInteger(EntityAnimHash.Pattern, 0); // 패턴 안정화

        if (gameObject.activeInHierarchy && usePattern && cPatternArray == 0)
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

    Player,
    Random,

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