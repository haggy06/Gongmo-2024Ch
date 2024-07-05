using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageBlink : MonoBehaviour
{
    [SerializeField]
    private float blinkSpeed = 1f;
    [SerializeField]
    private Color color1 = Color.white;
    public Color Color1 => color1;

    [SerializeField]
    private Color color2 = Color.yellow;
    public Color Color2 => color2;

    private Image image;
    public Image Img
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

    public void BlinkStart()
    {
        StartCoroutine("BlinkCor");
    }

    /// <summary>
    /// color1로 끝나면 true, color2로 끝나면 false
    /// </summary>
    public void BlinkStop(bool endColor = true)
    {
        StopCoroutine("BlinkCor");

        Img.color = endColor ? color1 : color2;
    }
    [Space(15), SerializeField]
    private Color color;
    private IEnumerator BlinkCor()
    {
        float time = 0;
        color = color1; // 시작 컬러는 color1로 고정
        while (true) // 깜빡이는 중이거나 종료 컬러에 도달하지 않은 동안 반복
        {
            Img.color = color;

            time += Time.deltaTime * Mathf.PI * blinkSpeed;

            color = MyCalculator.SinWave(time, color1, color2); // 깜박임을 위한 특제 공식.

            yield return null;
        }
    }
}

// 공식이 궁금한 사람들을 위해 :
// https://www.desmos.com/calculator?lang=ko
// y\ =\ \cos\left(3.14159265x\right)\cdot\left(\frac{\left(a-b\right)}{2}\right)+\left(\frac{\left(a+b\right)}{2}\right)