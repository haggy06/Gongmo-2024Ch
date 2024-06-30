using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Item : PoolObject
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private ItemType itemType;
    public ItemType ItemType => itemType;
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType => weaponType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out PlayerInteract playerInteract)) // 플레이어에 닿았을 경우
        {
            playerInteract.GetItem(this);

            ReturnToPool();
        }
    }

    protected virtual void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec((Random.Range(0, 4) * 90f) + 45f) * speed; // 45, 135, 225, 315도 중 한 쪽으로 튐
    }
}

public enum ItemType
{
    Heal,
    Invincible,
    Bomb,

    Weapon,
}