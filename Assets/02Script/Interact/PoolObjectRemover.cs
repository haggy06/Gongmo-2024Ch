using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PoolObjectRemover: MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PoolObject>(out PoolObject obj)) // ������Ʈ�� ���� ������ ������ ���
        {
            if (obj.gameObject.activeInHierarchy) // ������Ʈ�� Ȱ��ȭ �Ǿ� �־��� ���
            {
                obj.ReturnToPool(); // ������Ʈ�� Ǯ�� �־���
            }
        }
    }
}
