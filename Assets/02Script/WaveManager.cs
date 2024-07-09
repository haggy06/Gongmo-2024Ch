using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private StageInfo[] stageInfoArray = new StageInfo[3];

    private ObjectPool pool;
    private void Start()
    {
        pool = GetComponent<ObjectPool>();

        GameManager.BossEvent += BossEvent;
        StartCoroutine("SpawnEnemyCor");
        StartCoroutine("SpawnBossCor");
    }
    private void BossEvent(bool isAppear)
    {
        if (isAppear)
        {
            StopCoroutine("SpawnEnemyCor");
        }
        else
        {
            StartCoroutine("SpawnEnemyCor");
            StartCoroutine("SpawnBossCor");
        }
    }

    [SerializeField]
    private Vector2 bossSpawn;
    [Space(10)]
    [SerializeField]
    private Vector2 spawnRange1;
    [SerializeField]
    private Vector2 spawnRange2;
    private IEnumerator SpawnEnemyCor()
    {
        while (true)
        {
            StageInfo curStageInfo = stageInfoArray[GameManager.Stage - 1];

            PoolObject randomObject = curStageInfo.stageEnemies[Random.Range(0, curStageInfo.stageEnemies.Length)];
            Vector2 randomPosition = new Vector2(Random.Range(spawnRange1.x, spawnRange2.x), Random.Range(spawnRange1.y, spawnRange2.y));

            pool.GetPoolObject(randomObject).Init(randomPosition, 0f);

            yield return YieldReturn.WaitForSeconds(curStageInfo.spawnInterval);
        }
    }

    public const float bossInterval = 60f;
    private IEnumerator SpawnBossCor()
    {
        yield return YieldReturn.WaitForSeconds(bossInterval);

        PopupManager.Inst.BossAppear();
        Invoke("LaunchBoss", PopupManager.BossWarningTime);
    }
    private void LaunchBoss()
    {
        GameManager.Inst.BossAppear();
        pool.GetPoolObject(stageInfoArray[GameManager.Stage - 1].stageBoss).Init(bossSpawn, 0f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines(); // 잔버그 방지를 위해 파괴 시 코루틴 모두 종료
    }
}

[System.Serializable]
public struct StageInfo
{
    public PoolObject stageBoss;

    public float spawnInterval;
    public PoolObject[] stageEnemies;
}