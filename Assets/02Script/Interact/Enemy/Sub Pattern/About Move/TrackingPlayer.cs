using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MoveBase
{
    public bool tracking = true;
    [SerializeField, Tooltip("시작했을 때 가속을 얻은 상태로 시작할지 지정")]
    private bool impulseOnAwake = false;

    [Space(5)]
    [SerializeField, Tooltip("플레이어 방향을 바라볼지 여부.")]
    private bool lookPlayer = true;

    [Space(5)]
    [SerializeField, Tooltip("빙빙 돌지 여부.")]
    private bool spin = false;
    [SerializeField]
    private float spinSpeed = 0f;

    [Space(5)]
    [Range(0.1f, 10f), Tooltip("방향을 꺾는 데 소요되는 시간")]
    public float driftTime = 1f;

    private Rigidbody2D rigid2D;
    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (impulseOnAwake)
        {
            rigid2D.velocity = (PlayerController.Inst.transform.position - transform.position).normalized * moveSpeed;
        }
    }
    private void FixedUpdate()
    {
        if (tracking)
        {
            float lookPlayerAngle = MyCalculator.Vec2Deg((PlayerController.Inst.transform.position - transform.position).normalized);

            if (lookPlayer)
                transform.eulerAngles = Vector3.forward * lookPlayerAngle;
            else if (spin)
                transform.Rotate(Vector3.forward * (spinSpeed * Time.fixedDeltaTime));

            Vector2 goalVelo = MyCalculator.Deg2Vec(lookPlayerAngle) * moveSpeed;
            Vector2 newVelo = rigid2D.velocity + (goalVelo - rigid2D.velocity) * (Time.fixedDeltaTime / driftTime);

            rigid2D.velocity = newVelo;
        }        
    }
}
