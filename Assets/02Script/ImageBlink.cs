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
    /// color1�� ������ true, color2�� ������ false
    /// </summary>
    public void BlinkStop(bool endColor)
    {
        StopCoroutine("BlinkCor");

        if (Image)
            Image.color = endColor ? color1 : color2;
        else if (SRenderer)
            SRenderer.color = endColor ? color1 : color2;
        else
            Debug.LogError(name + "�� Image�� SpriteRenderer�� ����");
    }
    private Color color;
    private IEnumerator BlinkCor()
    {
        float time = 0;
        color = color1; // ���� �÷��� color1�� ����
        while (true) // �����̴� ���̰ų� ���� �÷��� �������� ���� ���� �ݺ�
        {
            if (Image)
                Image.color = color;
            else if (SRenderer)
                SRenderer.color = color;
            else
            {
                Debug.LogError(name + "�� Image�� SpriteRenderer�� ����");
                break;
            }

            time += Time.deltaTime;

            color = MyCalculator.CosWave(time * blinkSpeed, color1, color2); // �������� ���� Ư�� ����.

            yield return null;
        }
    }
}

// ������ �ñ��� ������� ���� :
// https://www.desmos.com/calculator?lang=ko
// y\ =\ \cos\left(3.14159265x\right)\cdot\left(\frac{\left(a-b\right)}{2}\right)+\left(\frac{\left(a+b\right)}{2}\right)