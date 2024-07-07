using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaAnemone : EnemyBase
{
    [SerializeField]
    private float scrollSpeed = 4f;

    [Header("Tentacle Scratch")]
    [SerializeField]
    private float tentacleReach = 3f;
    [SerializeField]
    private AttackBase tenacleAttack;
    [SerializeField]
    private ParticleSystem scratchEffect;

    [Header("Spread Projectile")]
    [SerializeField]
    private Transform spreadPosition;
    [SerializeField]
    private PoolObject anemoneProjectile;
    [SerializeField]
    private int projectileNumber = 8;

    protected override void Awake()
    {
        base.Awake();
        GameManager.BossEvent += (isOn) => rigid2D.velocity = isOn ? Vector2.zero : Vector2.down * scrollSpeed; // ���� ���� �ÿ� ��ũ���� ���߹Ƿ� �����ߵ� ���߰� ��
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        tenacleAttack.canAttack = false;
        scratchEffect.Clear();
        rigid2D.velocity = Vector2.down * scrollSpeed;
    }
    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }

    /* ������ ���� ����
     * 1. �˼� ������ (����)
     * 2. ��ź �߻� (���Ÿ�)
     */
    protected override void Pattern(int caseNumber, bool isListPattern = false) // �������� ����Ʈ�� �� ���� �н�
    {
        if (PatternCheck.shortDistance(spreadPosition.position, tentacleReach)) // ���� ������ ������ ���
        {
            anim.SetInteger(EntityAnimHash.Pattern, 1); // �˼� ������
        }
        else // ���� ������ �Ұ����� ���
        {
            anim.SetInteger(EntityAnimHash.Pattern, 2); // ��ź �߻�
        }
        /*
        switch (caseNumber)
        {
            case 0: // �˼� ������
                if (PatternCheck.shortDistance(transform.position, tentacleReach)) // ���� ������ ������ ���
                {
                    anim.SetInteger(EntityAnimHash.Pattern, 1);
                }
                else // ���� ������ �Ұ����� ���
                {
                    anim.SetInteger(EntityAnimHash.Pattern, 2);
                    //Pattern(1); // ��ź �߻� ����
                }
                break;

            case 1: // ��ź �߻�
                anim.SetInteger(EntityAnimHash.Pattern, 2);
                break;
        }
        */
    }

     // �ǽ����� ������ ����� Animator���� �����Ѵ�. (Ÿ�̹� ���߱� ����)
    public void TentacleAttack() // �˼� ������
    {
        scratchEffect.Play();
    }
    public void SpreadProjectile() // ��ź �߻�
    {
        float angleDiff = 360f / projectileNumber;
        float initialAngle = Random.Range(0f, 360f);
        for (int i = 0; i < projectileNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(anemoneProjectile);
            proj.Init(spreadPosition.position, initialAngle + (angleDiff * i));
        }
    }
}
