using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField]
    private float shootTerm = 0.1f;

    [SerializeField]
    private float damagePercent = 1f;

    private bool attackCoolDown = true;
    #endregion

    private Rigidbody2D rigid2D;
    private ObjectPool playerPool;

    private PlayerHit hit;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        playerPool = GetComponentInChildren<ObjectPool>();

        hit = GetComponentInChildren<PlayerHit>();
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
    private IEnumerator Attack()
    {
        PoolObject bullet = playerPool.GetPoolObject(GameManager.CurBullet); // 현재 장착된 총알 발사

        bullet.transform.position = transform.position;
        bullet.transform.eulerAngles = Vector3.forward * 90f;

        attackCoolDown = false;

        yield return YieldReturn.WaitForSeconds(shootTerm);

        attackCoolDown = true;
    }

    private void FixedUpdate()
    {
        rigid2D.velocity = defaultVelo + (move * moveSpeed);
    }
}
