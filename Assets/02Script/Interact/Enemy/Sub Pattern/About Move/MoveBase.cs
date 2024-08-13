using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBase : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed;
    protected float actualSpeed;

    protected virtual void OnEnable()
    {
        actualSpeed = moveSpeed;
    }

    public void ChangeSpeed(float newSpeed)
    {
        actualSpeed = newSpeed;
    }
}
