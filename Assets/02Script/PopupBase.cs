using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PopupBase : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 0.5f;
    public float FadeDuration => fadeDuration;

    private CanvasGroup popup;
    public CanvasGroup Popup
    {
        get
        {
            if (popup == null)
            {
                popup = GetComponent<CanvasGroup>();
            }

            return popup;
        }
    }
    private void OnDestroy()
    {
        Debug.Log(name + " ÆÄ±«µÊ.");
    }

    public void PopupHide()
    {
        Popup.alpha = 0f;
        Popup.blocksRaycasts = false;
    }
    public void PopupShow()
    {
        Popup.alpha = 1f;
        Popup.blocksRaycasts = true;
    }

    public void PopupClose(bool showFirst = false)
    {
        if (showFirst)
        {
            PopupShow();
        }

        StopCoroutine("PopupOpenCor");

        Popup.blocksRaycasts = false;

        StartCoroutine("PopupCloseCor");
    }
    private IEnumerator PopupCloseCor()
    {
        while (Popup.alpha > 0f)
        {
            Popup.alpha -= Time.deltaTime / fadeDuration;

            yield return null;
        }
    }

    public void PopupOpen(bool hideFirst = false)
    {
        if (hideFirst)
        {
            PopupHide();
        } 

        StopCoroutine("PopupCloseCor");

        Popup.blocksRaycasts = true;

        StartCoroutine("PopupOpenCor");
    }
    private IEnumerator PopupOpenCor()
    {
        while (Popup.alpha < 1f)
        {
            Popup.alpha += Time.deltaTime / fadeDuration;

            yield return null;
        }
    }
}
