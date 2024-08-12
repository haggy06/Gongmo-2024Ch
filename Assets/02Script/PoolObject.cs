using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PoolObject : MonoBehaviour
{
    [SerializeField, Tooltip("�� ������Ʈ�� ID.")]
    private int poolObjectID;
    public int PoolObjectID => poolObjectID;

    [SerializeField]
    private InitDirection initDirection;
    [SerializeField]
    private AudioClip awakeSound;
    public AudioClip AwakeSound => awakeSound;

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
            Debug.Log(name + "�� ����� Ǯ ����");
            Destroy(gameObject);
        }
    }
    public virtual void Init(Vector2 position, float angle)
    {
        transform.position = position;

        switch (initDirection) 
        {
            case InitDirection.None:
                transform.eulerAngles = Vector3.forward * angle;
                break;

            case InitDirection.Down:
                transform.eulerAngles = Vector3.forward * -90f;
                break;

            case InitDirection.Up:
                transform.eulerAngles = Vector3.forward * 90f;
                break;

            case InitDirection.Player:
                transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - transform.position);
                break;
        }

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
