using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWithBackground : MoveBase
{
    [SerializeField]
    private float scrollSpeed = 6f;

    public bool scrolling = true;

    private void Start()
    {
        GameManager.BossEvent += (isOn) => scrolling = !isOn;
        GameManager.GameEndEvent += (gameStatus) => scrolling = (gameStatus < GameStatus.GameOver);
    }
    private void FixedUpdate()
    {
        if (scrolling)
        {
            transform.Translate(Vector2.down * actualSpeed * Time.fixedDeltaTime);
        }
    }
}
