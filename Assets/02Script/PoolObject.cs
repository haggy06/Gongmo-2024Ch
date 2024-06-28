using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField, Tooltip("�� ������Ʈ�� ID.")]
    private int poolObjectID;
    public int PoolObjectID => poolObjectID;

    [HideInInspector]
    public ObjectPool parentPool;

    protected virtual void ObjectReturned()
    {
        gameObject.SetActive(false);
    }
    public void ReturnToPool()
    {
        ObjectReturned();

        parentPool.SetPoolObject(this);
    }

    public virtual void ExitFromPool()
    {
        gameObject.SetActive(true);
    }
}
