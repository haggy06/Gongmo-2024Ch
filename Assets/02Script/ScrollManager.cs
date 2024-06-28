using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer topBackground;
    public SpriteRenderer TopBackground
    {
        set
        {
            topBackground = value;
            topBackground.sprite = stageBackground_Main[GameManager.Stage - 1];
        }
    }
    [SerializeField]
    private Sprite[] stageBackground_Main = new Sprite[3];
    [SerializeField]
    private Sprite[] stageBackground_Fade = new Sprite[2];

    [Space(5)]
    [SerializeField]
    private SpriteRenderer topForeground;
    public SpriteRenderer TopForeground
    {
        set
        {
            topForeground = value;
            topForeground.sprite = stageForeground_Main[GameManager.Stage - 1];
        }
    }
    [SerializeField]
    private Sprite[] stageForeground_Main = new Sprite[3];
    [SerializeField]
    private Sprite[] stageForeground_Fade = new Sprite[2];

    private void Awake()
    {
        GameManager.StageChangeEvent += StageFade;
    }
    public void OnDestroy()
    {
        GameManager.StageChangeEvent -= StageFade;
    }

    private void StageFade()
    {
        if (GameManager.Stage > 1) // 2스테이지 이상일 경우
        {
            topBackground.sprite = stageBackground_Fade[GameManager.Stage - 2];
            topForeground.sprite = stageForeground_Fade[GameManager.Stage - 2];
        }
    }
}
