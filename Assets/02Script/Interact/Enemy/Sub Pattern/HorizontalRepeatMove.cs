using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRepeatMove : MonoBehaviour
{
    public bool moving;
    [SerializeField, Tooltip("얼마나 왔다갔다 할 건지 지정")]
    private float howMuchMove;
    [SerializeField]
    private float speed;

    private float lastValue = 0;
    private float time = 0f;
    private void OnEnable()
    {
        time = 0f;
    }
    private void FixedUpdate()
    {
        if (moving)
        {
            time += Time.fixedDeltaTime;

            float newValue = MyCalculator.SinWave(time * (speed / howMuchMove / 2f), -howMuchMove, howMuchMove);

            transform.position += Vector3.right * (newValue - lastValue);
            lastValue = newValue;
        }
        else
        {
            lastValue = time = 0f;
        }
    }
}
