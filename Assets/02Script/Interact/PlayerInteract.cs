using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerInteract : HitBase
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

        GameManager.LevelUPEvent += LevelUP;
    }
    private void OnDestroy()
    {
        GameManager.LevelUPEvent -= LevelUP;
        BombEvent = () => Debug.Log("��8 �̺�Ʈ"); // Bomb �̺�Ʈ �ʱ�ȭ(NullReferenceException ����)
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
    private IEnumerator ItemInvincible()
    {
        InvincibleCount++;
        
        float time = 0f;
        while (time <= 7f)
        {
            time += 0.2f;

            sprite.color = sprite.color.b < 0.5f ? Color.white : Color.yellow; // ��� �� �Ͼ��, �Ͼ� �� ����� �����.

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

    [SerializeField]
    private ParticleSystem[] particleList;

    public static event Action BombEvent = () => Debug.Log("��8 �̺�Ʈ");
    public ParticleSystem[] ParticleList => particleList;
    public void GetItem(Item item)
    {
        particleList[(int)item.ItemType].Play();
        switch (item.ItemType)
        {
            case ItemType.Heal:
                GameManager.CurHP = GameManager.MaxHP;
                break;

            case ItemType.Invincible:
                StartCoroutine("ItemInvincible");
                break;

            case ItemType.Bomb:
                BombEvent.Invoke();
                break;

            case ItemType.Weapon:
                Debug.Log("���� ���� => " + item.WeaponType);
                GameManager.CurWeaponType = item.WeaponType;
                break;
        }
    }
    public void GetItem(ItemType itemType)
    {
        particleList[(int)itemType].Play();
        switch (itemType)
        {
            case ItemType.Heal:
                GameManager.CurHP = GameManager.MaxHP;
                break;

            case ItemType.Invincible:
                StartCoroutine("ItemInvincible");
                break;

            case ItemType.Bomb:
                BombEvent.Invoke();
                break;

            case ItemType.Weapon:
                Debug.LogError("���� ���� �������� ItemType�� ����ؼ� ������ �� �����ϴ�. ");
                break;
        }
    }

    public void LevelUP()
    {
        particleList[4].Play(); // 4�� �ε��� ��ƼŬ = ������ ��ƼŬ
    }
}
