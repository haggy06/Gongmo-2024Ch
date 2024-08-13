using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMove : MoveBase
{
    [Space(10)]
    [SerializeField]
    private bool zigZag = false;
    [SerializeField]
    private Vector2 sinHeightRange;
    [SerializeField]
    private Vector2 sinFrequencyRange;

    private float actualSinHeight;
    private float actualSinFrequency;

    private float time = 0f;
    private float beforeValue = 0f;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (zigZag)
        {
            actualSinHeight = Random.Range(sinHeightRange.x, sinHeightRange.y);
            actualSinFrequency = Random.Range(sinFrequencyRange.x, sinFrequencyRange.y);
        }

        time = 0f;
        beforeValue = 0f;
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.fixedDeltaTime);

        if (zigZag)
        {
            time += Time.fixedDeltaTime * actualSinFrequency;
            float newValue = MyCalculator.SinWave(time, -actualSinHeight, actualSinHeight);

            transform.Rotate(Vector3.forward * (beforeValue - newValue));
            beforeValue = newValue;
        }
    }
}
