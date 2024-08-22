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
            if (instance == null) // 아직 인스턴스가 할당이 안 되었을 경우
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null) // 씬 내에 아예 인스턴스가 없을 경우
                {
                    T inst = ResourceLoader.PrefabLoad<T>(); // Resources 폴더에서 개체 소환
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
        if (Inst == this) // 이 오브젝트가 인스턴스일 경우 (null이었어도 Inst 프로퍼티에서 뭘 넣어 줄 거임)
        {
            if (donDestroy)
            {
                DontDestroyOnLoad(transform.root.gameObject); // 최상위 부모를 불멸 처리한다.
            }

            SceneManager.activeSceneChanged += SceneChanged; // 씬이 변경될 때마다 SceneChanged가 실행되도록 한다.
        }
        else // 이미 인스턴스가 할당이 되어 있을 경우
        {
            Debug.Log(instance + "가 이미 instance에 있어 이 오브젝트는 삭제함");
            Destroy(gameObject); // 자★폭
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
