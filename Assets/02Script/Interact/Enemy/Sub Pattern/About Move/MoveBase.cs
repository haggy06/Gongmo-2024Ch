using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBase : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed;
    public float MoveSpeed  { get => moveSpeed; set => moveSpeed = value; }
}
