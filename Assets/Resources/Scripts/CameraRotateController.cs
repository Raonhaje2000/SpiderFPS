using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateController : MonoBehaviour
{
    public float sensitivity = 400;         // ȸ�� ����
    public float rotationLimitY = 55.0f;    // y�� ȸ�� ���� ����

    float rotationX = 0; // x�� ȸ����                   
    float rotationY = 0; // y�� ȸ����

    void Start()
    {
        // ���� �� �߾ӿ� Ŀ�� ���� �� ����� (�׽�Ʈ�� ���Ǽ��� ����)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ī�޶� ȸ�� (ȭ�� ȸ��)
        CameraRotate();
    }

    // ī�޶� ȸ�� (ȭ�� ȸ��)
    void CameraRotate()
    {
        // ���콺 x��, y�� ������ ���� ����
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // x��, y�� ȸ���� ���� ���
        rotationX += x * sensitivity * Time.deltaTime;
        rotationY += y * sensitivity * Time.deltaTime;

        // y�� ȸ�� ���� ����
        rotationY = Mathf.Clamp(rotationY, -rotationLimitY, rotationLimitY);

        // ȸ������ ī�޶� �ݿ� (z�� ����)
        transform.rotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
    }
}