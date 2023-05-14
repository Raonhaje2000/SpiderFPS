using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �̱��� ����	
    public static GameManager instance;

    public int score;                   // ����

    public bool isGameOver = false;     // ���� �������� Ȯ���ϴ� �÷���

    // �̱��� ����	
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // �ʱ�ȭ	
        Time.timeScale = 1.0f;

        score = 0;

        isGameOver = false;

        // ������� ���
        PlayBgm();
    }

    // ���� ����
    public void ChangeScore(int point)
    {
        // ���� ����Ʈ��ŭ ���� ����
        score += point;

        // ���� �ؽ�Ʈ UI ����
        UIManager.instance.ChangeScoreText(score);
    }

    // ������� ���
    public void PlayBgm()
    {
        // ������� GameObject ����
        SoundManager.instance.PlayBGM(SoundManager.instance.mainBgm, 0, true);
    }

    // ������� ����
    public void StopBgm()
    {
        // ������� GameObject�� ã�Ƽ� ����
        GameObject bgm = GameObject.Find("BGM");

        Destroy(bgm);
    }

    // Game Over ó��
    public void GameOver()
    {
        // ���� ���� UI �̹��� �����
        UIManager.instance.HideUI();

        // Game Over ���� UI ����
        UIManager.instance.ActiveGameOverUI();

        // ������� ����
        StopBgm();

        // Ŀ�� ���� �� ����� ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ���� �������� Ȯ���ϴ� �÷��� ture�� ����
        isGameOver = true;

        // ���� �Ͻ� ����
        Time.timeScale = 0.0f;
    }

    // ���� �����
    public void Restart()
    {
        // Scene �ҷ����� (Main Scene �ҷ�����)	
        SceneManager.LoadScene(0);
    }
}
