using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // ����� �ּ� 0, �ִ� 1
    public float[] shootingVolum;           // �ѼҸ� ����
    public float bgmVolum;                  // ����� ����
    public float sfxVolum;                  // ȿ���� ����

    public bool isMute = false;             // ���Ұ� �������� üũ

    AudioSource bgmSource;              
    public AudioClip mainBgm;               // ���� �����

    public AudioClip[] stepSound;           // �߰��� ȿ����
        
    public AudioClip[] shootingSound;       // �ѼҸ� ȿ����

    public AudioClip weaponChangeSound;     // ���� ���� ȿ����
    public AudioClip weaponReloadSound;     // ������ ȿ����

    public AudioClip playerDamageSound;     // �÷��̾� �ǰ� ȿ����

    //public AudioClip playerHpWarningSound;  // �÷��̾� ü�� ��� ȿ����

    //public AudioClip enemyDamageSound;      // ���� �ǰ� ȿ����
    //public AudioClip enemyDeadSound;        // ���� óġ ȿ����

    //public AudioClip gameOverBgm;           // Game Over �����

    private void Awake()
    {
        instance = this;

        // ����� �� ȿ���� �ε�
        LoadResources();
    }

    private void Start()
    {
        // �ʱ�ȭ
        shootingVolum = new float[3] {0.1f, 0.3f, 0.1f};
        bgmVolum = 1.0f;
        sfxVolum = 0.5f;

        isMute = false;
    }

    // ����� �� ȿ���� �ε�
    private void LoadResources()
    {
        mainBgm = Resources.Load<AudioClip>("Sound/Jungle Atmosphere Night");

        stepSound = Resources.LoadAll<AudioClip>("Sound/Step");

        shootingSound = Resources.LoadAll<AudioClip>("Sound/Shooting");

        weaponChangeSound = Resources.Load<AudioClip>("Sound/Handgun Cocking");
        weaponReloadSound = Resources.Load<AudioClip>("Sound/Gun reload");

        playerDamageSound = Resources.Load<AudioClip>("Sound/Hit with Club");
}

    // ȿ���� ���(���� ����)
    public void PlaySfx(Vector3 position, AudioClip sfx, float delay, float volum)
    {
        // ���Ұ� ���°� �ƴ� ���
        if (!isMute)
        {
            // ȿ���� ���
            StartCoroutine(PlaySfxDelay(position, sfx, delay, volum));
        }
    }

    // ȿ���� ���
    IEnumerator PlaySfxDelay(Vector3 position, AudioClip sfx, float delay, float volum)
    {
        yield return new WaitForSeconds(delay);

        // ȿ���� Object ����
        GameObject sfxObject = new GameObject("sfx");

        AudioSource audio = sfxObject.AddComponent<AudioSource>();

        // ȿ������ ����� ��ġ, ����� ȿ����, ȿ������ ���� ���� ����
        sfxObject.transform.position = position;
        audio.clip = sfx;
        audio.volume = volum;

        // �Ÿ��� ���� �Ҹ� ����
        audio.minDistance = 5.0f;
        audio.maxDistance = 10.0f;

        // ȿ���� ���
        audio.Play();

        // ȿ������ ������ ����
        Destroy(sfxObject, sfx.length);
    }

    // ������� ���
    public void PlayBGM(AudioClip bgm, float delay, bool loop)
    {
        // ���Ұ� ���°� �ƴ� ���
        if (!isMute)
        {
            // ������� ���
            StartCoroutine(PlayBgmDelay(bgm, delay, loop));
        }
    }

    // ������� ���
    IEnumerator PlayBgmDelay(AudioClip bgm, float delay, bool loop)
    {
        yield return new WaitForSeconds(delay);

        // ������� Object ����
        GameObject bgmObject = new GameObject("BGM");

        if (!bgmSource)
        {
            bgmSource = bgmObject.AddComponent<AudioSource>();
        }

        // ����� �������, ��������� ���� ����, �ݺ� ����
        bgmSource.clip = bgm;
        bgmSource.volume = bgmVolum;
        bgmSource.loop = loop;

        // ������� ���
        bgmSource.Play();
    }
}
