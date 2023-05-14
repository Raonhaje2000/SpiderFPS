using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float playerMaxHp;                    // �÷��̾��� �ִ� ü��
    public float playerCurrentHp;                // �÷��̾��� ���� ü��

    public float HpWarningCriterion;             // �÷��̾� ü�� ��� ���� (%)

    float playerStepSoundDelay = 0.55f;          // �÷��̾��� �߰��� ȿ���� ������
    bool isPlayerStepSoundDelay = false;         // �÷��̾��� �߰��� ȿ���� ������ �÷���

    public enum State
    {
        IDLE = 0, // �⺻
        WALK,     // �ȱ�
        ATTACK,   // ����
        DAMAGE,   // �ǰ�
        DEAD      // ����
    };

    public State playerState = State.IDLE; // �÷��̾� ����

    void Start()
    {
        // �ʱ�ȭ
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
                    // ����Ű �Է��� ���� �� WALK�� ���� ����
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                    {
                        playerState = State.WALK;
                    }

                    // ���콺 ��Ŭ�� �Է��� ���� �� ATTACK���� ���� ����
                    if (Input.GetMouseButton(0))
                    {
                        playerState = State.ATTACK;
                    }
                }
                break;
            case State.WALK:
                {
                    // ���콺 ��Ŭ�� �Է��� �ִ� ��� ATTACK���� ���� ����
                    if (Input.GetMouseButton(0))
                    {
                        playerState = State.ATTACK;
                    }

                    if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !Input.GetButtonDown("Jump"))
                    {
                        // ����Ű �Է��� �ְ�, ����Ű�� ������ ���� ��� �߰��� ȿ���� ���
                        StartCoroutine(PlayStepSound());
                    }
                    else
                    {
                        // �ƹ��� �Է��� ���� ��� IDLE�� ���� ����
                        playerState = State.IDLE;
                    }
                }
                break;
            case State.ATTACK:
                {
                    // ���콺 ��Ŭ�� �Է��� ���� ��� IDLE�� ���� ����
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
                        // �÷��̾��� ���� ü���� 0 ������ ��� DEAD�� ���� ����
                        playerState = State.DEAD;
                    }
                    else
                    {
                        // �÷��̾� ü���� ü�� ��� ���� ������ ���
                        if (playerCurrentHp <= playerMaxHp * HpWarningCriterion)
                        {
                            // ü�� ��� ����Ʈ Ȱ��ȭ
                            GetComponent<ScreenEffect>().SetActiveHpWarning(true);
                        }

                        playerState = State.IDLE;
                    }
                }
                break;
            case State.DEAD:
                {
                    //Debug.Log("Game Over");

                    // Game Over ó��
                    GameManager.instance.GameOver();
                }
                break;
        }
    }

    // �÷��̾�� ���ظ� ����
    public void TakeDamagePlayer(float damage)
    {
        // ���� ��������ŭ �÷��̾� ü�� ����
        playerCurrentHp -= damage;

        // �÷��̾� �ǰ� ����Ʈ �����ֱ�
        GetComponent<ScreenEffect>().ShowPlayerDamage();

        // �÷��̾� �ǰ� ȿ���� ���
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.playerDamageSound, 0, SoundManager.instance.sfxVolum - 0.35f);

        // �÷��̾� ü�� ǥ�� UI ����
        UIManager.instance.ChangePlayerHpBar(playerCurrentHp, playerMaxHp);

        // DAMAGE�� ���� ����
        playerState = State.DAMAGE;
    }

    // �߰��� ȿ���� ���
    IEnumerator PlayStepSound()
    {
        // �÷��̾��� �߰��� ȿ���� �����̰� �ƴ� ���
        if (!isPlayerStepSoundDelay)
        {
            isPlayerStepSoundDelay = true;

            // �߰��� ȿ������ �� �ϳ� ���ؼ� ���� ���
            AudioClip[] randomAudio = SoundManager.instance.stepSound;
            int index = Random.Range(0, randomAudio.Length);

            SoundManager.instance.PlaySfx(transform.position, randomAudio[index], 0, SoundManager.instance.sfxVolum);

            yield return new WaitForSeconds(playerStepSoundDelay);

            isPlayerStepSoundDelay = false;
        }
    }
}
