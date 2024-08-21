using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MoveBase
{
    public bool tracking = true;
    [SerializeField, Tooltip("시작했을 때 가속도 퍼센트"),Range(0f, 1f)]
    private float impulseOnAwake = 0f;

    [Space(5)]
    [SerializeField, Tooltip("플레이어 방향을 바라볼지 여부.")]
    private bool lookPlayer = true;

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
        if (!Mathf.Approximately(impulseOnAwake, 0f)) 
            Invoke("ImpulseOnAwake", Time.deltaTime);
    }
    private void ImpulseOnAwake()
    {
        rigid2D.velocity = (PlayerController.Inst.transform.position - transform.position).normalized * moveSpeed * impulseOnAwake;
    }

    private void FixedUpdate()
    {
        float lookPlayerAngle = MyCalculator.Vec2Deg((PlayerController.Inst.transform.position - transform.position).normalized);

        if (tracking)
        {
            Vector2 goalVelo = MyCalculator.Deg2Vec(lookPlayerAngle) * moveSpeed;
            Vector2 newVelo = rigid2D.velocity + (goalVelo - rigid2D.velocity) * (Time.fixedDeltaTime / driftTime);

            rigid2D.velocity = newVelo;
        }

        if (lookPlayer)
            transform.eulerAngles = Vector3.forward * lookPlayerAngle;
    }
}
