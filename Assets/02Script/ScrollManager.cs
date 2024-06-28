using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] stageBackground_Main = new Sprite[3];
    [SerializeField]
    private Sprite[] stageBackground_Fade = new Sprite[2];

    [Space(5)]
    [SerializeField]
    private Sprite[] stageForeground_Main = new Sprite[3];
    [SerializeField]
    private Sprite[] stageForeground_Fade = new Sprite[2];

    private SpriteRenderer topBackground;
    public SpriteRenderer TopBackground
    {
        set
        {
            topBackground = value;
            topBackground.sprite = stageBackground_Main[GameManager.Stage - 1];
        }
    }

    private SpriteRenderer topForeground;
    public SpriteRenderer TopForeground
    {
        set
        {
            topForeground = value;
            topForeground.sprite = stageForeground_Main[GameManager.Stage - 1];
        }
    }

    public event Action<bool> ScrollSwitchEvent = (_) => { };
    public void ScrollSwitch(bool isOn)
    {
        ScrollSwitchEvent(isOn);
    }
}
