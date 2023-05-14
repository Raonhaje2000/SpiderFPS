using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float playerMaxHp;                    // 플레이어의 최대 체력
    public float playerCurrentHp;                // 플레이어의 현재 체력

    public float HpWarningCriterion;             // 플레이어 체력 경고 기준 (%)

    float playerStepSoundDelay = 0.55f;          // 플레이어의 발걸음 효과음 딜레이
    bool isPlayerStepSoundDelay = false;         // 플레이어의 발걸음 효과음 딜레이 플래그

    public enum State
    {
        IDLE = 0, // 기본
        WALK,     // 걷기
        ATTACK,   // 공격
        DAMAGE,   // 피격
        DEAD      // 죽음
    };

    public State playerState = State.IDLE; // 플레이어 상태

    void Start()
    {
        // 초기화
        playerMaxHp = 100.0f;
        playerCurrentHp = playerMaxHp;

        HpWarningCriterion = 0.3f;

        playerStepSoundDelay = 0.55f;
        isPlayerStepSoundDelay = false;
    }

    void Update()
    {
        switch (playerState)
        {
            case State.IDLE:
                {
                    // 방향키 입력이 있을 때 WALK로 상태 전이
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                    {
                        playerState = State.WALK;
                    }

                    // 마우스 우클릭 입력이 있을 때 ATTACK으로 상태 전이
                    if (Input.GetMouseButton(0))
                    {
                        playerState = State.ATTACK;
                    }
                }
                break;
            case State.WALK:
                {
                    // 마우스 우클릭 입력이 있는 경우 ATTACK으로 상태 전이
                    if (Input.GetMouseButton(0))
                    {
                        playerState = State.ATTACK;
                    }

                    if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !Input.GetButtonDown("Jump"))
                    {
                        // 방향키 입력이 있고, 점프키를 누르지 않은 경우 발걸음 효과음 재생
                        StartCoroutine(PlayStepSound());
                    }
                    else
                    {
                        // 아무런 입력이 없을 경우 IDLE로 상태 전이
                        playerState = State.IDLE;
                    }
                }
                break;
            case State.ATTACK:
                {
                    // 마우스 우클릭 입력이 없는 경우 IDLE로 상태 전이
                    if (!Input.GetMouseButton(0))
                    {
                        playerState = State.IDLE;
                    }
                }
                break;
            case State.DAMAGE:
                {
                    if (playerCurrentHp <= 0)
                    {
                        // 플레이어의 현재 체력이 0 이하인 경우 DEAD로 상태 전이
                        playerState = State.DEAD;
                    }
                    else
                    {
                        // 플레이어 체력이 체력 경고 기준 이하인 경우
                        if (playerCurrentHp <= playerMaxHp * HpWarningCriterion)
                        {
                            // 체력 경고 이펙트 활성화
                            GetComponent<ScreenEffect>().SetActiveHpWarning(true);
                        }

                        playerState = State.IDLE;
                    }
                }
                break;
            case State.DEAD:
                {
                    //Debug.Log("Game Over");

                    // Game Over 처리
                    GameManager.instance.GameOver();
                }
                break;
        }
    }

    // 플레이어에게 피해를 입힘
    public void TakeDamagePlayer(float damage)
    {
        // 입은 데미지만큼 플레이어 체력 감소
        playerCurrentHp -= damage;

        // 플레이어 피격 이펙트 보여주기
        GetComponent<ScreenEffect>().ShowPlayerDamage();

        // 플레이어 피격 효과음 재생
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.playerDamageSound, 0, SoundManager.instance.sfxVolum - 0.35f);

        // 플레이어 체력 표기 UI 변경
        UIManager.instance.ChangePlayerHpBar(playerCurrentHp, playerMaxHp);

        // DAMAGE로 상태 전이
        playerState = State.DAMAGE;
    }

    // 발걸음 효과음 재생
    IEnumerator PlayStepSound()
    {
        // 플레이어의 발걸음 효과음 딜레이가 아닌 경우
        if (!isPlayerStepSoundDelay)
        {
            isPlayerStepSoundDelay = true;

            // 발걸음 효과음들 중 하나 택해서 랜덤 재생
            AudioClip[] randomAudio = SoundManager.instance.stepSound;
            int index = Random.Range(0, randomAudio.Length);

            SoundManager.instance.PlaySfx(transform.position, randomAudio[index], 0, SoundManager.instance.sfxVolum);

            yield return new WaitForSeconds(playerStepSoundDelay);

            isPlayerStepSoundDelay = false;
        }
    }
}
