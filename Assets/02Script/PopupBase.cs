using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class PopupBase : MonoBehaviour
{
    [SerializeField]
    private Button firstButton;
    private Button lastButton = null; // �� �˾� ���� ���� �ִ� �˾��� ���� ��� �ӽ÷� ������ �� �ʵ�

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
        Debug.Log(name + " �ı���.");
    }

    public void PopupHide()
    {
        if (lastButton)
        {
            lastButton.Select();
        }

        Popup.alpha = 0f;
        Popup.blocksRaycasts = false;
        Popup.interactable = false;
    }
    public void PopupShow()
    {
        if (firstButton)
        {
            if (EventSystem.current.currentSelectedGameObject)
                lastButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>(); // ���� ��ư �ӽ� ����

            firstButton.Select();
        }

        Popup.alpha = 1f;
        Popup.blocksRaycasts = true;
        Popup.interactable = true;
    }

    public void PopupClose(bool showFirst = false)
    {
        if (lastButton)
        {
            lastButton.Select();
        }

        if (showFirst)
        {
            PopupShow();
        }

        StopCoroutine("PopupOpenCor");

        Popup.blocksRaycasts = false;
        Popup.interactable = false;

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
        if (firstButton)
        {
            if (EventSystem.current.currentSelectedGameObject)
                lastButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>(); // ���� ��ư �ӽ� ����

            firstButton.Select();
            print(firstButton.name + " ��ư ����");
        }

        if (hideFirst)
        {
            PopupHide();
        } 

        StopCoroutine("PopupCloseCor");

        Popup.blocksRaycasts = true;
        Popup.interactable = true;

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
