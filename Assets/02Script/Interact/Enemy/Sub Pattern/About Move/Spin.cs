using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed = 360f;
        
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * (spinSpeed * Time.fixedDeltaTime));
    }
}
