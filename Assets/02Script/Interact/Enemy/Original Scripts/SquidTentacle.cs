using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidTentacle : StraightMoveEnemy
{
    [SerializeField]
    private float tentacleBreakTerm = 1f;
    [SerializeField]
    private float tentacleDisappearTerm = 10f;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        if (lookPlayer)
            transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg(PlayerController.Player.transform.position - transform.position);

        rigid2D.velocity = MyCalculator.Deg2Vec(transform.eulerAngles.z) * speed;

        Invoke("TentacleBreak", tentacleBreakTerm);
    }

    private void TentacleBreak()
    {
        if (gameObject.activeInHierarchy) // ���� ������� ���
        {
            rigid2D.velocity = Vector2.zero; // ����

            Invoke("TentacleDisappear", tentacleDisappearTerm);
        }
    }
    private void TentacleDisappear()
    {
        if (gameObject.activeInHierarchy) // ���� ������� ���
            rigid2D.velocity = -MyCalculator.Deg2Vec(transform.eulerAngles.z) * speed; // ó�� ���� �ݴ� �������� ����
    }
}
