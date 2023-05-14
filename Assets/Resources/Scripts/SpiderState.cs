using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderState : MonoBehaviour
{
    Transform spider;
    Transform player;

    Rigidbody rbody;

    Animator animator;

    bool isChaseDelay = true;            // �߰� ���������� Ȯ���ϴ� �÷���

    float chaseDelay;                    // �߰� ������ (�߰� ��� �ð�)

    bool isDamageAnimationDelay = false; // �ǰ� �ִϸ��̼� ������ �÷���

    float attackDistance;                // ���� ���� �Ÿ� (�߰� ���� �Ÿ��� ����)

    bool isAttackDelay;                  // ���� ���� ���������� üũ�ϴ� �÷���

    public float enemyMaxHp;             // ������ �ִ� ü��
    public float enemyCurrentHp;         // ������ ���� ü��

    bool isDead = false;                 // ���Ͱ� �׾����� Ȯ���ϴ� �÷���

    public enum State
    {
        IDLE = 0, // �⺻
        WALK,     // �ȱ� (�÷��̾� �߰�)
        ATTACK,   // ����
        DAMAGE,   // �ǰ�
        DEAD      // ����
    };

    public State enemyState = State.IDLE; // ���� ����

    void Start()
    {
        // ���� ���� �ε� �� �ʱ�ȭ
        spider = this.transform;
        player = EnemyManager.instance.player;

        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        isChaseDelay = true;

        isDamageAnimationDelay = false;

        chaseDelay = EnemyManager.instance.chaseDelay;
        attackDistance = EnemyManager.instance.chaseLimitDistance;

        enemyMaxHp = EnemyManager.instance.enemyMaxHp;
        enemyCurrentHp = enemyMaxHp;

        isDead = false;

        // �ִϸ����� �Ű����� �ʱ�ȭ
        animator.SetBool("isWalk", false);
        animator.SetBool("isDamaged", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isDead", false);
    }

    void Update()
    {
        switch (enemyState)
        {
            case State.IDLE:
                {
                    // �߰� �����̰� �ƴ� �� WALK�� ���� ����
                    if (isChaseDelay)
                    {
                        // �߰� ������ ����
                        StartCoroutine(WaitChaseDelay());
                    }
                    else
                    {
                        // x, y, z�� ��ġ ����
                        rbody.constraints = RigidbodyConstraints.FreezeAll;

                        animator.SetBool("isWalk", true);
                        enemyState = State.WALK;
                    }
                }
                break;
            case State.WALK:
                {
                    // �÷��̾� �߰�
                    this.GetComponent<SpiderAI>().ChaseTarget();

                    // ���Ϳ� �÷��̾� ������ �Ÿ��� ������ ���� �Ÿ��� ��� ATTACK���� ���� ����
                    if (Vector3.Distance(spider.position, player.position) <= attackDistance)
                    {
                        animator.SetBool("isWalk", false);
                        animator.SetBool("isAttack", true);
                        enemyState = State.ATTACK;
                    }
                }
                break;
            case State.ATTACK:
                {
                    // �÷��̾� �߰� ����
                    this.GetComponent<SpiderAI>().ChaseStop();

                    // ���Ϳ� �÷��̾� ������ �Ÿ��� ������ ���� �Ÿ��� ��� ��� IDLE�� ���� ����
                    if (Vector3.Distance(spider.position, player.position) > attackDistance)
                    {
                        animator.SetBool("isAttack", false);
                        enemyState = State.IDLE;
                    }
                }
                break;
            case State.DAMAGE:
                {
                    // �÷��̾� �߰� ����
                    this.GetComponent<SpiderAI>().ChaseStop();

                    // ���� �ǰ� �ִϸ��̼� ����
                    StartCoroutine(WaitDamageAnimationDelay());
                }
                break;
            case State.DEAD:
                {
                    // ���Ͱ� ���� �� �׾��� ��� (�ѹ��� ó���ǵ���)
                    if (!isDead)
                    {
                        //Debug.Log("���� óġ");

                        // �÷��̾� �߰� ����
                        this.GetComponent<SpiderAI>().ChaseStop();

                        isDead = true;

                        // �÷��̾ ���͸� ����, ���� ���� ���� �� ����
                        EnemyManager.instance.KillEnemy();

                        // �ִϸ��̼��� ���� �� ���� ����
                        Destroy(spider.gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.8f);
                    }
                }
                break;
        }
    }

    // �߰� ������ ����
    IEnumerator WaitChaseDelay()
    {
        if (isChaseDelay)
        {
            yield return new WaitForSeconds(chaseDelay);

            isChaseDelay = false;
        }
    }

    // �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        // ���� ���°� ATTACK�̰�, �浹�� ������Ʈ�� �÷��̾��� ���
        if(enemyState == State.ATTACK && other.gameObject.tag == "Player")
        {
            // ���� ���� �����̰� �ƴ� ���
            if (!isAttackDelay)
            {
                isAttackDelay = true;

                //Debug.Log("�÷��̾� ����");

                // �÷��̾�� ���ظ� ����
                other.GetComponent<PlayerState>().TakeDamagePlayer(EnemyManager.instance.enemyDamage);

                // ���� ���� ������ ����
                StartCoroutine(WaitAttackDelay());
            }
            
        }
    }

    // ���� ���� ������ ���� (�ߺ� ó�� ����)
    IEnumerator WaitAttackDelay()
    {
        yield return new WaitForSeconds(EnemyManager.instance.enemyAttackDelay);

        isAttackDelay = false;
    }

    // ���Ϳ��� ���ظ� ����
    public void TakeDamageSpider(float bulletDamage)
    {
        // ���Ͱ� ���� �� �׾��� ��� DAMAGE�� ���� ����
        if (!isDead)
        {
            // ������ ���� ü�� ����
            enemyCurrentHp -= bulletDamage;

            //Debug.Log("���� ü�� : " + enemyCurrentHp);

            animator.SetBool("isDamaged", true);
            enemyState = State.DAMAGE;
        }
    }

    // ���� �ǰ� �ִϸ��̼� ����
    IEnumerator WaitDamageAnimationDelay()
    {
        // �ǰ� �ִϸ��̼� �����̰� �ƴ� ���
        if (!isDamageAnimationDelay)
        {
            isDamageAnimationDelay = true;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // ������ ü���� ���� ��� DEAD�� ���� ����, ü���� �����ִ� ��� IDLE�� ���� ����
            if (enemyCurrentHp <= 0)
            {
                animator.SetBool("isDead", true);

                enemyState = State.DEAD;
            }
            else
            {
                animator.SetBool("isDamaged", false);
                enemyState = State.IDLE;
            }

            isDamageAnimationDelay = false;
        }
    }
}