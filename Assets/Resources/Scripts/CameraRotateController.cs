using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateController : MonoBehaviour
{
    public float sensitivity = 400;         // 회전 감도
    public float rotationLimitY = 55.0f;    // y축 회전 각도 제한

    float rotationX = 0; // x축 회전량                   
    float rotationY = 0; // y축 회전량

    void Start()
    {
        // 게임 뷰 중앙에 커서 고정 후 숨기기 (테스트가 편의성을 위해)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 카메라 회전 (화면 회전)
        CameraRotate();
    }

    // 카메라 회전 (화면 회전)
    void CameraRotate()
    {
        // 마우스 x축, y축 움직임 각각 감지
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // x축, y축 회전량 각각 계산
        rotationX += x * sensitivity * Time.deltaTime;
        rotationY += y * sensitivity * Time.deltaTime;

        // y축 회전 각도 제한
        rotationY = Mathf.Clamp(rotationY, -rotationLimitY, rotationLimitY);

        // 회전량을 카메라에 반영 (z축 고정)
        transform.rotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
    }
}