using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyInteract : HitBase
{
    [Header("Enemy Info")]
    [SerializeField, Range(0f, 1f), Tooltip("���� ������� �� �ۼ�Ʈ�� ��ų�� ġȯ���� �� ���ϴ� �ʵ�")]
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
        if (!gameObject.activeInHierarchy || curHP <= 0) // ��� ��
            return;

        StopCoroutine("HitBlink");
        StartCoroutine("HitBlink");

        if (victim == EntityType.Player && !isSkill) // �÷��̾�� �¾Ұ� ��ų�� �ƴϾ��� ���
        {
            GameManager.Skill += (damage * damageScope / GameManager.CurWeapon.skillGauge) * 100f * skillEff; // ����/�䱸 ���� * 100 * ����� ȿ��
        }
    }
    private IEnumerator HitBlink()
    {
        if (gameObject.activeInHierarchy) // ������� ���
        {
            sprite.color = Color.red;

            yield return YieldReturn.WaitForSeconds(0.1f);

            sprite.color = OriginalColor;
        }
    }

    protected override void DeadBy(EntityType killer)
    {
        if (killer == EntityType.Player) // �÷��̾�� �׾��� ���
        {
            GameManager.Skill += skillPerDead;
            GameManager.EXP += expPerDead;
            GameManager.Score += scorePerDead;

            if (Random.Range(0f, 1f) < itemDropProbability) // ������ ��� Ȯ���� ��÷�Ǿ��� ���
            {
                if (Random.Range(0, 3) <= 1) // 2/3 Ȯ���� �������� ���� 
                {
                    PlayerController.Inst.SpawnItem(transform.position);
                }
                else // 1/3 Ȯ���� ���Ⱑ ����
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
