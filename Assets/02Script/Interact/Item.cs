using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Item : PoolObject
{
    [SerializeField]
    private float speed = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHit>(out PlayerHit playerHit)) // 플레이어에 닿았을 경우
        {
            // todo : PlayerHit에 아이템 획득 구현

            ReturnToPool();
        }
    }

    protected virtual void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec((Random.Range(0, 4) * 90f) + 45f) * speed; // 45, 135, 225, 315도 중 한 쪽으로 튐
    }

}
