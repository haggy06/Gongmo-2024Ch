using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class MonkfishLightball : MonoBehaviour
{
    public static event System.Action PlayerDetected = () => { Debug.Log("아귀에게 발각됨!"); };
    private void OnDestroy()
    {
        PlayerDetected = () => { Debug.Log("아귀에게 발각됨!"); }; // 파괴될 때(풀 들어갈 떄 말고) 감지 이벤트 초기화
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _)) // 플레이어를 감지했을 경우
        {
            PlayerDetected.Invoke();
        }
    }
}
