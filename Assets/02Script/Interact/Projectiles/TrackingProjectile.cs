using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : Projectile
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private EntityType targetEntity;

    [Space(5)]
    [SerializeField]
    private float searchRadius = 5f;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        target = null;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (target) // 타겟이 지정되어 있을 경우
        {
            transform.eulerAngles = Vector3.forward * MyCalculator.Vec2Deg((target.position - transform.position).normalized);
        }
        else // 타겟이 지정되어 있지 않을 경우
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, searchRadius, 1 << (int)LAYER.Censor);
            foreach (var v in cols)
            {
                if (v.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)targetEntity))
                {
                    target = hitBase.transform;

                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.4f, 1f, 0.25f, 0.1f);
        Gizmos.DrawSphere(transform.position, searchRadius);
    }
}
