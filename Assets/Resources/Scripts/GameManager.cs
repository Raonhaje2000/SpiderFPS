using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글턴 패턴	
    public static GameManager instance;

    public int score;                   // 점수

    public bool isGameOver = false;     // 게임 오버인지 확인하는 플래그

    // 싱글턴 패턴	
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 초기화	
        Time.timeScale = 1.0f;

        score = 0;

        isGameOver = false;

        // 배경음악 재생
        PlayBgm();
    }

    // 점수 변경
    public void ChangeScore(int point)
    {
        // 얻은 포인트만큼 점수 증가
        score += point;

        // 점수 텍스트 UI 변경
        UIManager.instance.ChangeScoreText(score);
    }

    // 배경음악 재생
    public void PlayBgm()
    {
        // 배경음악 GameObject 생성
        SoundManager.instance.PlayBGM(SoundManager.instance.mainBgm, 0, true);
    }

    // 배경음악 정지
    public void StopBgm()
    {
        // 배경음악 GameObject를 찾아서 제거
        GameObject bgm = GameObject.Find("BGM");

        Destroy(bgm);
    }

    // Game Over 처리
    public void GameOver()
    {
        // 관련 없는 UI 이미지 숨기기
        UIManager.instance.HideUI();

        // Game Over 관련 UI 띄우기
        UIManager.instance.ActiveGameOverUI();

        // 배경음악 정지
        StopBgm();

        // 커서 고정 및 숨기기 해제
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 게임 오버인지 확인하는 플래그 ture로 변경
        isGameOver = true;

        // 게임 일시 정지
        Time.timeScale = 0.0f;
    }

    // 게임 재시작
    public void Restart()
    {
        // Scene 불러오기 (Main Scene 불러오기)	
        SceneManager.LoadScene(0);
    }
}
