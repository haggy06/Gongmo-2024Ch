using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRepeatMove : MoveBase
{
    [SerializeField, Tooltip("얼마나 왔다갔다 할 건지 지정")]
    private float howMuchMove;

    private float lastValue = 0;
    private float time = 0f;
    private void OnEnable()
    {
        lastValue = 0f;
        time = 0f;
    }
    private void FixedUpdate()
    {
            time += Time.fixedDeltaTime;

            float newValue = MyCalculator.SinWave(time * (moveSpeed / howMuchMove / 2f), -howMuchMove, howMuchMove);

            transform.position += Vector3.right * (newValue - lastValue);
            lastValue = newValue;
    }
}
