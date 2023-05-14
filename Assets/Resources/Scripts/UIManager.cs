using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public ProgressBar playerHpBar;     // �÷��̾� ü�� ǥ�� UI

    public ProgressBar enemyHpBar;      // ���� ü�� ǥ�� UI
    public ProgressBar enemyCountBar;   // ���� ������ ǥ�� UI

    public Image shootingAimImage;

    public Text weaponBulletText;       // �Ѿ� �ؽ�Ʈ UI
    public Text weaponDelayText;        // ���� ���� �Ұ� �ؽ�Ʈ UI

    public Text reloadWarningText;      // ������ ��� �޼��� �ؽ�Ʈ UI

    public Text scoreText;              // ���� �ؽ�Ʈ UI

    public Image playerDamageEffect;    // �÷��̾� �ǰ� ����Ʈ �̹���
    public Image playerHpWarningEffect; // �÷��̾� ü�� ��� ����Ʈ �̹���

    public Text gameOverText;           // Game Over �ؽ�Ʈ UI
    public Button restartButton;        // ����� ��ư UI

    private void Awake()
    {
        instance = this;

        // ������Ʈ �ε�
        LoadComponent();
    }

    void Start()
    {
        // �ʱ�ȭ
        playerHpBar.BarValue = 100.0f;
        enemyHpBar.BarValue = 100.0f;

        weaponBulletText.text = "10 / 10";
        weaponDelayText.gameObject.SetActive(false);

        scoreText.text = "00000";

        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        reloadWarningText.gameObject.SetActive(false);
    }

    // ������Ʈ �ε�
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

    // �÷��̾� ü�� ǥ�� UI ����
    public void ChangePlayerHpBar(float playerCurrentHp, float playerMaxHp)
    {
        playerHpBar.BarValue = playerCurrentHp / playerMaxHp * 100.0f;
    }

    // ���� ü�� ǥ�� UI ����
    public void ChangeEnemyHpBar(float enemyHp)
    {
        enemyHpBar.BarValue = enemyHp / EnemyManager.instance.enemyMaxHp * 100.0f;
    }

    // ���� ü�� ǥ�� UI ��ġ ����
    public void ChangeEnemyHpBarPosition(Transform enemy)
    {
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(enemy.position + new Vector3(0, 0.7f, 0));
    }

    // ���� ������ ǥ�� UI ����
    public void ChangeEnemyCountBar(int enemyCurrentSpawnCount, int enemyMaxSpawnCount)
    {
        enemyCountBar.BarValue = (float) enemyCurrentSpawnCount / (float) enemyMaxSpawnCount * 100.0f;
    }

    // �Ѿ� �ؽ�Ʈ UI ����
    public void ChangeWeaponBulletText(int bulletCurrentCount, int bulletMaxCount)
    {
        weaponBulletText.text = bulletCurrentCount + " / " + bulletMaxCount;
    }

    // ���� ���� �Ұ� �ؽ�Ʈ UI Ȱ��ȭ ����
    public void ActiveWeaponDelayText(bool active)
    {
        weaponDelayText.gameObject.SetActive(active);
    }

    // ������ ��� �޼��� �ؽ�Ʈ UI Ȱ��ȭ ����
    public void ActiveReloadWarningText(bool active)
    {
        reloadWarningText.gameObject.SetActive(active);
    }

    // ���� �ؽ�Ʈ UI ����
    public void ChangeScoreText(int score)
    {
        // ���� 5�ڸ� ǥ�� (100000 �Ѿ�� 99999�� ǥ����)
        if(score < 100000)
        {
            scoreText.text = string.Format("{0:D5}", score);
        }
        else
        {
            scoreText.text = "99999";
        }
    }

    // ���� ���� UI �����
    public void HideUI()
    {
        playerDamageEffect.gameObject.SetActive(false);
        playerHpWarningEffect.gameObject.SetActive(false);

        shootingAimImage.gameObject.SetActive(false);
    }

    // Game Over ���� UI ����
    public void ActiveGameOverUI()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    // ����� ��ư Ŭ�� �� ���� ó��
    public void ClickRestartButton()
    {
        // ���� �����
        GameManager.instance.Restart();
    }
}
