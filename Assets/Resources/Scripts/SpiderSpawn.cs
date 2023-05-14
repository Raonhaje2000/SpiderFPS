using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawn : MonoBehaviour
{
    GameObject spider;              // ���� ������
    GameObject[] spiderSpawnPoint;  // ���� ���� ��ġ

    public float spawnDelay = 5.0f; // ���� ���� ������
    bool isSpawnDelay = false;      // ���� ���� ������ ���� Ȯ���ϴ� �÷���

    void Start()
    {
        // ���� ������ �ε�
        spider = Resources.Load<GameObject>("Prefabs/Spider");

        // ���� ���� ��ġ �ε�
        spiderSpawnPoint = GameObject.FindGameObjectsWithTag("SpiderSpawnPoint");
    }

    void Update()
    {
        // ���� ���� ���� ���� �ִ� ���� �� ���� ���� ���
        if (EnemyManager.instance.enemyCurrentSpawnCount < EnemyManager.instance.enemyMaxSpawnCount)
        {
            // ���� ����
            SpawnSpider();
        }
    }

    // ���� ����
    void SpawnSpider()
    {
        // ���� �����̰� �ƴ� ���
        if (!isSpawnDelay)
        {
            // ���� ���� ��ġ �߿� ������ ���� ���� ����
            int spawnIndex = Random.Range(0, spiderSpawnPoint.Length);
            Instantiate(spider, spiderSpawnPoint[spawnIndex].transform);

            // ���� ���� ���� �� ����
            EnemyManager.instance.enemyCurrentSpawnCount++;

            // ���� ������ ǥ�� UI ����
            UIManager.instance.ChangeEnemyCountBar(EnemyManager.instance.enemyCurrentSpawnCount, EnemyManager.instance.enemyMaxSpawnCount);

            // ���� ���� ������ ����
            StartCoroutine(WaitSpawnDealy());
        }
    }

    // ���� ���� ������ ����
    IEnumerator WaitSpawnDealy()
    {
        isSpawnDelay = true;

        yield return new WaitForSeconds(spawnDelay);

        isSpawnDelay = false;
    }
}