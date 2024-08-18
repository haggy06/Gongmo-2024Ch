using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class MonkfishLightball : MonoBehaviour
{
    public static event System.Action PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); };
    private void OnDestroy()
    {
        PlayerDetected = () => { Debug.Log("�ƱͿ��� �߰���!"); }; // �ı��� ��(Ǯ �� �� ����) ���� �̺�Ʈ �ʱ�ȭ
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _)) // �÷��̾ �������� ���
        {
            PlayerDetected.Invoke();
        }
    }
}
