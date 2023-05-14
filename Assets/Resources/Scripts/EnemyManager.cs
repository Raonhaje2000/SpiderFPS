using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 싱글턴 패턴
    public static EnemyManager instance = null;

    public int enemyMaxSpawnCount = 10;     // 최대 몬스터 생성 수
    public int enemyCurrentSpawnCount = 0;  // 현재 몬스터 생성 수

    public float chaseDelay = 2.0f;         // 추격 딜레이 (추격 대기 시간)
    public float chaseLimitDistance = 1.5f; // 추격 제한 거리 (플레이어와의 여유 공간)

    public float enemyMaxHp = 5.0f;         // 몬스터의 최대 체력

    public float enemyDamage = 5.0f;        // 몬스터가 주는 데미지
    public float enemyAttackDelay = 1.0f;   // 몬스터 공격 딜레이

    public int killPoint = 100;             // 몬스터 처치 포인트

    public Transform player;                // 플레이어

    // 싱글턴 패턴
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // 플레이어 로드
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 현재 몬스터 생성 수 0으로 초기화
        enemyCurrentSpawnCount = 0;

        // 추격 관련 정보 초기화
        chaseDelay = 2.0f;
        chaseLimitDistance = 1.5f;

        // 몬스터의 최대 체력 초기화
        enemyMaxHp = 5.0f;
    }

    // 플레이어가 몬스터를 죽임
    public void KillEnemy()
    {
        // 현재 몬스터 생성 수 감소
        enemyCurrentSpawnCount--;

        // 몬스터 마릿수 표기 UI 변경
        UIManager.instance.ChangeEnemyCountBar(enemyCurrentSpawnCount, enemyMaxSpawnCount);

        // 점수 변경
        GameManager.instance.ChangeScore(killPoint);
    }
}