using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // 사운드는 최소 0, 최대 1
    public float[] shootingVolum;           // 총소리 볼륨
    public float bgmVolum;                  // 배경음 볼륨
    public float sfxVolum;                  // 효과음 볼륨

    public bool isMute = false;             // 음소거 상태인지 체크

    AudioSource bgmSource;              
    public AudioClip mainBgm;               // 메인 배경음

    public AudioClip[] stepSound;           // 발걸음 효과음
        
    public AudioClip[] shootingSound;       // 총소리 효과음

    public AudioClip weaponChangeSound;     // 무기 변경 효과음
    public AudioClip weaponReloadSound;     // 재장전 효과음

    public AudioClip playerDamageSound;     // 플레이어 피격 효과음

    //public AudioClip playerHpWarningSound;  // 플레이어 체력 경고 효과음

    //public AudioClip enemyDamageSound;      // 몬스터 피격 효과음
    //public AudioClip enemyDeadSound;        // 몬스터 처치 효과음

    //public AudioClip gameOverBgm;           // Game Over 배경음

    private void Awake()
    {
        instance = this;

        // 배경음 및 효과음 로드
        LoadResources();
    }

    private void Start()
    {
        // 초기화
        shootingVolum = new float[3] {0.1f, 0.3f, 0.1f};
        bgmVolum = 1.0f;
        sfxVolum = 0.5f;

        isMute = false;
    }

    // 배경음 및 효과음 로드
    private void LoadResources()
    {
        mainBgm = Resources.Load<AudioClip>("Sound/Jungle Atmosphere Night");

        stepSound = Resources.LoadAll<AudioClip>("Sound/Step");

        shootingSound = Resources.LoadAll<AudioClip>("Sound/Shooting");

        weaponChangeSound = Resources.Load<AudioClip>("Sound/Handgun Cocking");
        weaponReloadSound = Resources.Load<AudioClip>("Sound/Gun reload");

        playerDamageSound = Resources.Load<AudioClip>("Sound/Hit with Club");
}

    // 효과음 재생(동적 생성)
    public void PlaySfx(Vector3 position, AudioClip sfx, float delay, float volum)
    {
        // 음소거 상태가 아닌 경우
        if (!isMute)
        {
            // 효과음 재생
            StartCoroutine(PlaySfxDelay(position, sfx, delay, volum));
        }
    }

    // 효과음 재생
    IEnumerator PlaySfxDelay(Vector3 position, AudioClip sfx, float delay, float volum)
    {
        yield return new WaitForSeconds(delay);

        // 효과음 Object 생성
        GameObject sfxObject = new GameObject("sfx");

        AudioSource audio = sfxObject.AddComponent<AudioSource>();

        // 효과음이 재생될 위치, 재생될 효과음, 효과음의 볼륨 조절 설정
        sfxObject.transform.position = position;
        audio.clip = sfx;
        audio.volume = volum;

        // 거리에 따른 소리 조절
        audio.minDistance = 5.0f;
        audio.maxDistance = 10.0f;

        // 효과음 재생
        audio.Play();

        // 효과음이 끝나면 제거
        Destroy(sfxObject, sfx.length);
    }

    // 배경음악 재생
    public void PlayBGM(AudioClip bgm, float delay, bool loop)
    {
        // 음소거 상태가 아닌 경우
        if (!isMute)
        {
            // 배경음악 재생
            StartCoroutine(PlayBgmDelay(bgm, delay, loop));
        }
    }

    // 배경음악 재생
    IEnumerator PlayBgmDelay(AudioClip bgm, float delay, bool loop)
    {
        yield return new WaitForSeconds(delay);

        // 배경음악 Object 생성
        GameObject bgmObject = new GameObject("BGM");

        if (!bgmSource)
        {
            bgmSource = bgmObject.AddComponent<AudioSource>();
        }

        // 재생될 배경음악, 배경음악의 볼륨 조절, 반복 설정
        bgmSource.clip = bgm;
        bgmSource.volume = bgmVolum;
        bgmSource.loop = loop;

        // 배경음악 재생
        bgmSource.Play();
    }
}
