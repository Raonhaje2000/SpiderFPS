using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    Transform target;         // 추격하고자하는 타겟 (=플레이어)

    Vector3 spiderPosition;   // 몬스터의 위치
    Vector3 targetPosition;   // 타겟의 위치

    NavMeshAgent navMeshAgent;

    bool isChaseDelay = true; // 추격 딜레이인지 확인하는 플래그

    void Start()
    {
        // 관련 정보 로드 및 초기화
        target = EnemyManager.instance.player;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    // 플레이어 추격
    public void ChaseTarget()
    {
        // 추격 딜레이인 경우
        if (isChaseDelay)
        {
            // 추격 딜레이 적용
            StartCoroutine(WaitChaseDelay());
        }
        else
        {
            // 타겟의 위치 갱신
            spiderPosition = this.transform.position;
            targetPosition = target.position;

            // 몬스터의 위치와 타겟의 위치가 다른 경우
            if (spiderPosition != targetPosition)
            {
                // 추격 활성화
                navMeshAgent.enabled = true;

                // 타겟의 위치로 추격 경로 지정
                navMeshAgent.SetDestination(targetPosition);
            }
            else
            {
                // 추격 비활성화
                navMeshAgent.enabled = false;
            }
        }
    }

    // 추격 딜레이 적용
    IEnumerator WaitChaseDelay()
    {
        if (isChaseDelay)
        {
            yield return new WaitForSeconds(EnemyManager.instance.chaseDelay);

            isChaseDelay = false;
        }
    }

    // 플레이어 추격 중지
    public void ChaseStop()
    {
        navMeshAgent.enabled = false;
    }
}