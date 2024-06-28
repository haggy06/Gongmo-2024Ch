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
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime); // ������Ʈ ���� ���� ��(������)���� �����̵�
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
