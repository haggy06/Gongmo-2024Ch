using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[RequireComponent(typeof(Animator))]
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

    public SpriteRenderer sprite { get; private set; }
    public Animator anim { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.LevelUPEvent += LevelUP;
        GameManager.GameEndEvent += GameEnd;
        BombEvent = () => Debug.Log("폭8 이벤트");
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
    }
    private void OnDestroy()
    {
        GameManager.LevelUPEvent -= LevelUP;
        BombEvent = () => Debug.Log("폭8 이벤트"); // Bomb 이벤트 초기화(NullReferenceException 방지)
    }

    public override void Hit(EntityType victim, float damage, bool isSkill = false)
    {
        GameManager.CurHP -= (int)Mathf.Round(damage * (1f - damageResistance));

        if (GameManager.CurHP <= 0 && GameManager.GameStatus == GameStatus.Play) // 살아 있는 상태에서 죽었을 경우
        {
            AudioManager.Inst.PlaySFX(ResourceLoader.AudioLoad(FolderName.Player, "Dead"));

            DeadBy(victim);
        }
        else // 살았을 경우
        {
            AudioManager.Inst.PlaySFX(ResourceLoader.AudioLoad(FolderName.Player, "Hit"));

            StartCoroutine("DamageInvincible");
        }
    }
    [Header("Player Interact Setting")]
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
        StartCoroutine("ColliderBlink");
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
        StartCoroutine("ColliderBlink");
    }
    private IEnumerator ColliderBlink()
    {
        GetComponent<Collider2D>().enabled = false;

        yield return YieldReturn.waitForFixedUpdate;
        
        GetComponent<Collider2D>().enabled = true;
    }

    protected override void DeadBy(EntityType killer)
    {
        GameManager.GameStatus = GameStatus.GameOver;
    }

    [Space(5)]
    [SerializeField]
    private Transform particles;

    public static event Action BombEvent = () => Debug.Log("폭8 이벤트");

    public void GetItem(Item item)
    {
        particles.GetChild((int)item.ItemType).GetComponent<ParticleSystem>().Play();
        AudioManager.Inst.PlaySFX(ResourceLoader.AudioLoad(FolderName.Player, item.ItemType.ToString()));

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
                int thisGrade = (int)item.WeaponType % 3 == 0 ? 3 : (int)item.WeaponType % 3;
                int currentGrade = (int)GameManager.CurWeaponType % 3 == 0 ? 3 : (int)GameManager.CurWeaponType % 3;

                if (GameManager.CurWeaponType > WeaponType.Normal && 
                    ((item.WeaponType <= WeaponType.Green3 && GameManager.CurWeaponType <= WeaponType.Green3) || 
                    (item.WeaponType >= WeaponType.Red1 && GameManager.CurWeaponType >= WeaponType.Red1)) &&
                    thisGrade <= currentGrade)
                { // 같은 타입의 총이며 등급이 나와 같거나 더 낮을 경우

                    GameManager.Score += 150 * thisGrade;
                }
                else
                {
                    Debug.Log("무기 변경 => " + item.WeaponType);
                    GameManager.CurWeaponType = item.WeaponType;
                }
                break;
        }
    }
    public void GetItem(ItemType itemType)
    {
        particles.GetChild((int)itemType).GetComponent<ParticleSystem>().Play();
        AudioManager.Inst.PlaySFX(ResourceLoader.AudioLoad(FolderName.Player, itemType.ToString()));

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
                Debug.LogError("무기 변경 아이템은 ItemType을 사용해서 구현할 수 없습니다. ");
                break;
        }
    }

    public void LevelUP(bool isUP)
    {
        sprite.sprite = ResourceLoader.SpriteLoad(FolderName.Player, "Player", GameManager.Level);

        if (isUP) //레벨 업 이벤트의 경우
        {
            AudioManager.Inst.PlaySFX(ResourceLoader.AudioLoad(FolderName.Player, "LevelUp"));
            particles.GetChild(4).GetComponent<ParticleSystem>().Play(); // 4번 인덱스 파티클 = 레벨업 파티클}
        }
    }

    protected override void HalfHP()
    {

    }
}
