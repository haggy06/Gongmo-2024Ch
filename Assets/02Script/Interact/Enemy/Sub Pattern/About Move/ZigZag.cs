using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZag : MoveBase
{
    [SerializeField]
    private Vector2 sinHeightRange;
    [SerializeField]
    private Vector2 sinFrequencyRange;

    private float actualSinHeight;

    private float beforeValue = 0f;
    protected override void OnEnable()
    {
        base.OnEnable();

        actualSinHeight = Random.Range(sinHeightRange.x, sinHeightRange.y);
        moveSpeed = Random.Range(sinFrequencyRange.x, sinFrequencyRange.y);

        beforeValue = 0f;
    }
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime * moveSpeed;
        float newValue = MyCalculator.SinWave(time, -actualSinHeight, actualSinHeight);

        transform.Rotate(Vector3.forward * (beforeValue - newValue));
        beforeValue = newValue;
    }
}
