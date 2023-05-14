using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarSpawn : MonoBehaviour
{
    Transform raySpawnPoint;    // 광선이 생성되는 위치

    Ray ray;                    // 광선
    RaycastHit raycastHit;      // 광선과 충돌한 오브젝트에 대한 정보

    float rayDistance = 15.0f;  // 광선 거리(길이)

    void Start()
    {
        // 새로운 광선 생성
        ray = new Ray();
    }

    void Update()
    {
        // 광선 갱신
        UpdateRay();
    }

    private void LateUpdate()
    {
        // 광선과 충돌한 오브젝트에 대한 정보 갱신
        ObjectRayHit();
    }

    // 광선 갱신
    void UpdateRay()
    {
        // 광선이 생성되는 위치 가져오기
        raySpawnPoint = GameObject.Find("RayPoint").transform;

        // 광선의 원점과 방향 설정
        ray.origin = raySpawnPoint.position;
        ray.direction = raySpawnPoint.forward;

        // Scene View에 광선 표시
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
    }

    // 광선과 충돌한 오브젝트에 대한 정보 갱신
    void ObjectRayHit()
    {
        // 광선과 충돌한 오브젝트가 있는 경우
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, rayDistance))
        {
            if (raycastHit.collider.gameObject.tag == "Enemy") // 충돌한 오브젝트의 태그가 Enemy인 경우
            {
                // 해당 몬스터의 체력만큼 몬스터 체력 표기 UI 변경
                UIManager.instance.ChangeEnemyHpBar(raycastHit.collider.gameObject.GetComponent<SpiderState>().enemyCurrentHp);

                // 몬스터 체력 표기 UI를 해당 오브젝트 위치로 이동
                UIManager.instance.ChangeEnemyHpBarPosition(raycastHit.collider.gameObject.transform);

                // 몬스터 체력 표기 UI 활성화
                UIManager.instance.enemyHpBar.gameObject.SetActive(true);
            }
            else // 충돌한 오브젝트의 태그가 Enemy가 아닌 경우
            {
                // 몬스터 체력 표기 UI 비활성화
                UIManager.instance.enemyHpBar.gameObject.SetActive(false);
            }
        }
    }
}
