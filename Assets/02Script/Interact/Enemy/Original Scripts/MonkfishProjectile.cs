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

    public static event System.Action PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); };
    //private static List<SubPattern> subscribes = new();
    //private static Dictionary<GameObject, SubPattern> subDictionary = new();
    public static void Subscribe2DetectedEvent(SubPattern subPattern)
    {
        //subDictionary.TryAdd(subPattern.gameObject, subPattern);
        PlayerDetected += () =>
        {
            if (subPattern.gameObject.activeInHierarchy && subPattern.GetComponent<EnemyBase>().curPattern != 2)
            {
                if (subPattern.GetComponent<EnemyBase>().curPattern == 2)
                    print("Error!!!");
                subPattern.PatternInvoke_Sub(0);
            }
        };
    }
    /*
    public static void CancleSub2DetectedEvent(SubPattern subPattern)
    {
        //subDictionary.Remove(subPattern.gameObject);
    }
    */

    private void OnDestroy()
    {
        PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); }; // �ı��� ��(Ǯ �� �� ����) ���� �̺�Ʈ �ʱ�ȭ
        //subDictionary.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _)) // �÷��̾ �������� ���
        {
            PlayerDetected.Invoke();
            /*
            foreach (var subscribe in subDictionary)
            {
                    subscribe.Value.PatternInvoke_Sub(0);
            }
            */
        }
    }
}
