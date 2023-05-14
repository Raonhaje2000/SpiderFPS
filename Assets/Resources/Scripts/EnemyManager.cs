using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // �̱��� ����
    public static EnemyManager instance = null;

    public int enemyMaxSpawnCount = 10;     // �ִ� ���� ���� ��
    public int enemyCurrentSpawnCount = 0;  // ���� ���� ���� ��

    public float chaseDelay = 2.0f;         // �߰� ������ (�߰� ��� �ð�)
    public float chaseLimitDistance = 1.5f; // �߰� ���� �Ÿ� (�÷��̾���� ���� ����)

    public float enemyMaxHp = 5.0f;         // ������ �ִ� ü��

    public float enemyDamage = 5.0f;        // ���Ͱ� �ִ� ������
    public float enemyAttackDelay = 1.0f;   // ���� ���� ������

    public int killPoint = 100;             // ���� óġ ����Ʈ

    public Transform player;                // �÷��̾�

    // �̱��� ����
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // �÷��̾� �ε�
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ���� ���� ���� �� 0���� �ʱ�ȭ
        enemyCurrentSpawnCount = 0;

        // �߰� ���� ���� �ʱ�ȭ
        chaseDelay = 2.0f;
        chaseLimitDistance = 1.5f;

        // ������ �ִ� ü�� �ʱ�ȭ
        enemyMaxHp = 5.0f;
    }

    // �÷��̾ ���͸� ����
    public void KillEnemy()
    {
        // ���� ���� ���� �� ����
        enemyCurrentSpawnCount--;

        // ���� ������ ǥ�� UI ����
        UIManager.instance.ChangeEnemyCountBar(enemyCurrentSpawnCount, enemyMaxSpawnCount);

        // ���� ����
        GameManager.instance.ChangeScore(killPoint);
    }
}