using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleEditor : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]
    private float timeScale = 1f;
    void Update()
    {
        Time.timeScale = timeScale;
    }
}
