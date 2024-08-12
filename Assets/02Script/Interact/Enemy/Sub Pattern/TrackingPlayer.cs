using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MonoBehaviour
{
    public bool tracking = true;
    [SerializeField, Tooltip("�������� �� ������ ���� ���·� �������� ����")]
    private bool impulseOnAwake = false;
    [SerializeField, Tooltip("�÷��̾� ������ �ٶ��� ����")]
    private bool useRotation = true;

    [Space(5)]
    public float speed;
    [Range(0.1f, 10f), Tooltip("������ ���� �� �ҿ�Ǵ� �ð�")]
    public float driftTime = 1f;

    private Rigidbody2D rigid2D;
    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private bool firstUpdate = false;
    private void OnDisable()
    {
        firstUpdate = false;
    }
    private void FixedUpdate()
    {
        if (impulseOnAwake && firstUpdate)
        {
            firstUpdate = true;
            rigid2D.velocity = (PlayerController.Inst.transform.position - transform.position).normalized * speed;
        }


        if (tracking)
        {
            float lookPlayerAngle = MyCalculator.Vec2Deg((PlayerController.Inst.transform.position - transform.position).normalized);

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
}
