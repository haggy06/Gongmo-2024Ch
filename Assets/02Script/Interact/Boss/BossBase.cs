using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[RequireComponent(typeof(HorizontalRepeatMove))]
public class BossBase : EnemyBase
{
    [Header("Boss Setting")]
    [SerializeField]
    private string bossName;
    public string BossName => bossName;

    [Space(10)]
    [SerializeField]
    private Vector3 initialPosition;
    [SerializeField]
    private float comebackSpeed;
    [SerializeField]
    private UnityEvent arrivePattern;

    protected HorizontalRepeatMove repeatMove;

    protected override void Awake()
    {
        base.Awake();

        repeatMove = GetComponent<HorizontalRepeatMove>();
    }
    protected override void ObjectReturned()
    {
        base.ObjectReturned();
        GameManager.Inst.BossDisappear();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        StartCoroutine("MoveToInitialPosition");
    }
    private IEnumerator MoveToInitialPosition()
    {
        repeatMove.enabled = false;

        while (MyCalculator.Distance(initialPosition, transform.position) > 0.1f) // 원래 위치와의 오차가 0.5 이하가 될 때까지 반복 
        {
            transform.position += (initialPosition - transform.position).normalized * Time.deltaTime * comebackSpeed;

            yield return null;
        }

        repeatMove.enabled = true;

        if (arrivePattern != null)
        {
            arrivePattern.Invoke();
        }
    }
}
