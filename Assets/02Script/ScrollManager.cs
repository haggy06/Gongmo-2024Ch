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
            topBackground.sprite = ResourceLoader.SpriteLoad(FolderName.Background, "Background", GameManager.Stage);
        }
    }

    [Space(5)]
    [SerializeField]
    private SpriteRenderer topForeground;
    public SpriteRenderer TopForeground
    {
        set
        {
            topForeground = value;
            topForeground.sprite = ResourceLoader.SpriteLoad(FolderName.Background, "Foreground", GameManager.Stage);
        }
    }

    private void Start()
    {
        GameManager.StageChangeEvent += StageFade;
    }

    private void StageFade()
    {
        if (GameManager.Stage > 1) // 2스테이지 이상일 경우
        {
            topBackground.sprite = ResourceLoader.SpriteLoad(FolderName.Background, "FadeBG", GameManager.Stage);
            topForeground.sprite = ResourceLoader.SpriteLoad(FolderName.Background, "FadeFG", GameManager.Stage);
        }
    }
}
