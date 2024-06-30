using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
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
    [Header("About Attack")]

    [SerializeField]
    private float shootTerm = 0.1f;

    [SerializeField]
    private float damagePercent = 1f;

    private bool attackCoolDown = true;

    private IEnumerator Attack()
    {
        PoolObject bullet = playerPool.GetPoolObject(GameManager.CurBullet.bullet); // 현재 장착된 총알 발사

        bullet.transform.position = transform.position;
        bullet.transform.eulerAngles = Vector3.forward * 90f;

        attackCoolDown = false;

        yield return YieldReturn.WaitForSeconds(GameManager.CurBullet.coolDown);

        attackCoolDown = true;
    }
    #endregion

    #region _About Item_
    [Header("About Item")]

    [SerializeField]
    private Item[] itemList = new Item[3]; // 회복, 무적, 폭탄 아이템. 어차피 랜덤 생성이니 순서는 크게 상관없음.
    [SerializeField]
    private Item[] weaponList = new Item[6]; // 무기 아이템. 1, 1+, 1++, 2, 2+, 2++ 순서로 넣음.

    public void SpawnItem()
    {
        playerPool.GetPoolObject(itemList[Random.Range(0, 3)]).transform.position = Vector2.zero;
    }
    public void SpawnItem(Vector2 position)
    {
        playerPool.GetPoolObject(itemList[Random.Range(0, 3)]).transform.position = position;
    }

    public void SpawnWeapon()
    {
        playerPool.GetPoolObject(weaponList[RandomWeapon() - 1]).transform.position = Vector2.zero; // WeaponType.Normal에 해당하는 값이 생략되어 있으므로 나온 값 - 1을 해줘 인덱스를 맞춘다.
    }
    public void SpawnWeapon(Vector2 position)
    {
        playerPool.GetPoolObject(weaponList[RandomWeapon() - 1]).transform.position = position; // WeaponType.Normal에 해당하는 값이 생략되어 있으므로 나온 값 - 1을 해줘 인덱스를 맞춘다.
    }
    private int RandomWeapon()
    {
        int weaponType;
        // 0은 무기 1, 1은 무기 2가 나온 상황임.
        if (Random.Range(0, 2) == 0) // 무기 1의 경우
        {
            if (GameManager.CurWeaponType <= WeaponType.Green1 && GameManager.CurWeaponType <= WeaponType.Green3) // 무기 1을 장착한 상태였을 경우
            {
                weaponType = Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Green1, (int)WeaponType.Green3); // 현재 무기에 + 1강화 붙임(최대 강화를 넘지 않음)
            }
            else // 다른 무기를 장착한 상태였을 경우
            {
                weaponType = (int)WeaponType.Green1; // 기본 무기 1 지급
            }
        }
        else // 무기 2의 경우
        {
            if (GameManager.CurWeaponType <= WeaponType.Red1 && GameManager.CurWeaponType <= WeaponType.Red3) // 무기 2를 장착한 상태였을 경우
            {
                weaponType = Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Red1, (int)WeaponType.Red3); // 현재 무기에 + 1강화 붙임(최대 강화를 넘지 않음)
            }
            else // 다른 무기를 장착한 상태였을 경우
            {
                weaponType = (int)WeaponType.Red1; // 기본 무기 2 지급
            }
        }

        return weaponType;
    }
    #endregion

    private Rigidbody2D rigid2D;
    private ObjectPool playerPool;

    private PlayerInteract hit;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        playerPool = GetComponentInChildren<ObjectPool>();

        hit = GetComponentInChildren<PlayerInteract>();
    }
    private bool useInvincibleCheat = false;
    private void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        if (attackCoolDown && Input.GetKey(KeyCode.X)) // 공격
        {
            StartCoroutine("Attack");
        }

        #region _Cheat Input_
        if (Input.GetKeyDown(KeyCode.F1)) // 무적 / 비무적
        {
            Debug.Log("무적 / 비무적");

            GameManager.UseCheat = true;
            useInvincibleCheat = !useInvincibleCheat;
            hit.InvincibleCount += useInvincibleCheat ? 1 : -1;
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
            GameManager.CurHP = GameManager.MaxHP;
        }

        if (Input.GetKeyDown(KeyCode.F5)) // 무기 랜덤생성
        {
            Debug.Log("무기 랜덤생성");

            GameManager.UseCheat = true;
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

    private void FixedUpdate()
    {
        rigid2D.velocity = defaultVelo + (move * moveSpeed);
    }    
}
