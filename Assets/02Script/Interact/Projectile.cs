using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBase))]
public class Projectile : PoolObject
{
    [Header("Bullet Setting")]
    [SerializeField]
    private float lifeTime = 5f;
    [SerializeField]
    protected float speed = 10f;

    private AttackBase attack;
    public AttackBase Attack => attack;

    protected virtual void Awake()
    {
        attack = GetComponent<AttackBase>();
        attack.AttackSuccessEvent += (_) => ReturnToPool();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime); // 오브젝트 방향 기준 앞(오른쪽)으로 평행이동
    }

    private void OnEnable()
    {
        StartCoroutine("AutoReturn");
    }

    private IEnumerator AutoReturn()
    {
        yield return YieldReturn.WaitForSeconds(lifeTime);

        if (gameObject.activeSelf)
        {
            ReturnToPool();
        }
    }
}
