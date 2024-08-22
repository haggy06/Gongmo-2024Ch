using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMove : MoveBase
{
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.fixedDeltaTime);
    }
}
