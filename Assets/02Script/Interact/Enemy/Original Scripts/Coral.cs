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
    private AudioClip crackSound;
    [Space(5)]
    [SerializeField]
    private Sprite crackedSprite;
    [SerializeField]
    private AudioClip breakSound;

    [Header("Coral Splint")]
    [SerializeField]
    protected PoolObject splinter;
    [SerializeField]
    protected Vector2 positionOffset;
    [SerializeField]
    protected Vector2 angleOffset;
    [SerializeField]
    protected int splintNumber;

    private SpriteRenderer _sprite;
    protected override void Awake()
    {
        base.Awake();

        _sprite = GetComponentInChildren<SpriteRenderer>();
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        _sprite.sprite = neatSprite;
        _sprite.color = coralColorArray[Random.Range(0, coralColorArray.Length)];
        enemyInteract.SaveOriginalColor();
    }

    protected override void HalfHP()
    {
        AudioManager.Inst.PlaySFX(crackSound);
        _sprite.sprite = crackedSprite;
        anim.SetInteger(EntityAnimHash.Pattern, 1);
    }

    protected override void Dead(EntityType killer)
    {
        base.Dead(killer);
        AudioManager.Inst.PlaySFX(breakSound);
        Splint();
    }
    public virtual void Splint()
    {
        for (int i = 0; i < splintNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(splinter);
            proj.Init(transform.position + Vector3.one * Random.Range(positionOffset.x, positionOffset.y), 90f + Random.Range(angleOffset.x, angleOffset.y));
            proj.GetComponent<SpriteRenderer>().color = enemyInteract.OriginalColor;
        }

        StabilizePattern();
    }

    public override void Pattern(int caseNumber)
    {

    }
}
