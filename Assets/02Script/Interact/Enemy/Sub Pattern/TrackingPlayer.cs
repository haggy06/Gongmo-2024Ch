using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MonoBehaviour
{
    [SerializeField, Tooltip("플레이어 방향을 바라볼지 여부")]
    private bool useRotation = true;

    [Space(5)]
    public float speed;
    [SerializeField, Range(0.1f, 10f), Tooltip("방향을 꺾는 데 소요되는 시간")]
    private float driftTime = 1f;

    private Rigidbody2D rigid2D;
    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float lookPlayerAngle = MyCalculator.Vec2Deg((PlayerController.Player.transform.position - transform.position).normalized);

        if (useRotation)
            transform.eulerAngles = Vector3.forward * lookPlayerAngle;

        Vector2 goalVelo = MyCalculator.Deg2Vec(lookPlayerAngle) * speed;
        Vector2 newVelo = rigid2D.velocity + (goalVelo - rigid2D.velocity) * (Time.fixedDeltaTime / driftTime);

        /*
        newVelo.x = Mathf.Clamp(Mathf.Abs(newVelo.x), -Mathf.Abs(goalVelo.x), Mathf.Abs(goalVelo.x)) * Mathf.Sign(newVelo.x);
        newVelo.y = Mathf.Clamp(Mathf.Abs(newVelo.y), -Mathf.Abs(goalVelo.x), Mathf.Abs(goalVelo.y)) * Mathf.Sign(newVelo.y);
        */

        rigid2D.velocity = newVelo;
    }
}
