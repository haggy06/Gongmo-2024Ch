using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class PoolObjectRemover: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PoolObject>(out PoolObject obj)) // 오브젝트가 영역 밖으로 나갔을 경우
        {
            if (obj.isActiveAndEnabled) // 오브젝트가 활성화 되었을 경우
            {
                obj.ReturnToPool(); // 오브젝트를 풀에 넣어줌
            }
        }
    }
}
