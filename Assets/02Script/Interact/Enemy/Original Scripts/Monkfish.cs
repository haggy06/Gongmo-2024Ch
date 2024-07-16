using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackingPlayer))]
public class Monkfish : EnemyBase
{
    [Header("Move")]
    [SerializeField]
    private float speed;

    [Header("Spread Lightball")]
    [SerializeField]
    private AudioClip lightballSound;
    [SerializeField]
    private PoolObject lightball;
    [SerializeField]
    private Transform spreadPosition;
    [SerializeField]
    private int lightballNumber;

    [Header("Detection")]
    [SerializeField]
    private AudioClip detectionSound;
    private TrackingPlayer tracking;
    protected override void Awake()
    {
        base.Awake();
        tracking = GetComponent<TrackingPlayer>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        tracking.tracking = false;
        tracking.speed = Random.Range(5f, 7f);
        tracking.driftTime = Random.Range(0.25f, 0.75f);

        enemyInteract.damageResistance = 0f;

        transform.eulerAngles = Vector3.forward * -90f;
        rigid2D.velocity = Vector2.down * speed;

        MonkfishLightball.PlayerDetected += PlayerFound;
    }
    protected override void ObjectReturned()
    {
        base.ObjectReturned();

        MonkfishLightball.PlayerDetected -= PlayerFound;
    }
    private void PlayerFound()
    {
        AudioManager.Inst.PlaySFX(detectionSound);
        StopCoroutine("PatternRepeat");

        enemyInteract.damageResistance = 0.25f;
        anim.SetInteger(EntityAnimHash.Pattern, 2);
        tracking.tracking = true;

        MonkfishLightball.PlayerDetected -= PlayerFound; // ��ȯ �� ������ �� �̻� �ʿ䰡 �����Ƿ� �ߺ� ���� ������ ���� ������ �����ش�.
    }

    protected override void HalfHP()
    {

    }

    protected override void MoribundHP()
    {

    }

    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        anim.SetInteger(EntityAnimHash.Pattern, 1); // �� ������ ������
    }

    public void SpreadLight()
    {
        AudioManager.Inst.PlaySFX(lightballSound);
        for (int i = 0; i < lightballNumber; i++)
        {
            parentPool.GetPoolObject(lightball).Init(spreadPosition.position, Random.Range(-210f, 30f));
        }
    }
}
