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
        cWidth = Screen.width; // 기기 너비 저장
        cHeight = Screen.height; // 기기 높이 저장

        //Screen.SetResolution(setWidth, (int)((deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if (width / height < cWidth / cHeight) // 현재 해상도가 가로로 더 큰 경우
        {
            float newWidth = (width / height) / (cWidth / cHeight); // 너비를 높이에 맞춰 재조정함
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else if (width / height > cWidth / cHeight) // 현재 해상도가 세로로 더 큰 경우
        {
            float newHeight = (cWidth / cHeight) / (width / height); // 높이를 너비에 맞춰 재조정함
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
        else
        {
            print("비율 정확히 맞춤");
        }
    }
}
