using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionLock : MonoBehaviour
{
    /*
    private void FixedUpdate()
    {
        //FixResolution(cam, 16, 9);
        //SetResolution(16, 9);
    }
    */
    /*
    private static void FixResolution(Camera targetCam, int x, int y)
    {
        Resolution resolution = Screen.currentResolution;
        print(Screen.currentResolution);

        if (resolution.width * x < resolution.height * y)
        {
            Screen.SetResolution(resolution.width, (int)(resolution.height * (y * x)), true);
        }
        else
        {
            Screen.SetResolution((int)(resolution.width * (x * y)), resolution.height, true);
        }

        print(Screen.currentResolution);
    }
    */
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

        //Screen.SetResolution(setWidth, (int)((deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

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
