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
            Debug.Log(index);
            subPatterns[index].Invoke();
        }
        catch (System.Exception)
        {
            Debug.LogError(index + "�� �ش��ϴ� UnityEvent�� ����.");
        }
    }

    /*
    private EnemyBaseData eData;
    private EnemyBaseData EDATA => eData;
    private void Test()
    {
        eData.usePattern = false;

        EDATA.usePattern = false; // ����ü property�� property �� ������ ù property�� ��ȯ�� ������ ���� ������ �ǹ̾��� �ڵ尡 �Ǿ� ������ ����.
    }
    */
}
