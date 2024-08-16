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
            Debug.LogError(index + "에 해당하는 UnityEvent가 없음.");
        }
    }

    /*
    private EnemyBaseData eData;
    private EnemyBaseData EDATA => eData;
    private void Test()
    {
        eData.usePattern = false;

        EDATA.usePattern = false; // 구조체 property의 property 값 수정은 첫 property의 반환값 수정과 같기 떄문에 의미없는 코드가 되어 에러가 난다.
    }
    */
}
