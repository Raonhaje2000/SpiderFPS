using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    Transform target;         // �߰��ϰ����ϴ� Ÿ�� (=�÷��̾�)

    Vector3 spiderPosition;   // ������ ��ġ
    Vector3 targetPosition;   // Ÿ���� ��ġ

    NavMeshAgent navMeshAgent;

    bool isChaseDelay = true; // �߰� ���������� Ȯ���ϴ� �÷���

    void Start()
    {
        // ���� ���� �ε� �� �ʱ�ȭ
        target = EnemyManager.instance.player;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    // �÷��̾� �߰�
    public void ChaseTarget()
    {
        // �߰� �������� ���
        if (isChaseDelay)
        {
            // �߰� ������ ����
            StartCoroutine(WaitChaseDelay());
        }
        else
        {
            // Ÿ���� ��ġ ����
            spiderPosition = this.transform.position;
            targetPosition = target.position;

            // ������ ��ġ�� Ÿ���� ��ġ�� �ٸ� ���
            if (spiderPosition != targetPosition)
            {
                // �߰� Ȱ��ȭ
                navMeshAgent.enabled = true;

                // Ÿ���� ��ġ�� �߰� ��� ����
                navMeshAgent.SetDestination(targetPosition);
            }
            else
            {
                // �߰� ��Ȱ��ȭ
                navMeshAgent.enabled = false;
            }
        }
    }

    // �߰� ������ ����
    IEnumerator WaitChaseDelay()
    {
        if (isChaseDelay)
        {
            yield return new WaitForSeconds(EnemyManager.instance.chaseDelay);

            isChaseDelay = false;
        }
    }

    // �÷��̾� �߰� ����
    public void ChaseStop()
    {
        navMeshAgent.enabled = false;
    }
}