using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawn : MonoBehaviour
{
    GameObject spider;              // 몬스터 프리팹
    GameObject[] spiderSpawnPoint;  // 몬스터 생성 위치

    public float spawnDelay = 5.0f; // 몬스터 생성 딜레이
    bool isSpawnDelay = false;      // 몬스터 생성 딜레이 인지 확인하는 플래그

    void Start()
    {
        // 몬스터 프리팹 로드
        spider = Resources.Load<GameObject>("Prefabs/Spider");

        // 몬스터 생성 위치 로드
        spiderSpawnPoint = GameObject.FindGameObjectsWithTag("SpiderSpawnPoint");
    }

    void Update()
    {
        // 현재 몬스터 생성 수가 최대 생성 수 보다 적은 경우
        if (EnemyManager.instance.enemyCurrentSpawnCount < EnemyManager.instance.enemyMaxSpawnCount)
        {
            // 몬스터 생성
            SpawnSpider();
        }
    }

    // 몬스터 생성
    void SpawnSpider()
    {
        // 생성 딜레이가 아닌 경우
        if (!isSpawnDelay)
        {
            // 몬스터 생성 위치 중에 랜덤한 곳에 몬스터 생성
            int spawnIndex = Random.Range(0, spiderSpawnPoint.Length);
            Instantiate(spider, spiderSpawnPoint[spawnIndex].transform);

            // 현재 몬스터 생성 수 증가
            EnemyManager.instance.enemyCurrentSpawnCount++;

            // 몬스터 마릿수 표기 UI 변경
            UIManager.instance.ChangeEnemyCountBar(EnemyManager.instance.enemyCurrentSpawnCount, EnemyManager.instance.enemyMaxSpawnCount);

            // 몬스터 생성 딜레이 적용
            StartCoroutine(WaitSpawnDealy());
        }
    }

    // 몬스터 생성 딜레이 적용
    IEnumerator WaitSpawnDealy()
    {
        isSpawnDelay = true;

        yield return new WaitForSeconds(spawnDelay);

        isSpawnDelay = false;
    }
}