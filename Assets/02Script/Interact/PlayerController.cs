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
    private Vector2 defaultVelo = Vector2.zero; // �ط� ���� ������ ������� �����̴� �ӵ�
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

        if (attackCoolDown && Input.GetKey(KeyCode.X)) // ����
        {
            StartCoroutine("Attack");
        }

        #region _Cheat Input_
        if (Input.GetKeyDown(KeyCode.F1)) // ���� / ����
        {
            Debug.Log("���� / ����");

            GameManager.UseCheat = true;
            useInvincibleCheat = !useInvincibleCheat;
            hit.InvincibleCount += useInvincibleCheat ? 1 : -1;
        }

        if (Input.GetKeyDown(KeyCode.F2)) // ���� + 1
        {
            Debug.Log("���� + 1");

            GameManager.UseCheat = true;
            GameManager.Level++;
        }

        if (Input.GetKeyDown(KeyCode.F3)) // ���� - 1
        {
            Debug.Log("���� - 1");

            GameManager.UseCheat = true;
            GameManager.Level--;
        }

        if (Input.GetKeyDown(KeyCode.F4)) // Ǯ��
        {
            Debug.Log("Ǯ��");

            GameManager.UseCheat = true;
            GameManager.CurHP = GameManager.MaxHP;
        }

        if (Input.GetKeyDown(KeyCode.F5)) // ���� ��������
        {
            Debug.Log("���� ��������");

            GameManager.UseCheat = true;
            // todo : ���� ��������
        }

        if (Input.GetKeyDown(KeyCode.F6)) // ������ ��������
        {
            Debug.Log("������ ��������");

            GameManager.UseCheat = true;
            // todo : ������ ��������
        }

        if (Input.GetKeyDown(KeyCode.F7)) // ���ھ� ��� (���̵��� ����� ��ŭ)
        {
            Debug.Log("���ھ� ��� (���̵��� ����� ��ŭ)");

            GameManager.UseCheat = true;
            GameManager.Score += 1000;
        }

        if (Input.GetKeyDown(KeyCode.F8)) // �÷��̾� ���� �� ����ġ ������ ON/OFF
        {
            Debug.Log("�÷��̾� ���� �� ����ġ ������ ON/OFF");

            GameManager.UseCheat = true;
            // todo : �÷��̾� ���� �� ����ġ ������ ON/OFF
        }

        if (Input.GetKeyDown(KeyCode.P)) // �Ͻ�����
        {
            Debug.Log("�Ͻ�����");

            GameManager.UseCheat = true;
            Time.timeScale = Time.timeScale > 0.5f ? 0f : 1f;
        }
        #endregion
    }
    private IEnumerator Attack()
    {
        PoolObject bullet = playerPool.GetPoolObject(GameManager.CurBullet); // ���� ������ �Ѿ� �߻�

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
