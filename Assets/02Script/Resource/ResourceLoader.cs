using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

public static class ResourceLoader
{ 
    private static Dictionary<Tuple<FolderName, string>, Sprite> spriteCache = new Dictionary<Tuple<FolderName, string>, Sprite>(); // �ҷ��� ���ҽ�  
    public static Sprite SpriteLoad(FolderName folder, string resourceName)
    {
        Sprite value;
        Tuple<FolderName, string> key = new Tuple<FolderName, string>(folder, resourceName);

        if (!spriteCache.TryGetValue(key, out value)) // ĳ�̵� ���ҽ��� ���� ��� �ҷ���
        { // ĳ�̵� ���ҽ��� ���� ���
            value = Resources.Load<Sprite>(Path.Combine("Sprite", Path.Combine(folder.ToString(), resourceName))); // ���ҽ� �ε�

            spriteCache.Add(key, value); // �ҷ��� ���ҽ� ĳ��
        }

        if (value == null)
            Debug.LogError(resourceName + " �ε� ����");

        return value;
    }

    private static Dictionary<Tuple<FolderName, string>, AudioClip> audioCache = new Dictionary<Tuple<FolderName, string>, AudioClip>(); // �ҷ��� ���ҽ�  
    public static AudioClip AudioLoad(FolderName folder, string resourceName)
    {
        AudioClip value;
        Tuple<FolderName, string> key = new Tuple<FolderName, string>(folder, resourceName);

        if (!audioCache.TryGetValue(key, out value)) // ĳ�̵� ���ҽ��� ���� ��� �ҷ���
        { // ĳ�̵� ���ҽ��� ���� ���
            value = Resources.Load<AudioClip>(Path.Combine("Audio", Path.Combine(folder.ToString(), resourceName))); // ���ҽ� �ε�

            audioCache.Add(key, value); // �ҷ��� ���ҽ� ĳ��
        }

        if (value == null)
            Debug.LogError(resourceName + " �ε� ����");

        return value;
    }

    private static Dictionary<string, GameObject> objectCache = new Dictionary<string, GameObject>(); // �ҷ��� ���ҽ�
    public static GameObject PrefabLoad(string resourceName)
    {
        GameObject value;

        if (!objectCache.TryGetValue(resourceName, out value)) // ĳ�̵� ���ҽ��� ���� ��� �ҷ���
        { // ĳ�̵� ���ҽ��� ���� ���
            value = Resources.Load<GameObject>(Path.Combine("Prefab", resourceName)); ; // ���ҽ� �ε�

            objectCache.Add(resourceName, value); // �ҷ��� ���ҽ� ĳ��
        }

        if (value == null)
            Debug.LogError(resourceName + " �ε� ����");

        return value;
    }
}

public enum FolderName
{
    Player,

    BGM,
    Ect,

    Background,
    Icon,
}