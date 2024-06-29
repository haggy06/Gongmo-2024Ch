using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Scroll : MonoBehaviour
{
    private readonly Vector2 topPosition = new Vector2(0, 48f);

    [SerializeField, Tooltip("Background면 true, Foreground면 false")]
    private bool background;
    private float resetTime;
    [SerializeField]
    private float scrollSpeed = 6f;

    public bool scrolling = false;

    [SerializeField]
    private float time = 0f;

    private ScrollManager scrollManager;
    private void Awake()
    {
        resetTime = 60f / scrollSpeed;
        time = (60f - (transform.localPosition.y + 12f)) / scrollSpeed; //  -12부터 48까지 움직인다.

        scrollManager = transform.root.GetComponent<ScrollManager>();
        GameManager.BossEvent += SwitchScroll;
    }
    private void SwitchScroll(bool isOn)
    {
        scrolling = isOn;
    }
    private void FixedUpdate()
    {
        if (scrolling)
        {
            transform.Translate(Vector2.down * scrollSpeed * Time.fixedDeltaTime);

            time += Time.fixedDeltaTime;
            if (time >= resetTime)
            {
                transform.localPosition = topPosition;

                time = 0;

                if (background)
                {
                    scrollManager.TopBackground = GetComponent<SpriteRenderer>();
                }
                else
                {
                    scrollManager.TopForeground = GetComponent<SpriteRenderer>();
                }
            }
        }
    }
}
