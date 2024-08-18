using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubPattern_Cycle : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent[] subPatterns;

    [Space(5)]
    [SerializeField]
    private float cycleTerm = 1f;

    [SerializeField]
    private bool isInfinityCycle = false;
    [SerializeField]
    private int cycleNumber = 1;
    private int cCycleNumber = 0;

    public void PatternInvoke_Start()
    {
        StartCoroutine("SubPatternCor");
    }
    public void PatternInvoke_Stop()
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
