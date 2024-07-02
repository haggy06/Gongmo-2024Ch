using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PoolObject : MonoBehaviour
{
    [SerializeField, Tooltip("이 오브젝트의 ID.")]
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
            Debug.Log(name + "에 연결된 풀 없음");
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
