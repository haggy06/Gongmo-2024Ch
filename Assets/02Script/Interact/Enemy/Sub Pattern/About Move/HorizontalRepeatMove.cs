using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRepeatMove : MoveBase
{
    [SerializeField, Tooltip("�󸶳� �Դٰ��� �� ���� ����")]
    private float howMuchMove;

    private float lastValue = 0;
    protected override void OnEnable()
    {
        base.OnEnable();
        lastValue = 0f;
    }
    private void FixedUpdate()
    {
            time += Time.fixedDeltaTime;

            float newValue = MyCalculator.SinWave(time * (moveSpeed / howMuchMove / 2f), -howMuchMove, howMuchMove);

            transform.position += Vector3.right * (newValue - lastValue);
            lastValue = newValue;
    }
}
