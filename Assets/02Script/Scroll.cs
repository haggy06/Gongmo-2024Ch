using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Scroll : MonoBehaviour
{
    private readonly Vector2 topPosition = new Vector2(0, 24f);

    [SerializeField, Tooltip("Background�� true, Foreground�� false")]
    private bool background;
    private double resetTime;
    [SerializeField]
    private float scrollSpeed = 6f;

    public bool scrolling = false;

    [SerializeField]
    private double time = 0f; // ���� == ȭ�� ��׷����̹Ƿ� ������ ���� double ���

    private ScrollManager scrollManager;
    private void Awake()
    {
        resetTime = 36 / scrollSpeed;
        time = (36 - (transform.localPosition.y + 12)) / scrollSpeed;

        scrollManager = transform.root.GetComponent<ScrollManager>();
        scrollManager.ScrollSwitchEvent += (isOn) => scrolling = isOn;
    }
    private void FixedUpdate()
    {
        if (scrolling)
        {
            transform.Translate(Vector2.down * scrollSpeed * Time.fixedDeltaTime);

            time += Time.deltaTime;
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
