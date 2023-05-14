using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour
{
    Image damageEffect;           // 피격 이펙트 이미지
    Image hpWarningEffect;        // 체력 경고 이펙트 이미지

    bool activeHpWarning = false; // 체력 경고 이펙트 활성화 플래그

    void Start()
    {
        // 초기화
        damageEffect = UIManager.instance.playerDamageEffect;
        hpWarningEffect = UIManager.instance.playerHpWarningEffect;

        activeHpWarning = false;
    }

    void Update()
    {
        // 체력 경고 이펙트가 활성화 된 경우
        if (activeHpWarning)
        {
            // 플레이어 체력 경고 이펙트 보여주기
            ShowHpWarning();
        }
    }

    // 체력 경고 이펙트 활성화 설정
    public void SetActiveHpWarning(bool value)
    {
        activeHpWarning = value;
    }

    // 플레이어 피격 이펙트 보여주기
    public void ShowPlayerDamage()
    {
        // 플레이어 피격 이펙트 나타냈다가 숨기기
        StartCoroutine(FadeDamageEffect());
    }

    // 플레이어 피격 이펙트 나타냈다가 숨기기
    IEnumerator FadeDamageEffect()
    {
        // 프레임 단위 만큼 알파값 감소
        for (float i = 1.0f; i >= -0.1f; i -= Time.deltaTime)
        {
            damageEffect.color = new Vector4(1, 1, 1, i);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    // 플레이어 체력 경고 이펙트 보여주기
    public void ShowHpWarning()
    {
        // 알파값 0에서 1로, 1에서 0으로 번갈아가면서 천천히 변경
        hpWarningEffect.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), Mathf.PingPong(Time.time, 1.0f));
    }
}
