using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : PoolObject // ȸ���� ���� ȥ�� ������ �� ���� ��ֹ��� ��ȯ�� �ִ� ����
{
    [SerializeField]
    private PoolObject loadedObject;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        parentPool.GetPoolObject(loadedObject).Init(transform.position, -90f);

        ReturnToPool();
    }
}
