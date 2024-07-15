using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : BossBase
{
    [Header("Ink Spread")]
    [SerializeField]
    private Transform projectilePosition;
    [SerializeField]
    private PoolObject spreadInkProj;
    [SerializeField]
    private PoolObject fastInkProj;
    [SerializeField]
    private PoolObject largeInkProj;

    [Header("Tentacle Attack")]
    [SerializeField]
    private PoolObject tentacleWarning;
    [SerializeField]
    private int tentacleNumber = 5;
    [SerializeField]
    private float tentacleTerm = 0.75f;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        enemyInteract.damageResistance = 0f;
    }

    protected override void HalfHP()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 5);
    }

    protected override void MoribundHP()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 6);
    }

    /* ø¿¬°æÓ ∆–≈œ
     * 1. ∏‘π∞ ªÍ≈∫
     * 2. ∏‘π∞ ±‚∞¸√—
     * 3. ≈´ ∏‘π∞ µ¢æÓ∏Æ
     * 5. √Àºˆ ø¨πﬂ ¬Ó∏£±‚(∆‰¿Ã¡Ó 1)
     * 6. ∏‘π∞ ø¨∏∑(∆‰¿Ã¡Ó 2)
     */
    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
    }

    public void SpreadInk()
    {
        parentPool.GetPoolObject(spreadInkProj).Init(projectilePosition.position, -90f);
    }
    public void SpitInk()
    {
        parentPool.GetPoolObject(fastInkProj).Init(projectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Player.transform.position - projectilePosition.position));
    }
    public void LargeInk()
    {
        parentPool.GetPoolObject(largeInkProj).Init(projectilePosition.position, -90f);
    }

    public void TentacleAttack()
    {
        StartCoroutine("TentacleCor");
    }
    private IEnumerator TentacleCor()
    {
        StopCoroutine("PatternRepeat");
        enemyInteract.damageResistance = 0.5f;

        for (int i = 0; i < tentacleNumber; i++)
        {
            parentPool.GetPoolObject(fastInkProj).Init(projectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Player.transform.position - projectilePosition.position));

            yield return YieldReturn.WaitForSeconds(1f);
        }

        StartCoroutine("PatternRepeat");
        enemyInteract.damageResistance = 0f;
    }
}
