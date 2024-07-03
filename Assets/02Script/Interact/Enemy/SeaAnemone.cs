using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaAnemone : EnemyBase
{
    [SerializeField]
    private float scrollSpeed = 4f;

    protected override void Awake()
    {
        base.Awake();
        GameManager.BossEvent += (isOn) => rigid2D.velocity = isOn ? Vector2.zero : Vector2.down * scrollSpeed; // 보스 등장 시엔 스크롤이 멈추므로 말미잘도 멈추게 함
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        rigid2D.velocity = Vector2.down * scrollSpeed;
    }

    protected override void Dead(AttackBase attack)
    {
        
    }

    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }

    protected override void Pattern(int caseNumber)
    {
        switch (caseNumber)
        {
            case 0:

                break;
        }
    }

}
