using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField, Tooltip("ó�� Ǯ�� ���� �� �� ���� ����� �� �� ���ϱ�")]
    private int initialNumber = 10;
    [SerializeField]
    private PoolObject[] initialObject;
    private void Awake()
    {
        foreach (PoolObject obj in initialObject)
        {
            CreatePool(obj);
        }
    }

    private Dictionary<int, Stack<PoolObject>> poolDictionary = new Dictionary<int, Stack<PoolObject>>();
    public PoolObject GetPoolObject(PoolObject poolObject)
    {
        PoolObject value;

        if (!poolDictionary.TryGetValue(poolObject.PoolObjectID, out Stack<PoolObject> pool)) // poolDictionary���� poolObject�� �ش��ϴ� Ǯ�� �ҷ���
        {
            //  poolDictionary�� poolObject�� �ش��ϴ� Ǯ�� ���� ���
            print("Ǯ ���� ����");
            pool = CreatePool(poolObject); // Ǯ ���� ���� �� ����
        }

        if (!pool.TryPop(out value)) // �ҷ��� Ǯ���� ������Ʈ �ϳ� ������
        {
            // ������Ʈ�� ���� �� ���� ���
            value = Instantiate(poolObject.gameObject).GetComponent<PoolObject>(); // ������Ʈ�� �ϳ� ���� �����
            value.parentPool = this; // �� Ǯ�� �����ѵ�
        }

        value.transform.parent = null;
        value.ExitFromPool();
        return value;
    }

    public void SetPoolObject(PoolObject poolObject)
    {
        if (poolDictionary.TryGetValue(poolObject.PoolObjectID, out Stack<PoolObject> pool)) // poolDictionary���� poolObject�� �ش��ϴ� Ǯ�� �ҷ���
        {
            pool.Push(poolObject); // Ǯ�� ��ȯ�� ������Ʈ �־� ��
            if (poolObjectContainers.TryGetValue(poolObject.PoolObjectID, out Transform container)) // �ش��ϴ� �����̳ʸ� ������
            {
                poolObject.transform.parent = container; // �θ�� ��� ��
            }
            else // �˸��� �����̳ʰ� ���� ���
            {
                Debug.LogWarning(poolObject.gameObject.name + "�� �ش��ϴ� �����̳� ����");

                container = Instantiate(new GameObject(), transform).transform; // �����̳ʸ� ���� �����
                container.name = poolObject.gameObject.name + " Pool";
                poolObjectContainers.Add(poolObject.PoolObjectID, container);

                poolObject.transform.parent = container; // �θ�� ��� ��
            }
        }
        else // poolDictionary�� poolObject�� �ش��ϴ� Ǯ�� ���� ���
        {
            Debug.LogError("Ǯ�� �߸� ã�ƿ��� �� �մϴ�.");
        }
    }

    private Dictionary<int,Transform> poolObjectContainers = new Dictionary<int, Transform>(); // ������Ʈ�� ��ȯ�Ǿ��� �� �־�� �����̳ʵ�
    private Stack<PoolObject> CreatePool(PoolObject poolObject)
    {
        Stack<PoolObject> pool;

        if (poolDictionary.TryGetValue(poolObject.PoolObjectID, out pool)) // �̹� Ǯ�� ���� ��� �ҷ���
        {
            Debug.LogWarning("�̹� targetObject�� ���� ������Ʈ Ǯ�� ����.");
        }
        else // Ǯ�� ���� ���
        {
            pool = new Stack<PoolObject>();
            poolDictionary.Add(poolObject.PoolObjectID, pool);

            Transform container = Instantiate(new GameObject(), transform).transform;
            container.name = poolObject.gameObject.name + " Pool";
            poolObjectContainers.Add(poolObject.PoolObjectID, container);

            for (int i = 0; i < initialNumber; i++) // �ʱ� ���� ����ŭ �ݺ�
            {
                PoolObject obj = Instantiate(poolObject.gameObject, container).GetComponent<PoolObject>(); // ������Ʈ�� �����̳� �ڽ����� ���� �����

                obj.parentPool = this; // �� Ǯ�� ����Ų ��
                //obj.ReturnToPool(); // Ǯ�� �־��
                obj.gameObject.SetActive(false);
            }
        }

        return pool;
    }
}
