using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWithBackground : MoveBase
{
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
            transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime;
        }
    }
}
