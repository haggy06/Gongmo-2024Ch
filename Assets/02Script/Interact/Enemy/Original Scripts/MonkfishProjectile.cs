using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(MonkfishLightball))]
public class MonkfishProjectile : PoolObject
{
    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private float speed;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec(angle) * speed;

        Invoke("BlinkStart", lifeTime);
    }
    private void BlinkStart()
    {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<Animator>().SetTrigger(EntityAnimHash.Pattern);
        }
    }
}
