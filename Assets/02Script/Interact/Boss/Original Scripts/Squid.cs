using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : BossBase
{
    [Header("Tentacle Attack")]
    [SerializeField]
    private SpriteRenderer leftTentacle;
    [SerializeField]
    private SpriteRenderer rightTentacle;

    [Space(5)]
    [SerializeField]
    private Sprite t1;
    [SerializeField]
    private Sprite t2;
    [SerializeField]
    private Sprite t3;

    [Space(10)]
    [SerializeField]
    private PoolObject tentacleWarning;

    [Space(5)]
    [SerializeField]
    private Vector2 leftSpawnRange1;
    [SerializeField]
    private Vector2 leftSpawnRange2;
    [Space(3)]
    [SerializeField]
    private Vector2 rightSpawnRange1;
    [SerializeField]
    private Vector2 rightSpawnRange2;

    [Space(10)]
    [SerializeField]
    private float tentacleTerm = 0.75f;
    private bool tentacleON = false;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        tentacleON = false;
        enemyInteract.damageResistance = 0f;

        leftTentacle.sprite = rightTentacle.sprite = t1;
    }
    /*
    protected override void HalfHP()
    {
        StopCoroutine("PatternCor");
        anim.SetInteger(EntityAnimHash.Pattern, 4);

        StartCoroutine("TentacleAnimation");
    }
    */
    public IEnumerator TentacleAnimation()
    {
        yield return YieldReturn.WaitForSeconds(0.3f);
        leftTentacle.sprite = rightTentacle.sprite = t2;

        yield return YieldReturn.WaitForSeconds(0.7f);
        leftTentacle.sprite = rightTentacle.sprite = t3;

        tentacleON = true;
        StartCoroutine("TentacleCor");

        StabilizePattern();
    }

    /* ¿ÀÂ¡¾î ÆÐÅÏ
     * 1. ¸Ô¹° »êÅº
     * 2. ¸Ô¹° ±â°üÃÑ
     * 3. Å« ¸Ô¹° µ¢¾î¸®
     * 4. ÃË¼ö ¿¬¹ß Âî¸£±â(ÆäÀÌÁî 1)
     */
    /*
    protected override void Pattern(int caseNumber)
    {
        anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
    }

    public void SpreadInk()
    {
        parentPool.GetPoolObject(spreadInkProj).Init(projectilePosition.position, -90f);

        StabilizePattern();
    }
    public void SpitInk()
    {
        parentPool.GetPoolObject(fastInkProj).Init(projectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - projectilePosition.position));

        StabilizePattern();
    }
    public void LargeInk()
    {
        parentPool.GetPoolObject(largeInkProj).Init(projectilePosition.position, 0f);

        StabilizePattern();
    }
    */
    private IEnumerator TentacleCor()
    {
        while (tentacleON)
        {
            print("ÁÂÃø ½ºÆù"); parentPool.GetPoolObject(tentacleWarning).Init(new Vector2(Random.Range(leftSpawnRange1.x, leftSpawnRange2.x), Random.Range(leftSpawnRange1.y, leftSpawnRange2.y)), 0f);

            yield return YieldReturn.WaitForSeconds(tentacleTerm);

            print("¿ìÃø ½ºÆù"); parentPool.GetPoolObject(tentacleWarning).Init(new Vector2(Random.Range(rightSpawnRange1.x, rightSpawnRange2.x), Random.Range(rightSpawnRange1.y, rightSpawnRange2.y)), 0f);

            yield return YieldReturn.WaitForSeconds(tentacleTerm);
        }
    }
}
