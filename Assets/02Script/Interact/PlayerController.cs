using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController player;
    public static PlayerController Player => player;

    #region _About Move_
    [Header("About Move")]

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    Vector2 move = Vector2.zero;

    [SerializeField]
    private Vector2 defaultVelo = Vector2.zero; // 해류 같이 의지와 상관없이 움직이는 속도
    public Vector2 DefaultVelo { get => defaultVelo; set => defaultVelo = value; }
    #endregion

    #region _About Attack_
    private bool attackCoolDown = true;

    private IEnumerator Attack()
    {
        PoolObject bullet = playerPool.GetPoolObject(GameManager.CurWeapon.bullet); // 현재 장착된 총알 발사

        bullet.Init(transform.position, 90f);

        attackCoolDown = false;

        yield return YieldReturn.WaitForSeconds(GameManager.CurWeapon.coolDown);

        attackCoolDown = true;
    }
    #endregion

    #region _About Item_
    [Header("About Item")]

    [SerializeField]
    private Item item;

    public void SpawnItem()
    {
        Item item = (Item)playerPool.GetPoolObject(this.item);

        item.transform.position = Vector2.zero;
        item.InitItem((ItemType)Random.Range(0, 3));
    }
    public void SpawnItem(Vector2 position)
    {
        Item item = (Item)playerPool.GetPoolObject(this.item);

        item.transform.position = position;
        item.InitItem((ItemType)Random.Range(0, 3));
    }

    public void SpawnWeapon()
    {
        Item item = (Item)playerPool.GetPoolObject(this.item);

        item.transform.position = Vector2.zero;
        item.InitItem(RandomWeapon());
    }
    public void SpawnWeapon(Vector2 position)
    {
        Item item = (Item)playerPool.GetPoolObject(this.item);

        item.transform.position = position;
        item.InitItem(RandomWeapon());
    }
    private WeaponType RandomWeapon()
    {
        WeaponType weaponType;
        // 0은 무기 1, 1은 무기 2가 나온 상황임.
        if (Random.Range(0, 2) == 0) // 무기 1의 경우
        {
            if (WeaponType.Green1 <= GameManager.CurWeaponType && GameManager.CurWeaponType <= WeaponType.Green3) // 무기 1을 장착한 상태였을 경우
            {
                Debug.Log("강화된 무기 생성");
                weaponType = (WeaponType)Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Green1, (int)WeaponType.Green3); // 현재 무기에 + 1강화 붙임(최대 강화를 넘지 않음)
            }
            else // 다른 무기를 장착한 상태였을 경우
            {
                weaponType = WeaponType.Green1; // 기본 무기 1 지급
            }
        }
        else // 무기 2의 경우
        {
            if (WeaponType.Red1 <= GameManager.CurWeaponType && GameManager.CurWeaponType <= WeaponType.Red3) // 무기 2를 장착한 상태였을 경우
            {
                Debug.Log("강화된 무기 생성");
                weaponType = (WeaponType)Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Red1, (int)WeaponType.Red3); // 현재 무기에 + 1강화 붙임(최대 강화를 넘지 않음)
            }
            else // 다른 무기를 장착한 상태였을 경우
            {
                weaponType = WeaponType.Red1; // 기본 무기 2 지급
            }
        }

        return weaponType;
    }
    #endregion

    private Rigidbody2D rigid2D;
    private ObjectPool playerPool;

    private PlayerInteract interact;

    private void Awake()
    {
        player = this; // 어차피 플레이어는 Play 씬에 하나밖에 없을 테고 DonDestroyOnLoad도 아니니 싱글톤을 복잡하게 만들지 않아도 될 듯 함.

        rigid2D = GetComponent<Rigidbody2D>();
        playerPool = GetComponentInChildren<ObjectPool>();

        interact = GetComponentInChildren<PlayerInteract>();
    }

    private bool useInvincibleCheat = false;
    private void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move = move.normalized;


        if (attackCoolDown && Input.GetKey(KeyCode.X)) // 공격
        {
            StartCoroutine("Attack");
        }
        if (Input.GetKey(KeyCode.Space) && GameManager.Skill >= 100f) // 스킬
        {
            interact.anim.SetInteger(EntityAnimHash.Pattern, 1);
        }

        #region _Cheat Input_
        if (Input.GetKeyDown(KeyCode.F1)) // 무적 / 비무적
        {
            Debug.Log("무적 / 비무적");

            GameManager.UseCheat = true;
            useInvincibleCheat = !useInvincibleCheat;
            interact.InvincibleCount += useInvincibleCheat ? 1 : -1;
        }

        if (Input.GetKeyDown(KeyCode.F2)) // 레벨 + 1
        {
            Debug.Log("레벨 + 1");

            GameManager.UseCheat = true;
            GameManager.Level++;
        }

        if (Input.GetKeyDown(KeyCode.F3)) // 레벨 - 1
        {
            Debug.Log("레벨 - 1");

            GameManager.UseCheat = true;
            GameManager.Level--;
        }

        if (Input.GetKeyDown(KeyCode.F4)) // 풀피
        {
            Debug.Log("풀피");

            GameManager.UseCheat = true;

            interact.GetItem(ItemType.Heal);
            GameManager.CurHP = GameManager.MaxHP;
        }

        if (Input.GetKeyDown(KeyCode.F5)) // 무기 랜덤생성
        {
            Debug.Log("무기 랜덤생성");

            GameManager.UseCheat = true;
            SpawnWeapon();
            // todo : 무기 랜덤생성
        }

        if (Input.GetKeyDown(KeyCode.F6)) // 아이템 랜덤생성
        {
            Debug.Log("아이템 랜덤생성");

            GameManager.UseCheat = true;
            SpawnItem();
            // todo : 아이템 랜덤생성
        }

        if (Input.GetKeyDown(KeyCode.F7)) // 스코어 상승 (난이도가 변경될 만큼)
        {
            Debug.Log("스코어 상승 (난이도가 변경될 만큼)");

            GameManager.UseCheat = true;
            GameManager.Score += 1000;
        }

        if (Input.GetKeyDown(KeyCode.F8)) // 플레이어 레벨 및 경험치 게이지 ON/OFF
        {
            Debug.Log("플레이어 레벨 및 경험치 게이지 ON/OFF");

            GameManager.UseCheat = true;
            PopupManager.Inst.Open_CloseTable();
            // todo : 플레이어 레벨 및 경험치 게이지 ON/OFF
        }

        if (Input.GetKeyDown(KeyCode.P)) // 일시정지
        {
            Debug.Log("일시정지");

            GameManager.UseCheat = true;
            Time.timeScale = Time.timeScale > 0.5f ? 0f : 1f;
        }
        #endregion
    }
    public void SkillLaunch()
    {
        print("스킬 사용");
        GameManager.Skill = 0f;
        playerPool.GetPoolObject(GameManager.CurWeapon.skill).Init(transform.position, 90f);

        interact.anim.SetInteger(EntityAnimHash.Pattern, 0);
    }

    private void FixedUpdate()
    {
        rigid2D.velocity = defaultVelo + (move * moveSpeed);
    }    
}
