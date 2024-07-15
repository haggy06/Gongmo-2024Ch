using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : StraightMoveEnemy
{
    [SerializeField]
    private Color[] coralColorArray;

    [Header("Electricity Attack")]
    [SerializeField]
    private ParticleSystem electricityReady;
    [SerializeField]
    private float electricityTerm;
    [SerializeField]
    private ExplosionObject electricity;

    [Header("Tentacles")]
    [SerializeField]
    private Transform tentaclePosition;
    [SerializeField]
    private Vector2 positionOffset;
    [SerializeField]
    private float angleOffset;

    [Space(5)]
    [SerializeField]
    private PoolObject tentacle;
    [SerializeField]
    private int tentacleNumber;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        GetComponentInChildren<SpriteRenderer>().color = coralColorArray[Random.Range(0, coralColorArray.Length)];
        enemyInteract.SaveOriginalColor();
    }

    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        electricityReady.Play();
        Invoke("ElectroAttack", electricityTerm);
    }
    private void ElectroAttack()
    {
        if (gameObject.activeInHierarchy) // 아직 해파리가 살아있을 경우
        {
            parentPool.GetPoolObject(electricity).Init(transform.position, 0f);
        }
    }

    protected override void Dead(EntityType killer)
    {
        base.Dead(killer);

        for (int i = 0; i < tentacleNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(tentacle);
            proj.Init(tentaclePosition.position + new Vector3(Random.Range(positionOffset.x, positionOffset.x), Random.Range(positionOffset.y, positionOffset.y), 0f), 90f + Random.Range(-angleOffset, angleOffset));
            proj.GetComponent<SpriteRenderer>().color = enemyInteract.originalColor;
        }
    }
}
