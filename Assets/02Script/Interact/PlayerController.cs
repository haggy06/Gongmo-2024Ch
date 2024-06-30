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
    private Vector2 defaultVelo = Vector2.zero; // �ط� ���� ������ ������� �����̴� �ӵ�
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
        PoolObject bullet = playerPool.GetPoolObject(GameManager.CurBullet.bullet); // ���� ������ �Ѿ� �߻�

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
    private Item[] itemList = new Item[3]; // ȸ��, ����, ��ź ������. ������ ���� �����̴� ������ ũ�� �������.
    [SerializeField]
    private Item[] weaponList = new Item[6]; // ���� ������. 1, 1+, 1++, 2, 2+, 2++ ������ ����.

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
        playerPool.GetPoolObject(weaponList[RandomWeapon() - 1]).transform.position = Vector2.zero; // WeaponType.Normal�� �ش��ϴ� ���� �����Ǿ� �����Ƿ� ���� �� - 1�� ���� �ε����� �����.
    }
    public void SpawnWeapon(Vector2 position)
    {
        playerPool.GetPoolObject(weaponList[RandomWeapon() - 1]).transform.position = position; // WeaponType.Normal�� �ش��ϴ� ���� �����Ǿ� �����Ƿ� ���� �� - 1�� ���� �ε����� �����.
    }
    private int RandomWeapon()
    {
        int weaponType;
        // 0�� ���� 1, 1�� ���� 2�� ���� ��Ȳ��.
        if (Random.Range(0, 2) == 0) // ���� 1�� ���
        {
            if (GameManager.CurWeaponType <= WeaponType.Green1 && GameManager.CurWeaponType <= WeaponType.Green3) // ���� 1�� ������ ���¿��� ���
            {
                weaponType = Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Green1, (int)WeaponType.Green3); // ���� ���⿡ + 1��ȭ ����(�ִ� ��ȭ�� ���� ����)
            }
            else // �ٸ� ���⸦ ������ ���¿��� ���
            {
                weaponType = (int)WeaponType.Green1; // �⺻ ���� 1 ����
            }
        }
        else // ���� 2�� ���
        {
            if (GameManager.CurWeaponType <= WeaponType.Red1 && GameManager.CurWeaponType <= WeaponType.Red3) // ���� 2�� ������ ���¿��� ���
            {
                weaponType = Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Red1, (int)WeaponType.Red3); // ���� ���⿡ + 1��ȭ ����(�ִ� ��ȭ�� ���� ����)
            }
            else // �ٸ� ���⸦ ������ ���¿��� ���
            {
                weaponType = (int)WeaponType.Red1; // �⺻ ���� 2 ����
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
            SpawnItem();
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

    private void FixedUpdate()
    {
        rigid2D.velocity = defaultVelo + (move * moveSpeed);
    }    
}
