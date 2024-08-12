using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField, Tooltip("이 오브젝트의 ID.")]
    private int poolObjectID;
    public int PoolObjectID => poolObjectID;

    [SerializeField]
    private bool destroyWhenBomb = true;
    [HideInInspector]
    public ObjectPool parentPool;

    [Space(5)]
    [SerializeField]
    private InitDirection initDirection;
    [SerializeField]
    private AudioClip awakeSound;
    public AudioClip AwakeSound => awakeSound;


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
        catch (System.NullReferenceException) 
        {
            Debug.Log(name + "에 연결된 풀 없음");
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
