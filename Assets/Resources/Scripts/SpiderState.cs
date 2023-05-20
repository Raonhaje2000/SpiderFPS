using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderState : MonoBehaviour
{
    Transform spider;
    Transform player;

    Rigidbody rbody;

    Animator animator;

    bool isChaseDelay = true;            // 추격 딜레이인지 확인하는 플래그

    float chaseDelay;                    // 추격 딜레이 (추격 대기 시간)

    bool isDamageAnimationDelay = false; // 피격 애니메이션 딜레이 플래그

    float attackDistance;                // 몬스터 공격 거리 (추격 제한 거리와 동일)

    bool isAttackDelay;                  // 몬스터 공격 딜레이인지 체크하는 플래그

    public float enemyMaxHp;             // 몬스터의 최대 체력
    public float enemyCurrentHp;         // 몬스터의 현재 체력

    bool isDead = false;                 // 몬스터가 죽었는지 확인하는 플래그

    public enum State
    {
        IDLE = 0, // 기본
        WALK,     // 걷기 (플레이어 추격)
        ATTACK,   // 공격
        DAMAGE,   // 피격
        DEAD      // 죽음
    };

    public State enemyState = State.IDLE; // 몬스터 상태

    void Start()
    {
        // 관련 정보 로드 및 초기화
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

        // 애니메이터 매개변수 초기화
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
                    // 추격 딜레이가 아닐 때 WALK로 상태 전이
                    if (isChaseDelay)
                    {
                        // 추격 딜레이 적용
                        StartCoroutine(WaitChaseDelay());
                    }
                    else
                    {
                        // x, y, z축 위치와 회전 고정
                        rbody.constraints = RigidbodyConstraints.FreezeAll;

                        animator.SetBool("isWalk", true);
                        enemyState = State.WALK;
                    }
                }
                break;
            case State.WALK:
                {
                    // 플레이어 추격
                    this.GetComponent<SpiderAI>().ChaseTarget();

                    // 몬스터와 플레이어 사이의 거리가 몬스터의 공격 거리인 경우 ATTACK으로 상태 전이
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
                    // 플레이어 추격 중지
                    this.GetComponent<SpiderAI>().ChaseStop();

                    // 몬스터와 플레이어 사이의 거리가 몬스터의 공격 거리를 벗어난 경우 IDLE로 상태 전이
                    if (Vector3.Distance(spider.position, player.position) > attackDistance)
                    {
                        animator.SetBool("isAttack", false);
                        enemyState = State.IDLE;
                    }
                }
                break;
            case State.DAMAGE:
                {
                    // 플레이어 추격 중지
                    this.GetComponent<SpiderAI>().ChaseStop();

                    // 몬스터 피격 애니메이션 조절
                    StartCoroutine(WaitDamageAnimationDelay());
                }
                break;
            case State.DEAD:
                {
                    // 몬스터가 아직 안 죽었을 경우 (한번만 처리되도록)
                    if (!isDead)
                    {
                        //Debug.Log("몬스터 처치");

                        // 플레이어 추격 중지
                        this.GetComponent<SpiderAI>().ChaseStop();

                        isDead = true;

                        // 플레이어가 몬스터를 죽임, 현재 몬스터 생성 수 감소
                        EnemyManager.instance.KillEnemy();

                        // 애니메이션이 끝난 후 몬스터 제거
                        Destroy(spider.gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.8f);
                    }
                }
                break;
        }
    }

    // 추격 딜레이 적용
    IEnumerator WaitChaseDelay()
    {
        if (isChaseDelay)
        {
            yield return new WaitForSeconds(chaseDelay);

            isChaseDelay = false;
        }
    }

    // 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        // 몬스터 상태가 ATTACK이고, 충돌한 오브젝트가 플레이어인 경우
        if(enemyState == State.ATTACK && other.gameObject.tag == "Player")
        {
            // 몬스터 공격 딜레이가 아닌 경우
            if (!isAttackDelay)
            {
                isAttackDelay = true;

                //Debug.Log("플레이어 공격");

                // 플레이어에게 피해를 입힘
                other.GetComponent<PlayerState>().TakeDamagePlayer(EnemyManager.instance.enemyDamage);

                // 몬스터 공격 딜레이 적용
                StartCoroutine(WaitAttackDelay());
            }            
        }
    }

    // 몬스터 공격 딜레이 적용 (중복 처리 방지)
    IEnumerator WaitAttackDelay()
    {
        yield return new WaitForSeconds(EnemyManager.instance.enemyAttackDelay);

        isAttackDelay = false;
    }

    // 몬스터에게 피해를 입힘
    public void TakeDamageSpider(float bulletDamage)
    {
        // 몬스터가 아직 안 죽었을 경우 DAMAGE로 상태 전이
        if (!isDead)
        {
            // 몬스터의 현재 체력 감소
            enemyCurrentHp -= bulletDamage;

            //Debug.Log("몬스터 체력 : " + enemyCurrentHp);

            animator.SetBool("isDamaged", true);
            enemyState = State.DAMAGE;
        }
    }

    // 몬스터 피격 애니메이션 조절
    IEnumerator WaitDamageAnimationDelay()
    {
        // 피격 애니메이션 딜레이가 아닌 경우
        if (!isDamageAnimationDelay)
        {
            isDamageAnimationDelay = true;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // 몬스터의 체력이 없는 경우 DEAD로 상태 전이, 체력이 남아있는 경우 IDLE로 상태 전이
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
