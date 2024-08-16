using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyInteract : HitBase
{
    [Header("Enemy Info")]
    [SerializeField, Range(0f, 1f), Tooltip("받은 대미지의 몇 퍼센트를 스킬로 치환해줄 지 정하는 필드")]
    private float skillEff = 0.1f;

    [Space(5)]

    [SerializeField, Range(0f, 1f)]
    protected float itemDropProbability = 0.1f;

    [Space(5)]
    [SerializeField]
    private float skillPerDead = 5f;
    public float SkillPerDead => skillPerDead;

    [SerializeField]
    private int expPerDead = 20;
    public float ExpPerDead => expPerDead;
    [SerializeField]
    private int scorePerDead = 20;
    public float ScorePerDead => scorePerDead;

    private SpriteRenderer sprite;

    protected Color originalColor = Color.white;
    public Color OriginalColor => originalColor;
    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponent<SpriteRenderer>();
        SaveOriginalColor();
    }
    public override void Init()
    {
        base.Init();

        sprite.color = OriginalColor;
    }

    public void SaveOriginalColor()
    {
        originalColor = sprite.color;
    }

    public override void Hit(EntityType victim, float damage, bool isSkill = false)
    {
        base.Hit(victim, damage);
        if (!gameObject.activeInHierarchy || curHP <= 0) // 사망 시
            return;

        StopCoroutine("HitBlink");
        StartCoroutine("HitBlink");

        if (victim == EntityType.Player && !isSkill) // 플레이어에게 맞았고 스킬이 아니었을 경우
        {
            GameManager.Skill += (damage * damageScope / GameManager.CurWeapon.skillGauge) * 100f * skillEff; // 딜량/요구 딜량 * 100 * 대미지 효율
        }
    }
    private IEnumerator HitBlink()
    {
        if (gameObject.activeInHierarchy) // 살아있을 경우
        {
            sprite.color = Color.red;

            yield return YieldReturn.WaitForSeconds(0.1f);

            sprite.color = OriginalColor;
        }
    }

    protected override void DeadBy(EntityType killer)
    {
        if (killer == EntityType.Player) // 플레이어에게 죽었을 경우
        {
            GameManager.Skill += skillPerDead;
            GameManager.EXP += expPerDead;
            GameManager.Score += scorePerDead;

            if (Random.Range(0f, 1f) < itemDropProbability) // 아이템 드랍 확률에 당첨되었을 경우
            {
                if (Random.Range(0, 3) <= 1) // 2/3 확률로 아이템이 나옴 
                {
                    PlayerController.Inst.SpawnItem(transform.position);
                }
                else // 1/3 확률로 무기가 나옴
                {
                    PlayerController.Inst.SpawnWeapon(transform.position);
                }
            }
        }
        DeadParticle.ParticleLoad(transform.position, sprite.sprite, transform.parent.transform.eulerAngles.z);
    }
    protected override void HalfHP()
    {
        
    }
}
