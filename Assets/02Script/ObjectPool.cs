using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField, Tooltip("ó�� Ǯ�� ���� �� �� ���� ����� �� �� ���ϱ�")]
    private int initialNumber = 10;
    [SerializeField]
    private PoolObject[] initialObject;
    private void Start()
    {
        foreach (PoolObject obj in initialObject)
        {
            CreatePool(obj);
        }
    }

    private Dictionary<string, Stack<PoolObject>> poolDictionary = new Dictionary<string, Stack<PoolObject>>();
    public PoolObject GetPoolObject(PoolObject poolObject)
    {
        PoolObject value;

        if (!poolDictionary.TryGetValue(poolObject.name, out Stack<PoolObject> pool)) // poolDictionary���� poolObject�� �ش��ϴ� Ǯ�� �ҷ���
        {
            //  poolDictionary�� poolObject�� �ش��ϴ� Ǯ�� ���� ���
            print("Ǯ ���� ����");
            pool = CreatePool(poolObject); // Ǯ ���� ���� �� ����
        }

        if (!pool.TryPop(out value)) // �ҷ��� Ǯ���� ������Ʈ �ϳ� ������
        {
            // ������Ʈ�� ���� �� ���� ���
            value = Instantiate(poolObject.gameObject).GetComponent<PoolObject>(); // ������Ʈ�� �ϳ� ���� �����
            value.name = poolObject.name;
            value.parentPool = this; // �� Ǯ�� �����ѵ�
        }

        value.transform.parent = null;
        value.ExitFromPool();
        return value;
    }

    public void SetPoolObject(PoolObject poolObject)
    {
        if (poolDictionary.TryGetValue(poolObject.name, out Stack<PoolObject> pool)) // poolDictionary���� poolObject�� �ش��ϴ� Ǯ�� �ҷ���
        {
            pool.Push(poolObject); // Ǯ�� ��ȯ�� ������Ʈ �־� ��
            if (poolObjectContainers.TryGetValue(poolObject.name, out Transform container)) // �ش��ϴ� �����̳ʸ� ������
            {
                poolObject.transform.parent = container; // �θ�� ��� ��
            }
            else // �˸��� �����̳ʰ� ���� ���
            {
                Debug.LogWarning(poolObject.name + "�� �ش��ϴ� �����̳� ����");

                //container = Instantiate(new GameObject(), transform).transform; // �����̳ʸ� ���� �����
                container = new GameObject().transform;
                container.parent = transform;
                container.name = poolObject.name + " Pool";
                poolObjectContainers.Add(poolObject.name, container);

                poolObject.transform.parent = container; // �θ�� ��� ��
            }
        }
        else // poolDictionary�� poolObject�� �ش��ϴ� Ǯ�� ���� ���
        {
            Debug.LogError("Ǯ�� �߸� ã�ƿ��� �� �մϴ�.");
        }
    }

    private Dictionary<string, Transform> poolObjectContainers = new Dictionary<string, Transform>(); // ������Ʈ�� ��ȯ�Ǿ��� �� �־�� �����̳ʵ�
    private Stack<PoolObject> CreatePool(PoolObject poolObject)
    {
        Stack<PoolObject> pool;

        if (poolDictionary.TryGetValue(poolObject.name, out pool)) // �̹� Ǯ�� ���� ��� �ҷ���
        {
            if (poolObjectContainers.TryGetValue(poolObject.name, out Transform container))
            {
                Debug.LogWarning("�̹� " + poolObject.name + "�� ���� ������Ʈ Ǯ�� ���� (" + container.name + ")");
            }
            else
            {
                Debug.LogWarning("�̹� " + poolObject.name + "�� ���� ������Ʈ Ǯ�� ����.");
            }
        }
        else // Ǯ�� ���� ���
        {
            pool = new Stack<PoolObject>();
            poolDictionary.Add(poolObject.name, pool);

            //Transform container = Instantiate(new GameObject(), transform).transform;
            Transform container = new GameObject().transform;
            container.parent = transform;
            container.name = poolObject.gameObject.name + " Pool";
            poolObjectContainers.Add(poolObject.name, container);

            for (int i = 0; i < initialNumber; i++) // �ʱ� ���� ����ŭ �ݺ�
            {
                PoolObject obj = Instantiate(poolObject.gameObject, container).GetComponent<PoolObject>(); // ������Ʈ�� �����̳� �ڽ����� ���� �����
                obj.name = poolObject.name;

                obj.parentPool = this; // �� Ǯ�� ����Ų ��
                SetPoolObject(obj); // Ǯ�� �־��
                obj.gameObject.SetActive(false);
            }
        }

        return pool;
    }
}
