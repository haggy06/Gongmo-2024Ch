using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBase : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed;
    public float MoveSpeed  { get => actualSpeed; set => actualSpeed = value; }

    protected float actualSpeed;

    protected virtual void OnEnable()
    {
        actualSpeed = moveSpeed;
    }
}
