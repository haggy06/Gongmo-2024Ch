using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Animator))]
public class MonkfishLightball : PoolObject
{
    [SerializeField, Tooltip("true�� �� ���� �ð� �� �������. �Ʊ� �밡���� �޷� �ִ� ���� false�� �д�.")]
    private bool isProjectile;
    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private float speed;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec(angle) * speed;
    }
    private void OnEnable() // �Ʊ� �밡���� �ִ� ���� Init()�� ���� �����Ƿ� OnEnable()�� ���� �� ��Ȳ �� ����ǵ��� �Ѵ�.
    {
        if (isProjectile)
        {
            Invoke("BlinkStart", lifeTime);
        }
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
