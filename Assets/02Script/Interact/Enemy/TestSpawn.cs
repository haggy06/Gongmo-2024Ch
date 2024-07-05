using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[RequireComponent(typeof(ObjectPool))]
public class TestSpawn : MonoBehaviour
{
    private ObjectPool pool;

    [SerializeField]
    private Vector2 spawnRange1;
    [SerializeField]
    private Vector2 spawnRange2;

    [SerializeField]
    private PoolObject spawnTarget;
    private void Awake()
    {
        pool = GetComponent<ObjectPool>();
    }

    public void Spawn()
    {
        pool.GetPoolObject(spawnTarget).Init(new Vector2(Random.Range(spawnRange1.x, spawnRange2.x), Random.Range(spawnRange1.y, spawnRange2.y)), 0f);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TestSpawn))]
public class Editor_TestSpawn : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TestSpawn script = (TestSpawn)target;

        if (GUILayout.Button("Spawn Object"))
        {
            script.Spawn();
        }
    }
}
#endif