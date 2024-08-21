using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionLock : MonoBehaviour
{
    private static float cWidth;
    private static float cHeight;
    public static bool CheckResolution()
    {
        return (Mathf.Approximately(cWidth, Screen.width) && Mathf.Approximately(cHeight, Screen.height));
    }
    public static void SetResolution(float width, float height)
    {
        cWidth = Screen.width; // ��� �ʺ� ����
        cHeight = Screen.height; // ��� ���� ����

        if (width / height < cWidth / cHeight) // ���� �ػ󵵰� ���η� �� ū ���
        {
            float newWidth = (width / height) / (cWidth / cHeight); // �ʺ� ���̿� ���� ��������
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else if (width / height > cWidth / cHeight) // ���� �ػ󵵰� ���η� �� ū ���
        {
            float newHeight = (cWidth / cHeight) / (width / height); // ���̸� �ʺ� ���� ��������
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
        else
        {
            print("���� ��Ȯ�� ����");
        }
    }
}
