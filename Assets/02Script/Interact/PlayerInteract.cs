using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : HitBase
{
    private int invincibleCount = 0;
    public int InvincibleCount
    {
        get => invincibleCount;
        set
        {
            invincibleCount = value;

            invincible = invincibleCount > 0; // 무적 판정이 1회 이상 쌓여 있으면 무적, 아니면 무적 X
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

        if (GameManager.CurHP <= 0) // 죽었을 경우
        {
            DeadBy(attack);
        }
        else // 살았을 경우
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

            sprite.color = sprite.color.a < 0.5f ? Color.white : new Color(0f, 0f, 0f, 0f); // 투명할 땐 불투명하게, 불투명할 땐 투명하게 만든다.

            yield return YieldReturn.WaitForSeconds(0.2f);
        }

        sprite.color = Color.white;
        InvincibleCount--;
    }
    private IEnumerator ItemInvincible()
    {
        InvincibleCount++;
        
        float time = 0f;
        while (time <= 7f)
        {
            time += 0.2f;

            sprite.color = sprite.color.b < 0.5f ? Color.white : Color.yellow; // 노랄 땐 하얗게, 하얄 땐 노랗게 만든다.

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

    public void GetItem(Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Heal:
                GameManager.CurHP = GameManager.MaxHP;
                break;

            case ItemType.Invincible:
                StartCoroutine("ItemInvincible");
                break;

            case ItemType.Bomb:
                Debug.Log("쾅!!!");
                break;

            case ItemType.Weapon:
                Debug.Log("무기 변경 => " + item.WeaponType);
                GameManager.CurWeaponType = item.WeaponType;
                break;
        }
    }
}
