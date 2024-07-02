using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : HitBase
{
    [SerializeField, Range(0f, 1f), Tooltip("���� ������� �� �ۼ�Ʈ�� �޾Ƶ����� ����")]
    protected float damageResistance = 0f;

    public override void HitBy(AttackBase attack)
    {
        damageScope = attack.Owner == EntityType.Player ? GameManager.DamageScope : 1f; // �÷��̾��� ������ ��� ����� ���� ����
        damageScope *= (1f - damageResistance); // ����� ������ ����

        curHP = Mathf.Clamp(curHP - (int)Mathf.Round(attack.Damage * damageScope), 0, maxHP);

        if (curHP <= 0) // HP�� 0�� �Ǿ��� ��
        {
            DeadBy(attack); // ��� ó��
        }
        else if (curHP <= maxHP / 4f) // HP�� 1/4���ϰ� �Ǿ��� ��
        {

        }
        else if (curHP <= maxHP / 2f) // HP�� ���� ���ϰ� �Ǿ��� ��
        {

        }        
    }

    protected override void DeadBy(AttackBase attack)
    {
         if (TryGetComponent<PoolObject>(out PoolObject poolObject)) // PoolObject�� ���
        {
            poolObject.ReturnToPool();
        }
        else // PoolObject�� �ƴ� ���
        {
            Destroy(gameObject);
        }
    }
}
