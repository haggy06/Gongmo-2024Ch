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
    private Vector2 defaultVelo = Vector2.zero; // �ط� ���� ������ ������� �����̴� �ӵ�
    public Vector2 DefaultVelo { get => defaultVelo; set => defaultVelo = value; }
    #endregion

    #region _About Attack_
    private bool attackCoolDown = true;

    private IEnumerator Attack()
    {
        PoolObject bullet = playerPool.GetPoolObject(GameManager.CurWeapon.bullet); // ���� ������ �Ѿ� �߻�

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
        // 0�� ���� 1, 1�� ���� 2�� ���� ��Ȳ��.
        if (Random.Range(0, 2) == 0) // ���� 1�� ���
        {
            if (WeaponType.Green1 <= GameManager.CurWeaponType && GameManager.CurWeaponType <= WeaponType.Green3) // ���� 1�� ������ ���¿��� ���
            {
                Debug.Log("��ȭ�� ���� ����");
                weaponType = (WeaponType)Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Green1, (int)WeaponType.Green3); // ���� ���⿡ + 1��ȭ ����(�ִ� ��ȭ�� ���� ����)
            }
            else // �ٸ� ���⸦ ������ ���¿��� ���
            {
                weaponType = WeaponType.Green1; // �⺻ ���� 1 ����
            }
        }
        else // ���� 2�� ���
        {
            if (WeaponType.Red1 <= GameManager.CurWeaponType && GameManager.CurWeaponType <= WeaponType.Red3) // ���� 2�� ������ ���¿��� ���
            {
                Debug.Log("��ȭ�� ���� ����");
                weaponType = (WeaponType)Mathf.Clamp((int)GameManager.CurWeaponType + 1, (int)WeaponType.Red1, (int)WeaponType.Red3); // ���� ���⿡ + 1��ȭ ����(�ִ� ��ȭ�� ���� ����)
            }
            else // �ٸ� ���⸦ ������ ���¿��� ���
            {
                weaponType = WeaponType.Red1; // �⺻ ���� 2 ����
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
        player = this; // ������ �÷��̾�� Play ���� �ϳ��ۿ� ���� �װ� DonDestroyOnLoad�� �ƴϴ� �̱����� �����ϰ� ������ �ʾƵ� �� �� ��.

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


        if (attackCoolDown && Input.GetKey(KeyCode.X)) // ����
        {
            StartCoroutine("Attack");
        }
        if (Input.GetKey(KeyCode.Space) && GameManager.Skill >= 100f) // ��ų
        {
            interact.anim.SetInteger(EntityAnimHash.Pattern, 1);
        }

        #region _Cheat Input_
        if (Input.GetKeyDown(KeyCode.F1)) // ���� / ����
        {
            Debug.Log("���� / ����");

            GameManager.UseCheat = true;
            useInvincibleCheat = !useInvincibleCheat;
            interact.InvincibleCount += useInvincibleCheat ? 1 : -1;
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

            interact.GetItem(ItemType.Heal);
            GameManager.CurHP = GameManager.MaxHP;
        }

        if (Input.GetKeyDown(KeyCode.F5)) // ���� ��������
        {
            Debug.Log("���� ��������");

            GameManager.UseCheat = true;
            SpawnWeapon();
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
            PopupManager.Inst.Open_CloseTable();
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
    public void SkillLaunch()
    {
        print("��ų ���");
        GameManager.Skill = 0f;
        playerPool.GetPoolObject(GameManager.CurWeapon.skill).Init(transform.position, 90f);

        interact.anim.SetInteger(EntityAnimHash.Pattern, 0);
    }

    private void FixedUpdate()
    {
        rigid2D.velocity = defaultVelo + (move * moveSpeed);
    }    
}
