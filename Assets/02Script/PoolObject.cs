using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PoolObject : MonoBehaviour
{
    [SerializeField, Tooltip("이 오브젝트의 ID.")]
    private int poolObjectID;
    public int PoolObjectID => poolObjectID;
    [SerializeField]
    private AudioClip awakeSound;

    [SerializeField]
    private bool destroyWhenBomb = true;
    [HideInInspector]
    public ObjectPool parentPool;

    protected virtual void ObjectReturned()
    {
        if (destroyWhenBomb)
            PlayerInteract.BombEvent -= DestroyByBomb;

        gameObject.SetActive(false);
    }
    protected virtual void DestroyByBomb()
    {
        ReturnToPool();
    }

    public virtual void ReturnToPool()
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

        if (awakeSound)
            AudioManager.Inst.PlaySFX(awakeSound);

        if (destroyWhenBomb)
            PlayerInteract.BombEvent += DestroyByBomb;
    }
    public virtual void ExitFromPool()
    {
        gameObject.SetActive(true);
    }
}
