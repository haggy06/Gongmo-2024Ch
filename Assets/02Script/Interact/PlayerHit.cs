using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : HitBase
{
    private int invincibleCount = 0;
    public int InvincibleCount
    {
        get => invincibleCount;
        set
        {
            invincibleCount = value;

            invincible = invincibleCount > 0; // ���� ������ 1ȸ �̻� �׿� ������ ����, �ƴϸ� ���� X
        }
    }

    private SpriteRenderer sprite;
    public override void Init()
    {
        InvincibleCount = 0;

        sprite = GetComponent<SpriteRenderer>();
    }

    public override void HitBy(AttackBase attack)
    {
        GameManager.CurHP -= attack.Damage;

        if (GameManager.CurHP <= 0) // �׾��� ���
        {
            DeadBy(attack);
        }
        else // ����� ���
        {
            StartCoroutine("DamageInvincible");
        }
    }
    [SerializeField]
    private float hitInvincibleTime = 1.5f;
    private IEnumerator DamageInvincible()
    {
        InvincibleCount++;

        float time = 0f;
        while(time <= hitInvincibleTime)
        {
            time += 0.2f;

            sprite.color = sprite.color.a < 0.5f ? Color.white : new Color(0f, 0f, 0f, 0f); // ������ �� �������ϰ�, �������� �� �����ϰ� �����.

            yield return YieldReturn.WaitForSeconds(0.2f);
        }

        sprite.color = Color.white;
        InvincibleCount--;
    }

    protected override void DeadBy(AttackBase attack)
    {
        GameManager.GameStatus = GameStatus.GameOver;

        GetComponentInParent<PlayerController>().enabled = false;
        GetComponentInParent<Rigidbody2D>().simulated = false;
    }
}
