using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScrollWithBackground))]
public class Coral : EnemyBase
{
    [Space(5)]
    [SerializeField]
    private Color[] coralColorArray;

    [SerializeField]
    private Sprite neatSprite;
    [SerializeField]
    private Sprite crackedSprite;

    [Header("Coral Splint")]
    [SerializeField]
    protected PoolObject splinter;
    [SerializeField]
    protected Vector2 positionOffset;
    [SerializeField]
    protected Vector2 angleOffset;
    [SerializeField]
    protected int splintNumber;

    private SpriteRenderer sprite;
    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        sprite.sprite = neatSprite;
        sprite.color = coralColorArray[Random.Range(0, coralColorArray.Length)];
        enemyInteract.SaveOriginalColor();
    }

    protected override void HalfHP()
    {
        sprite.sprite = crackedSprite;
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
    public virtual void Splint()
    {
        for (int i = 0; i < splintNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(splinter);
            proj.Init(transform.position + Vector3.one * Random.Range(positionOffset.x, positionOffset.y), 90f + Random.Range(angleOffset.x, angleOffset.y));
            proj.GetComponent<SpriteRenderer>().color = enemyInteract.originalColor;
        }
    }

    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {

    }
}
