using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(Animator))]
public class MonkfishProjectile : PoolObject
{
    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private float speed;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec(angle) * speed;

        Invoke("BlinkStart", lifeTime);
    }
    private void BlinkStart()
    {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<Animator>().SetTrigger(EntityAnimHash.Pattern);
        }
    }

    public static event System.Action PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); };
    private void OnDestroy()
    {
        PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); }; // �ı��� ��(Ǯ �� �� ����) ���� �̺�Ʈ �ʱ�ȭ
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _)) // �÷��̾ �������� ���
        {
            PlayerDetected.Invoke();
        }
    }
}
