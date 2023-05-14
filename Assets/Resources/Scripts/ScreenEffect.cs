using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour
{
    Image damageEffect;           // �ǰ� ����Ʈ �̹���
    Image hpWarningEffect;        // ü�� ��� ����Ʈ �̹���

    bool activeHpWarning = false; // ü�� ��� ����Ʈ Ȱ��ȭ �÷���

    void Start()
    {
        // �ʱ�ȭ
        damageEffect = UIManager.instance.playerDamageEffect;
        hpWarningEffect = UIManager.instance.playerHpWarningEffect;

        activeHpWarning = false;
    }

    void Update()
    {
        // ü�� ��� ����Ʈ�� Ȱ��ȭ �� ���
        if (activeHpWarning)
        {
            // �÷��̾� ü�� ��� ����Ʈ �����ֱ�
            ShowHpWarning();
        }
    }

    // ü�� ��� ����Ʈ Ȱ��ȭ ����
    public void SetActiveHpWarning(bool value)
    {
        activeHpWarning = value;
    }

    // �÷��̾� �ǰ� ����Ʈ �����ֱ�
    public void ShowPlayerDamage()
    {
        // �÷��̾� �ǰ� ����Ʈ ��Ÿ�´ٰ� �����
        StartCoroutine(FadeDamageEffect());
    }

    // �÷��̾� �ǰ� ����Ʈ ��Ÿ�´ٰ� �����
    IEnumerator FadeDamageEffect()
    {
        // ������ ���� ��ŭ ���İ� ����
        for (float i = 1.0f; i >= -0.1f; i -= Time.deltaTime)
        {
            damageEffect.color = new Vector4(1, 1, 1, i);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    // �÷��̾� ü�� ��� ����Ʈ �����ֱ�
    public void ShowHpWarning()
    {
        // ���İ� 0���� 1��, 1���� 0���� �����ư��鼭 õõ�� ����
        hpWarningEffect.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), Mathf.PingPong(Time.time, 1.0f));
    }
}
