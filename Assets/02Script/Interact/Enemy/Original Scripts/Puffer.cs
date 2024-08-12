using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackingPlayer))]
public class Puffer : EnemyBase
{
    [SerializeField]
    private float followSpeed;

    [Header("Spit Needle")]
    [SerializeField]
    private AudioClip spitSound;
    [SerializeField]
    private Transform spitTransform;
    [SerializeField]
    private Projectile needle;

    [Header("Self Explosion")]
    [SerializeField]
    private AudioClip inflictSound;
    [SerializeField]
    private ExplosionObject explosion;

    [Space(5)]
    [SerializeField]
    private float explosionReach = 5f;
    [SerializeField]
    private int needleNumberWhenExplosion = 8;

    private TrackingPlayer tracking;
    protected override void Awake()
    {
        base.Awake();

        tracking = GetComponent<TrackingPlayer>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        enemyInteract.damageResistance = 0f;
        tracking.speed = followSpeed;
    }
    protected override void HalfHP()
    {
        tracking.speed *= 1.25f;
    }

    /* ���� ���� ����
     * 1. ���� (����)
     * 2. ���� �߻� (���Ÿ�)
     */
    protected override void Pattern(int caseNumber)
    {
        if (PatternCheck.ShortDistance(transform.position, explosionReach)) // ���� ������ ������ ���
        {
            tracking.speed *= 0.75f;
            AudioManager.Inst.PlaySFX(inflictSound);

            anim.SetInteger(EntityAnimHash.Pattern, 1); // ����
        }
        else // ���� ������ �Ұ����� ���
        {
            anim.SetInteger(EntityAnimHash.Pattern, 2); // ���� �߻�
        }
    }

    public void SpitNeedle() // ���� �߻�
    {
        AudioManager.Inst.PlaySFX(spitSound);

        PoolObject proj = parentPool.GetPoolObject(needle);
        proj.Init(spitTransform.position, transform.eulerAngles.z);

        StabilizePattern();
    }
    public void Explosion() // ����
    {
        parentPool.GetPoolObject(explosion).Init(transform.position, 0f); // ���� ����

        float initialAngle = Random.Range(0f, 360f);
        for (int i = 0; i < needleNumberWhenExplosion; i++) // ���� ����
        {
            PoolObject proj = parentPool.GetPoolObject(needle);
            proj.Init(transform.position, initialAngle + (360f / needleNumberWhenExplosion * i));
        }

        StabilizePattern();

        //enemyInteract.InstantKill(EntityType.Nothing); // ������Ʈ ��� ó��
        ReturnToPool();
    }
}
