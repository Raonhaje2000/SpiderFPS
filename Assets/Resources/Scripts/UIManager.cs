using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public ProgressBar playerHpBar;     // 플레이어 체력 표기 UI

    public ProgressBar enemyHpBar;      // 몬스터 체력 표기 UI
    public ProgressBar enemyCountBar;   // 몬스터 마릿수 표기 UI

    public Image shootingAimImage;

    public Text weaponBulletText;       // 총알 텍스트 UI
    public Text weaponDelayText;        // 무기 변경 불가 텍스트 UI

    public Text reloadWarningText;      // 재장전 경고 메세지 텍스트 UI

    public Text scoreText;              // 점수 텍스트 UI

    public Image playerDamageEffect;    // 플레이어 피격 이펙트 이미지
    public Image playerHpWarningEffect; // 플레이어 체력 경고 이펙트 이미지

    public Text gameOverText;           // Game Over 텍스트 UI
    public Button restartButton;        // 재시작 버튼 UI

    private void Awake()
    {
        instance = this;

        // 컴포넌트 로드
        LoadComponent();
    }

    void Start()
    {
        // 초기화
        playerHpBar.BarValue = 100.0f;
        enemyHpBar.BarValue = 100.0f;

        weaponBulletText.text = "10 / 10";
        weaponDelayText.gameObject.SetActive(false);

        scoreText.text = "00000";

        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        reloadWarningText.gameObject.SetActive(false);
    }

    // 컴포넌트 로드
    private void LoadComponent()
    {
        playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<ProgressBar>();

        enemyHpBar = GameObject.Find("EnemyHpBar").GetComponent<ProgressBar>();
        enemyCountBar = GameObject.Find("SpiderCountBar").GetComponent<ProgressBar>();

        shootingAimImage = GameObject.Find("ShootingAimImage").GetComponent<Image>();

        weaponBulletText = GameObject.Find("BulletText").GetComponent<Text>();
        weaponDelayText = GameObject.Find("WeaponDelayText").GetComponent<Text>();

        reloadWarningText = GameObject.Find("ReloadWarningText").GetComponent<Text>();

        scoreText = GameObject.Find("ScoreValueText").GetComponent<Text>();

        playerDamageEffect = GameObject.Find("DamageEffect").GetComponent<Image>();
        playerHpWarningEffect = GameObject.Find("HpWarningEffect").GetComponent<Image>();

        gameOverText = GameObject.Find("GameOver").transform.Find("GameOverText").GetComponent<Text>();
        restartButton = GameObject.Find("GameOver").transform.Find("RestartButton").GetComponent<Button>();
    }

    // 플레이어 체력 표기 UI 변경
    public void ChangePlayerHpBar(float playerCurrentHp, float playerMaxHp)
    {
        playerHpBar.BarValue = playerCurrentHp / playerMaxHp * 100.0f;
    }

    // 몬스터 체력 표기 UI 변경
    public void ChangeEnemyHpBar(float enemyHp)
    {
        enemyHpBar.BarValue = enemyHp / EnemyManager.instance.enemyMaxHp * 100.0f;
    }

    // 몬스터 체력 표기 UI 위치 변경
    public void ChangeEnemyHpBarPosition(Transform enemy)
    {
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(enemy.position + new Vector3(0, 0.7f, 0));
    }

    // 몬스터 마릿수 표기 UI 변경
    public void ChangeEnemyCountBar(int enemyCurrentSpawnCount, int enemyMaxSpawnCount)
    {
        enemyCountBar.BarValue = (float) enemyCurrentSpawnCount / (float) enemyMaxSpawnCount * 100.0f;
    }

    // 총알 텍스트 UI 변경
    public void ChangeWeaponBulletText(int bulletCurrentCount, int bulletMaxCount)
    {
        weaponBulletText.text = bulletCurrentCount + " / " + bulletMaxCount;
    }

    // 무기 변경 불가 텍스트 UI 활성화 설정
    public void ActiveWeaponDelayText(bool active)
    {
        weaponDelayText.gameObject.SetActive(active);
    }

    // 재장전 경고 메세지 텍스트 UI 활성화 설정
    public void ActiveReloadWarningText(bool active)
    {
        reloadWarningText.gameObject.SetActive(active);
    }

    // 점수 텍스트 UI 변경
    public void ChangeScoreText(int score)
    {
        // 점수 5자리 표기 (100000 넘어가면 99999로 표기함)
        if(score < 100000)
        {
            scoreText.text = string.Format("{0:D5}", score);
        }
        else
        {
            scoreText.text = "99999";
        }
    }

    // 관련 없는 UI 숨기기
    public void HideUI()
    {
        playerDamageEffect.gameObject.SetActive(false);
        playerHpWarningEffect.gameObject.SetActive(false);

        shootingAimImage.gameObject.SetActive(false);
    }

    // Game Over 관련 UI 띄우기
    public void ActiveGameOverUI()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    // 재시작 버튼 클릭 시 동작 처리
    public void ClickRestartButton()
    {
        // 게임 재시작
        GameManager.instance.Restart();
    }
}
