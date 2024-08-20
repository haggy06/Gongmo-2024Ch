using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackingPlayer))]
public class Monkfish : EnemyBase
{
    /*
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
    */

    private TrackingPlayer tracking;
    protected override void Awake()
    {
        base.Awake();
        tracking = GetComponent<TrackingPlayer>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        //tracking.tracking = false;
        tracking.MoveSpeed = Random.Range(5f, 7f);
        tracking.driftTime = Random.Range(0.25f, 0.75f);

        //enemyInteract.DamageResistance = 0f;

        //MonkfishProjectile.PlayerDetected += PlayerFound;
    }
    protected override void ObjectReturned()
    {
        base.ObjectReturned();

        //MonkfishProjectile.PlayerDetected -= PlayerFound;
    }
    private void PlayerFound()
    {
        StopCoroutine("PatternCor");

        PatternInvoke_Immediate(2);
        Pattern(2);

        /*
        enemyInteract.DamageResistance = 0.25f;
        tracking.tracking = true;
        */

        //MonkfishProjectile.PlayerDetected -= PlayerFound; // 반환 될 때까지 더 이상 필요가 없으므로 중복 실행 방지를 위해 구독을 날려준다.
    }
    /*
    public void SpreadLight()
    {
        AudioManager.Inst.PlaySFX(lightballSound);
        for (int i = 0; i < lightballNumber; i++)
        {
            parentPool.GetPoolObject(lightball).Init(spreadPosition.position, Random.Range(-210f, 30f));
        }

        StabilizePattern();
    }
    */
}
