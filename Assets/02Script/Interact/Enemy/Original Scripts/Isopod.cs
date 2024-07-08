using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isopod : EnemyBase
{
    [Header("Move Setting")]
    [SerializeField]
    private float moveDepth;
    [SerializeField]
    private float moveFrequency;
    [SerializeField]
    private float speed;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        rigid2D.velocity = Vector2.down * 3f;

        moveDepth = Random.Range(60f, 135f);
        moveFrequency = Random.Range(0.1f, 1f);
        speed = moveFrequency * moveDepth * Mathf.Deg2Rad;

        time = 0f;
        transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg(PlayerController.Player.transform.position - transform.position); // 처음엔 플레이어를 바라봄
        firstAngle = transform.eulerAngles.z; // 첫 각도 저장
    }

    private float time;
    private float firstAngle;
    private void FixedUpdate()
    {
        /*
        float randomAngle = Random.Range(-angleOffset, angleOffset) * Time.fixedDeltaTime;
        transform.Rotate(Vector3.forward * randomAngle);
        */
        time += Time.fixedDeltaTime;
        transform.eulerAngles = Vector3.forward * (firstAngle + Mathf.Sin(time * Mathf.PI * moveFrequency) * moveDepth); // 사인 함수 모양으로 구불거리며 이동

        transform.Translate(Vector2.right * speed * 3f * Time.fixedDeltaTime);
    }

    protected override void HalfHP()
    {

    }

    protected override void MoribundHP()
    {

    }

    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {

    }
}
