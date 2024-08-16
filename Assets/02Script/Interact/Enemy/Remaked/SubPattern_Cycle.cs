using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPattern_Cycle : SubPattern
{
    [SerializeField]
    private float cycleTerm = 1f;

    [SerializeField]
    private bool isInfinityCycle = false;
    [SerializeField]
    private int cycleNumber = 1;
    private int cCycleNumber = 0;

    public override void PatternInvoke_Sub(int index)
    {
        StartCoroutine("SubPatternCor");
    }
    public void Stop_SubPattern(int index)
    {
        StopCoroutine("SubPatternCor");
    }

    private void OnEnable()
    {
        cCycleNumber = 0;
    }

    private IEnumerator SubPatternCor()
    {
        int index = 0;
        while (isInfinityCycle || cCycleNumber <= cycleNumber)
        {
            if (++index >= subPatterns.Length)
                index = 0;

            if (isInfinityCycle)
                cCycleNumber++;
            
            try
            {
                subPatterns[index].Invoke();
            }
            catch (System.Exception)
            {
                Debug.LogError("UnityEvent°¡ ¾øÀ½.");
            }

            yield return YieldReturn.WaitForSeconds(cycleTerm);
        }
    }
}
