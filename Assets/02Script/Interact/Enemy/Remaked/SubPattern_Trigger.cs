using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubPattern_Trigger : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent[] subPatterns;

    [Space(5)]
    [SerializeField]
    private bool triggerWithScreen = false;
    [SerializeField]
    private EntityType triggerEntity;

    private int triggerCount = 0;
    private void OnEnable()
    {
        triggerCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
        {
            if ((triggerWithScreen && collision.gameObject.layer == (int)LAYER.Ground) ||
            (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)triggerEntity)))
            {

                triggerCount++;
                if (subPatterns.Length >= triggerCount)
                {
                    try
                    {
                        subPatterns[triggerCount - 1].Invoke();
                    }
                    catch (System.Exception)
                    {
                        Debug.LogError((triggerCount - 1) + "에 해당하는 UnityEvent가 없음.");
                    }
                    Debug.Log(triggerCount + "번 패턴 Invoke");
                }
                else
                {
                    enabled = false;
                }
            }
        }
    }
}
