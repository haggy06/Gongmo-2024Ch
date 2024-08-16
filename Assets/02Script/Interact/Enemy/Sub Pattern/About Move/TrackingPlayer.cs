using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MoveBase
{
    public bool tracking = true;
    [SerializeField, Tooltip("�������� �� ������ ���� ���·� �������� ����")]
    private bool impulseOnAwake = false;

    [Space(5)]
    [SerializeField, Tooltip("�÷��̾� ������ �ٶ��� ����.")]
    private bool lookPlayer = true;

    [Space(5)]
    [SerializeField, Tooltip("���� ���� ����.")]
    private bool spin = false;
    [SerializeField]
    private float spinSpeed = 0f;

    [Space(5)]
    [Range(0.1f, 10f), Tooltip("������ ���� �� �ҿ�Ǵ� �ð�")]
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
