using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[RequireComponent(typeof(EnemyBase))]
public class SubPattern : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent[] subPatterns;

    public virtual void PatternInvoke_Sub(int index)
    {
        try
        {
            subPatterns[index].Invoke();
        }
        catch (System.Exception)
        {
            Debug.LogError(index + "�� �ش��ϴ� UnityEvent�� ����.");
        }
    }
}

public interface I_SubPattern
{
    public void PatternInvoke_Sub(int index);
}