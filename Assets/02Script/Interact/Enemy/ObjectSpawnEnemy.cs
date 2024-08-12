using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectSpawnEnemy : PoolObject
{
    [SerializeField]
    private float speed;

    [Space(5)]
    [SerializeField]
    private bool spawnWhenInit = true;
    [SerializeField]
    private float spawnTerm;
    [SerializeField]
    private PoolObject spawnObject;

    private IEnumerator PatternRepeat()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return YieldReturn.WaitForSeconds(spawnTerm);

            parentPool.GetPoolObject(spawnObject).Init(transform.position, transform.eulerAngles.z);
        }
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        if (spawnWhenInit)
            parentPool.GetPoolObject(spawnObject).Init(transform.position, transform.eulerAngles.z);

        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec(angle) * speed;
        StartCoroutine("PatternCor");
    }

    protected override void ObjectReturned()
    {
        base.ObjectReturned();

        StopCoroutine("PatternCor");
    }
}
