using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRepeatMove : MoveBase
{
    [SerializeField, Tooltip("�󸶳� �Դٰ��� �� ���� ����")]
    private float howMuchMove;
    [SerializeField]
    private float speed;

    private float lastValue = 0;
    private float time = 0f;
    protected override void OnEnable()
    {
        time = 0f;
    }
    private void FixedUpdate()
    {
            time += Time.fixedDeltaTime;

            float newValue = MyCalculator.SinWave(time * (actualSpeed / howMuchMove / 2f), -howMuchMove, howMuchMove);

            transform.position += Vector3.right * (newValue - lastValue);
            lastValue = newValue;
    }
}
