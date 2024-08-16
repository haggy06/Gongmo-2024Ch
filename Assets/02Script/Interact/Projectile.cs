using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBase), typeof(Rigidbody2D))]
public class Projectile : PoolObject
{
    #region _Projectile Setting_
    [Header("Projectile Setting")]
    [SerializeField]
    protected int attackableCount = 1;
    protected int nowAttackCount = 0;
    [SerializeField]
    protected float lifeTime = 5f;
    public float LifeTime => lifeTime;
    [SerializeField]
    protected float speed = 10f;
    #endregion

    #region _Sub Projectile Setting_
    [Header("Sub Projectile Setting")]
    [SerializeField]
    protected SpreadTiming spreadTiming = SpreadTiming.Nothing;
    [SerializeField]
    protected PoolObject subObject;
    [SerializeField]
    protected AudioClip SubProjSound;

    [Space(5)]
    [SerializeField, Tooltip("기존 방향으로도 소환할지 결정.")]
    protected bool spawnForward = false;
    [SerializeField, Tooltip("기존 방향 뒤로도 소환할지 결정.")]
    protected bool spawnBackward = false;

    [SerializeField, Tooltip("보조 발사체의 쌍 개수. 1 입력 시 투사체 좌우에 보조 발사체가 하나씩 소환됨.")]
    protected int projPair = 1;
    [SerializeField]
    protected float angleDiff;
    #endregion

    #region _Tracking Setting_
    [Header("Tracking Setting")]
    [SerializeField]
    protected bool useTracking = false;

    [Space(5)]
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected EntityType targetEntity;

    [Space(5)]
    [SerializeField, Range(0.02f, 2f), Tooltip("방향을 꺾는 데 소요되는 시간")]
    protected float driftTime = 1f;
    [SerializeField]
    protected float searchRadius = 5f;
    #endregion

    protected AttackBase attack;
    protected Rigidbody2D rigid2D;
    protected virtual void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        attack = GetComponent<AttackBase>();
        attack.AttackSuccessEvent += AttackSuccess;
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        nowAttackCount = 0;
        target = null;
        if (MyCalculator.CompareFlag((int)spreadTiming, (int)SpreadTiming.Fire)) // 발사 시 보조 발사가 실행될 경우
        {
            SpawnSubProj();
        }

        rigid2D.velocity = MyCalculator.Deg2Vec(transform.eulerAngles.z) * speed;


        StartCoroutine("AutoReturn");
    }
    protected virtual IEnumerator AutoReturn()
    {
        yield return YieldReturn.WaitForSeconds(lifeTime);

        if (gameObject.activeInHierarchy)
        {
            if (MyCalculator.CompareFlag((int)spreadTiming, (int)SpreadTiming.TimeOut)) // 타임오버 시 보조 발사가 실행될 경우
                SpawnSubProj();

            ReturnToPool();
        }
    }

    public override void ReturnToPool()
    {
        StopCoroutine("AutoReturn");
        base.ReturnToPool();
    }

    protected virtual void AttackSuccess(HitBase hitBase)
    {
        nowAttackCount++;
        if (nowAttackCount >= attackableCount)
        {
            if (MyCalculator.CompareFlag((int)spreadTiming, (int)SpreadTiming.Attack)) // 공격 성공 시 보조 발사가 실행될 경우
                SpawnSubProj();

            ReturnToPool();
        }
    }
    protected override void ObjectReturned()
    {
        rigid2D.velocity = Vector2.zero;

        base.ObjectReturned();
    }


    protected virtual void FixedUpdate()
    {
        //transform.Translate(Vector2.right * speed * Time.fixedDeltaTime); // 오브젝트 방향 기준 앞(오른쪽)으로 평행이동

        if (useTracking) // 트래킹을 할 경우(= 이동 방향이 중간에 바뀔 경우)
        {
            if (target && target.gameObject.activeInHierarchy) // 타겟이 지정되어 있고 활성화되어 있을 경우
            {
                transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg((target.position - transform.position).normalized);
            }
            else // 타겟이 지정되어 있지 않을 경우
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, searchRadius, 1 << (int)LAYER.Censor);
                foreach (var v in cols)
                {
                    if (v.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)targetEntity))
                    {
                        target = hitBase.transform;

                        break;
                    }
                }
            }

            Vector2 goalVelo = MyCalculator.Deg2Vec(transform.eulerAngles.z) * speed;
            Vector2 newVelo = rigid2D.velocity + (goalVelo - rigid2D.velocity) * (Time.fixedDeltaTime / driftTime);

            newVelo.x = Mathf.Clamp(Mathf.Abs(newVelo.x), 0f, Mathf.Abs(goalVelo.x)) * Mathf.Sign(newVelo.x);
            newVelo.y = Mathf.Clamp(Mathf.Abs(newVelo.y), 0f, Mathf.Abs(goalVelo.y)) * Mathf.Sign(newVelo.y);

            rigid2D.velocity = newVelo;
        }
    }    

    private void SpawnSubProj()
    {
        PoolObject projectile;

        if (spawnForward) // 기존 방향에도 생성할 경우
        {
            projectile = parentPool.GetPoolObject(subObject);
            projectile.Init(transform.position, transform.eulerAngles.z);
        }
        if (spawnBackward) // 기존 방향 뒤에도 생성할 경우
        {
            projectile = parentPool.GetPoolObject(subObject);
            projectile.Init(transform.position, transform.eulerAngles.z + 180f);
        }

        for (int i = 1; i < projPair + 1; i++) // 1 ~ projPair까지의 수
        {
            projectile = parentPool.GetPoolObject(subObject);
            projectile.Init(transform.position, transform.eulerAngles.z - angleDiff * i);

            projectile = parentPool.GetPoolObject(subObject);
            projectile.Init(transform.position, transform.eulerAngles.z + angleDiff * i);
        }

        if (SubProjSound)
        {
            AudioManager.Inst.PlaySFX(SubProjSound);
        }
    }
}
