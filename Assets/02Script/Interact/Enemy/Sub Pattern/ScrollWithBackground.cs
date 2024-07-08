using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWithBackground : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 6f;

    public bool scrolling = true;

    private void Awake()
    {
        GameManager.BossEvent += (isOn) => scrolling = !isOn;
    }
    private void FixedUpdate()
    {
        if (scrolling)
        {
            transform.Translate(Vector2.down * scrollSpeed * Time.fixedDeltaTime);
        }
    }
}
