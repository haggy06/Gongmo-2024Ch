using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Scroll : MonoBehaviour
{
    private readonly Vector2 topPosition = new Vector2(0, 60f);

    [SerializeField, Tooltip("Background�� true, Foreground�� false")]
    private bool background;
    private float resetTime;
    [SerializeField]
    private float scrollSpeed = 6f;

    public bool scrolling = true;

    [SerializeField]
    private float time = 0f;

    private ScrollManager scrollManager;
    private void Awake()
    {
        resetTime = (topPosition.y + 12f) / scrollSpeed;
        time = ((topPosition.y + 12f) - (transform.localPosition.y + 12f)) / scrollSpeed; //  -12���� 60���� �����δ�.

        scrollManager = transform.root.GetComponent<ScrollManager>();
        GameManager.BossEvent += (isOn) => scrolling = !isOn;
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
