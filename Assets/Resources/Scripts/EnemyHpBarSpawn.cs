using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarSpawn : MonoBehaviour
{
    Transform raySpawnPoint;    // ������ �����Ǵ� ��ġ

    Ray ray;                    // ����
    RaycastHit raycastHit;      // ������ �浹�� ������Ʈ�� ���� ����

    float rayDistance = 15.0f;  // ���� �Ÿ�(����)

    void Start()
    {
        // ���ο� ���� ����
        ray = new Ray();
    }

    void Update()
    {
        // ���� ����
        UpdateRay();
    }

    private void LateUpdate()
    {
        // ������ �浹�� ������Ʈ�� ���� ���� ����
        ObjectRayHit();
    }

    // ���� ����
    void UpdateRay()
    {
        // ������ �����Ǵ� ��ġ ��������
        raySpawnPoint = GameObject.Find("RayPoint").transform;

        // ������ ������ ���� ����
        ray.origin = raySpawnPoint.position;
        ray.direction = raySpawnPoint.forward;

        // Scene View�� ���� ǥ��
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
    }

    // ������ �浹�� ������Ʈ�� ���� ���� ����
    void ObjectRayHit()
    {
        // ������ �浹�� ������Ʈ�� �ִ� ���
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, rayDistance))
        {
            if (raycastHit.collider.gameObject.tag == "Enemy") // �浹�� ������Ʈ�� �±װ� Enemy�� ���
            {
                // �ش� ������ ü�¸�ŭ ���� ü�� ǥ�� UI ����
                UIManager.instance.ChangeEnemyHpBar(raycastHit.collider.gameObject.GetComponent<SpiderState>().enemyCurrentHp);

                // ���� ü�� ǥ�� UI�� �ش� ������Ʈ ��ġ�� �̵�
                UIManager.instance.ChangeEnemyHpBarPosition(raycastHit.collider.gameObject.transform);

                // ���� ü�� ǥ�� UI Ȱ��ȭ
                UIManager.instance.enemyHpBar.gameObject.SetActive(true);
            }
            else // �浹�� ������Ʈ�� �±װ� Enemy�� �ƴ� ���
            {
                // ���� ü�� ǥ�� UI ��Ȱ��ȭ
                UIManager.instance.enemyHpBar.gameObject.SetActive(false);
            }
        }
    }
}
