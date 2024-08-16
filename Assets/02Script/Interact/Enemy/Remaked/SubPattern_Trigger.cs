using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPattern_Trigger : SubPattern
{
    [SerializeField]
    private AudioClip triggerSound;

    [Space(5)]
    [SerializeField]
    private bool triggerWithScreen = false;
    [SerializeField]
    private EntityType triggerEntity;
    [SerializeField, Tooltip("호출에 필요한 접촉 수")]
    private int[] triggerCountForInvoke;

    private int triggerCount = 0;
    private void OnEnable()
    {
        triggerCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((triggerWithScreen && collision.gameObject.layer == (int)LAYER.Ground) ||
            (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)triggerEntity)))
        {
            if (triggerSound)
                AudioManager.Inst.PlaySFX(triggerSound);

            triggerCount++;
            for (int i = 0; i < triggerCountForInvoke.Length; i++)
            {
                if (triggerCount == triggerCountForInvoke[i])
                {
                    PatternInvoke_Sub(i);
                    break;
                }
            }
        }
    }
}
