using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    [SerializeField]
    private float blinkSpeed = 1f;

    [Space(5)]
    [SerializeField]
    private Color color1 = Color.white;
    [SerializeField]
    private Color color2 = Color.yellow;

    public Color Color1 => color1;
    public Color Color2 => color2;

    private Image image;
    private SpriteRenderer sRenderer;

    public Image Image
    {
        get
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            return image;
        }
    }
    public SpriteRenderer SRenderer
    {
        get
        {
            if (sRenderer == null)
            {
                sRenderer = GetComponent<SpriteRenderer>();
            }

            return sRenderer;
        }
    }

    public void BlinkStart()
    {
        StartCoroutine("BlinkCor");
    }

    /// <summary>
    /// color1로 끝나면 true, color2로 끝나면 false
    /// </summary>
    public void BlinkStop(bool endColor)
    {
        StopCoroutine("BlinkCor");

        if (Image)
            Image.color = endColor ? color1 : color2;
        else if (SRenderer)
            SRenderer.color = endColor ? color1 : color2;
        else
            Debug.LogError(name + "에 Image도 SpriteRenderer도 없음");
    }
    private Color color;
    private IEnumerator BlinkCor()
    {
        float time = 0;
        color = color1; // 시작 컬러는 color1로 고정
        while (true) // 깜빡이는 중이거나 종료 컬러에 도달하지 않은 동안 반복
        {
            if (Image)
                Image.color = color;
            else if (SRenderer)
                SRenderer.color = color;
            else
            {
                Debug.LogError(name + "에 Image도 SpriteRenderer도 없음");
                break;
            }

            time += Time.deltaTime;

            color = MyCalculator.CosWave(time * blinkSpeed, color1, color2); // 깜박임을 위한 특제 공식.

            yield return null;
        }
    }
}

// 공식이 궁금한 사람들을 위해 :
// https://www.desmos.com/calculator?lang=ko
// y\ =\ \cos\left(3.14159265x\right)\cdot\left(\frac{\left(a-b\right)}{2}\right)+\left(\frac{\left(a+b\right)}{2}\right)