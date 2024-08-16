using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPattern_Trigger : SubPattern
{
    [SerializeField]
    private bool triggerWithScreen = false;
    [SerializeField]
    private EntityType triggerEntity;

    [Space(10)]
    [SerializeField]
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
                        Debug.LogError((triggerCount - 1) + "�� �ش��ϴ� UnityEvent�� ����.");
                    }
                    Debug.Log(triggerCount + "�� ���� Invoke");
                }
                else
                {
                    enabled = false;
                }
            }
        }
    }

    public override void PatternInvoke_Sub(int index)
    {
        Debug.Log("Trigger SubPattern�� PatternInvoke_Sub �޼���� �۵����� ����.");
    }
}
