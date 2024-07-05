using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puffer : EnemyBase
{
    [SerializeField]
    private float followSpeed = 1f;
    [SerializeField]
    private float cFollowSpeed;

    [Header("Spit Needle")]
    [SerializeField]
    private Transform spitTransform;
    [SerializeField]
    private Projectile needle;

    [Header("Self Explosion")]

    [SerializeField]
    private ExplosionObject explosion;

    [Space(5)]
    [SerializeField]
    private float explosionReach = 5f;
    [SerializeField]
    private int needleNumberWhenExplosion = 8;

    protected override void Awake()
    {
        base.Awake();

        cFollowSpeed = followSpeed;
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        enemyInteract.damageResistance = 0f;
        cFollowSpeed = followSpeed;
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg(PlayerController.Player.transform.position - transform.position);
        transform.Translate(Vector2.right * cFollowSpeed * Time.fixedDeltaTime);
    }
    protected override void HalfHP()
    {
        print("복어 광폭화");

        enemyInteract.damageResistance = 0.25f;
        cFollowSpeed *= 1.5f;
    }

    protected override void MoribundHP()
    {

    }

    /* 복어 공격 패턴
     * 1. 자폭 (근접)
     * 2. 가시 발사 (원거리)
     */
    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        if (PatternCheck.shortDistance(transform.position, explosionReach)) // 근접 공격이 가능할 경우
        {
            cFollowSpeed *= 0.8f;
            anim.SetInteger(EntityAnimHash.Pattern, 1); // 자폭
        }
        else // 근접 공격이 불가능할 경우
        {
            anim.SetInteger(EntityAnimHash.Pattern, 2); // 가시 발사
        }
    }

    public void SpitNeedle() // 가시 발사
    {
        PoolObject proj = parentPool.GetPoolObject(needle);
        proj.Init(spitTransform.position, transform.eulerAngles.z);
    }
    public void Explosion() // 자폭
    {
        parentPool.GetPoolObject(explosion).Init(transform.position, 0f); // 폭발 공격

        float initialAngle = Random.Range(0f, 360f);
        for (int i = 0; i < needleNumberWhenExplosion; i++) // 가시 공격
        {
            PoolObject proj = parentPool.GetPoolObject(needle);
            proj.Init(transform.position, initialAngle + (360f / needleNumberWhenExplosion * i));
        }

        //enemyInteract.InstantKill(EntityType.Nothing); // 오브젝트 사망 처리
        ReturnToPool();
    }
}
