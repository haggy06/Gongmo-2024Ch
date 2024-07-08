using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScrollWithBackground))]
public class Coral : EnemyBase
{
    [Space(5)]
    [SerializeField]
    private Color[] coralColorArray;

    [Header("Coral Splint")]
    [SerializeField]
    private Projectile splinter;
    [SerializeField]
    private Vector2 positionOffset;
    [SerializeField]
    private Vector2 angleOffset;
    [SerializeField]
    private int splintNumber;

    private SpriteRenderer sprite;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = coralColorArray[Random.Range(0, coralColorArray.Length)];
    }

    protected override void HalfHP()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 1);
    }

    protected override void MoribundHP()
    {
        
    }
    protected override void Dead(EntityType killer)
    {
        base.Dead(killer);
        Splint();
    }
    public void Splint()
    {
        for (int i = 0; i < splintNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(splinter);
            proj.Init(transform.position + Vector3.one * Random.Range(positionOffset.x, positionOffset.y), 90f + Random.Range(angleOffset.x, angleOffset.y));
            proj.GetComponent<SpriteRenderer>().color = sprite.color;
        }
    }

    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {

    }
}
