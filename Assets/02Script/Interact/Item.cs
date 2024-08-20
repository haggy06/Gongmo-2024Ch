using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Item : PoolObject
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private ItemType itemType;
    public ItemType ItemType => itemType;
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType => weaponType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out PlayerInteract playerInteract)) // �÷��̾ ����� ���
        {
            playerInteract.GetItem(this);

            ReturnToPool();
        }
    }

    protected virtual void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec((Random.Range(0, 4) * 90f) + 45f) * speed; // 45, 135, 225, 315�� �� �� ������ Ʀ
    }

    public void InitItem(ItemType itemType)
    {
        if (itemType == ItemType.Weapon)
        {
            Debug.LogWarning("���� Ÿ�� �������� WeaponType�� �־���� �մϴ�.");
            return;
        }

        this.itemType = itemType;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ResourceLoader.SpriteLoad(FolderName.Icon, itemType.ToString());
    }
    public void InitItem(WeaponType weaponType)
    {
        itemType = ItemType.Weapon;
        this.weaponType = weaponType;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ResourceLoader.SpriteLoad(FolderName.Icon, weaponType.ToString());
    }
}

public enum ItemType
{
    Heal,
    Invincible,
    Bomb,

    Weapon,
}