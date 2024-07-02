using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

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

        try
        {
            parentPool.SetPoolObject(this);
        }
        catch (NullReferenceException) 
        {
            Debug.Log(name + "�� ����� Ǯ ����");
            Destroy(gameObject);
        }
    }
    public virtual void Init(Vector2 position, float angle)
    {
        transform.position = position;
        transform.eulerAngles = Vector3.forward * angle;
    }
    public virtual void ExitFromPool()
    {
        gameObject.SetActive(true);
    }
}
