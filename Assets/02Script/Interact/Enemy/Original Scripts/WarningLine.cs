using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WarningLine : PoolObject
{
    [Header("Warning Blink")]
    [SerializeField]
    private float blinkSpeed = 1f;
    [Space(5)]
    [SerializeField]
    private Color color1;
    [SerializeField]
    private Color color2;

    [Header("Warning And Shoot")]
    public float warningTime = 1f;
    [SerializeField]
    private EnemyBase loadedEnemy;

    private SpriteRenderer sprite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg(PlayerController.Player.transform.position - transform.position);

        time = 0f;
        Invoke("WarningEnd", warningTime);
    }
    private void WarningEnd()
    {
        if (gameObject.activeInHierarchy) // 아직 오브젝트가 활성화되어 있을 경우
        {
            parentPool.GetPoolObject(loadedEnemy).Init(transform.position, transform.eulerAngles.z);
            ReturnToPool();
        }
    }

    private float time = 0f;
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        sprite.color = MyCalculator.CosWave(time * blinkSpeed, color1, color2);
    }
}
