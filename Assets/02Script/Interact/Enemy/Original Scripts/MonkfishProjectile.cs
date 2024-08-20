using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(Animator))]
public class MonkfishProjectile : PoolObject
{
    [SerializeField]
    private float lifeTime = 3f;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        Invoke("BlinkStart", lifeTime);
    }
    private void BlinkStart()
    {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<Animator>().SetTrigger(EntityAnimHash.Pattern);
        }
    }

    //public static event System.Action PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); };
    //private static List<SubPattern> subscribes = new();
    private static Dictionary<SubPattern, bool> subDictionary = new();
    public static void Subscribe2DetectedEvent(SubPattern subPattern)
    {
        subDictionary.TryAdd(subPattern, true);
    }
    public static void CancleSub2DetectedEvent(SubPattern subPattern)
    {
        subDictionary.TryAdd(subPattern, false);
    }

    private void OnDestroy()
    {
        //PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); }; // �ı��� ��(Ǯ �� �� ����) ���� �̺�Ʈ �ʱ�ȭ
        subDictionary.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _)) // �÷��̾ �������� ���
        {
            //PlayerDetected.Invoke();
            foreach (var subscribe in subDictionary)
            {
                if (subscribe.Value)
                    subscribe.Key.PatternInvoke_Sub(0);
            }
        }
    }
}
