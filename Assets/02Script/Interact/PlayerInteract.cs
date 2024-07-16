using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[RequireComponent(typeof(Animator))]
public class PlayerInteract : HitBase
{
    [Header("Sounds")]
    [SerializeField]
    private AudioClip damageSound;
    [SerializeField]
    private AudioClip deadSound;

    [Space(5)]
    [SerializeField]
    private AudioClip levelUPSound;

    [Space(5)]
    [SerializeField]
    private AudioClip healSound;
    [SerializeField]
    private AudioClip starSound;
    [SerializeField]
    private AudioClip bombSound;
    [SerializeField]
    private AudioClip weaponSound;

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

    public SpriteRenderer sprite { get; private set; }
    public Animator anim { get; private set; }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        GameManager.GameEndEvent += GameEnd;
        BombEvent = () => Debug.Log("��8 �̺�Ʈ");
    }
    private void GameEnd(GameStatus gameStatus)
    {
        switch (gameStatus)
        {
            case GameStatus.Play:
                break;

            case GameStatus.GameOver:
                anim.SetTrigger(EntityAnimHash.Dead);
                sprite.color = Color.grey;

                GetComponentInParent<PlayerController>().enabled = false;
                GetComponentInParent<Rigidbody2D>().simulated = false;
                break;

            case GameStatus.GameClear:
                GetComponentInParent<PlayerController>().enabled = false;
                GetComponentInParent<Rigidbody2D>().simulated = false;
                break;
        }
    }

    public override void Init()
    {
        InvincibleCount = 0;

        GameManager.LevelUPEvent += LevelUP;
    }
    private void OnDestroy()
    {
        GameManager.LevelUPEvent -= LevelUP;
        BombEvent = () => Debug.Log("��8 �̺�Ʈ"); // Bomb �̺�Ʈ �ʱ�ȭ(NullReferenceException ����)
    }

    public override void Hit(EntityType victim, float damage, bool isSkill = false)
    {
        GameManager.CurHP -= (int)Mathf.Round(damage * (1f - damageResistance));

        if (GameManager.CurHP <= 0) // �׾��� ���
        {
            AudioManager.Inst.PlaySFX(deadSound);

            DeadBy(victim);
        }
        else // ����� ���
        {
            AudioManager.Inst.PlaySFX(damageSound);

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
        GetComponent<Collider2D>().enabled = false;
        InvincibleCount--;

        yield return YieldReturn.waitForFixedUpdate;
        GetComponent<Collider2D>().enabled = true;
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

    protected override void DeadBy(EntityType killer)
    {
        GameManager.GameStatus = GameStatus.GameOver;
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
                AudioManager.Inst.PlaySFX(healSound);

                GameManager.CurHP = GameManager.MaxHP;
                break;

            case ItemType.Invincible:
                AudioManager.Inst.PlaySFX(starSound);

                StartCoroutine("ItemInvincible");
                break;

            case ItemType.Bomb:
                AudioManager.Inst.PlaySFX(bombSound);

                BombEvent.Invoke();
                break;

            case ItemType.Weapon:
                AudioManager.Inst.PlaySFX(weaponSound);
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
                AudioManager.Inst.PlaySFX(healSound);

                GameManager.CurHP = GameManager.MaxHP;
                break;

            case ItemType.Invincible:
                AudioManager.Inst.PlaySFX(starSound);

                StartCoroutine("ItemInvincible");
                break;

            case ItemType.Bomb:
                AudioManager.Inst.PlaySFX(bombSound);

                BombEvent.Invoke();
                break;

            case ItemType.Weapon:
                Debug.LogError("���� ���� �������� ItemType�� ����ؼ� ������ �� �����ϴ�. ");
                break;
        }
    }

    public void LevelUP()
    {
        AudioManager.Inst.PlaySFX(levelUPSound);

        particleList[4].Play(); // 4�� �ε��� ��ƼŬ = ������ ��ƼŬ
    }

    protected override void HalfHP()
    {

    }

    protected override void MoribundHP()
    {

    }
}
