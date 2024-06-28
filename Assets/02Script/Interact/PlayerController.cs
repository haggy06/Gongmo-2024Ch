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

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        playerPool = GetComponentInChildren<ObjectPool>();
    }
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
            // todo : 무적 / 비무적
        }

        if (Input.GetKeyDown(KeyCode.F2)) // 레벨 + 1
        {
            // todo : 레벨 + 1
        }

        if (Input.GetKeyDown(KeyCode.F3)) // 레벨 - 1
        {
            // todo : 레벨 - 1
        }

        if (Input.GetKeyDown(KeyCode.F4)) // 풀피
        {
            // todo : 풀피
        }

        if (Input.GetKeyDown(KeyCode.F5)) // 무기 랜덤생성
        {
            // todo : 무기 랜덤생성
        }

        if (Input.GetKeyDown(KeyCode.F6)) // 아이템 랜덤생성
        {
            // todo : 아이템 랜덤생성
        }

        if (Input.GetKeyDown(KeyCode.F7)) // 스코어 상승 (난이도가 변경될 만큼)
        {
            // todo : 스코어 상승 (난이도가 변경될 만큼)
        }

        if (Input.GetKeyDown(KeyCode.F8)) // 플레이어 레벨 및 경험치 게이지 ON/OFF
        {
            // todo : 플레이어 레벨 및 경험치 게이지 ON/OFF
        }

        if (Input.GetKeyDown(KeyCode.P)) // 일시정지
        {
            // todo : 일시정지
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
