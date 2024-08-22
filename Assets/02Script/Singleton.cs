using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using System.IO;
using System;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Inst
    {
        get
        {
            if (instance == null) // ���� �ν��Ͻ��� �Ҵ��� �� �Ǿ��� ���
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null) // �� ���� �ƿ� �ν��Ͻ��� ���� ���
                {
                    T inst = ResourceLoader.PrefabLoad<T>(); // Resources �������� ��ü ��ȯ
                    instance = inst;
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private bool donDestroy = false;
    protected virtual void Awake()
    {
        if (Inst == this) // �� ������Ʈ�� �ν��Ͻ��� ��� (null�̾�� Inst ������Ƽ���� �� �־� �� ����)
        {
            if (donDestroy)
            {
                DontDestroyOnLoad(transform.root.gameObject); // �ֻ��� �θ� �Ҹ� ó���Ѵ�.
            }

            SceneManager.activeSceneChanged += SceneChanged; // ���� ����� ������ SceneChanged�� ����ǵ��� �Ѵ�.
        }
        else // �̹� �ν��Ͻ��� �Ҵ��� �Ǿ� ���� ���
        {
            Debug.Log(instance + "�� �̹� instance�� �־� �� ������Ʈ�� ������");
            Destroy(gameObject); // �ڡ���
        }
    }
    protected virtual void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged;
        if (instance == this)
        {
            instance = null;
        }
    }

    protected abstract void SceneChanged(Scene replacedScene, Scene newScene);
}
