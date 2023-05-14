using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public float playerMoveSpeed = 5.0f; // �÷��̾��� ������ �ӵ�
    public float playerJumpSpeed = 5.0f; // �÷��̾��� ���� �ӵ�

    float gravity = 9.8f;   // �߷�
    float yVelocity = 0.0f; // y�� �ӵ�

    public Transform cameraPosition; // ī�޶� ��ġ

    CharacterController cc; // ĳ���� ��Ʈ�ѷ�

    Vector3 moveVector = Vector3.zero; // �÷��̾��� �̵� ���� * �̵� �ӵ� (Move�� ����)

    void Start()
    {
        cc = this.GetComponent<CharacterController>(); // ĳ���� ��Ʈ�ѷ� ��������
    }

    void Update()
    {
        PlayerMove();   // �÷��̾� �̵�
        PlayerRotate(); // �÷��̾� ȸ��
    }

    // ī�޶� ������ �ִ� ������Ʈ �Լ�
    private void LateUpdate()
    {
        // ī�޶� ��ġ�� Ư����ġ�� �����ϰ� ����
        Camera.main.transform.position = cameraPosition.position;
    }

    // �÷��̾� �̵�
    void PlayerMove()
    {
        // Ű���� Horizontal, Vertical �Է��� ���� �޾ƿ�
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �÷��̾��� �̵� ���� ����
        moveVector.x = x;
        moveVector.y = 0;
        moveVector.z = z;

        // ������ �̵����͸� �۷ι��� ��ȯ
        moveVector = transform.TransformDirection(moveVector);

        // �÷��̾��� �̵� �ӵ� ����
        moveVector *= playerMoveSpeed;

        // �÷��̾� ����
        PlayerJump();

        // �÷��̾� �̵� ����
        cc.Move(moveVector * Time.deltaTime);
    }

    //�÷��̾� ����
    void PlayerJump()
    {
        // �ٴڰ� �浹 ���¿��� Jump ��ư�� ���� ���
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            // y�� �ӵ��� �÷��̾��� ���� �ӵ��� ����
            yVelocity = playerJumpSpeed;
        }

        // y�� �������� �߷� ���� (�ٴڿ� ��� ������ �Ʒ��� ����������)
        yVelocity -= (gravity * Time.deltaTime);
        moveVector.y = yVelocity;
    }

    // �÷��̾� ȸ��
    void PlayerRotate()
    {
        // �÷��̾� ȸ���� ī�޶� ȸ���� �����ϵ��� ����
        transform.rotation = Camera.main.transform.rotation;
    }
}