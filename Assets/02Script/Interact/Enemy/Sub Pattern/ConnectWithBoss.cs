using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWithBoss : MonoBehaviour
{
    private PoolObject poolObject;
    private void Start()
    {
        poolObject = GetComponent<PoolObject>();

        GameManager.BossEvent += BossDisappear;
    }

    private void BossDisappear(bool isAppear)
    {
        if (!isAppear) // ������ ����� ���
        {
            poolObject.ReturnToPool(); // �굵 �����
        }
    }
}
