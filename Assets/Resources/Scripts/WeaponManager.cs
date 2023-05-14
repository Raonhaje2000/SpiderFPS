using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    GameObject[] weapons;              // ���� ���� ���� �迭
    GameObject weaponPosition;         // ���Ⱑ �����Ǵ� ��ġ

    GameObject currentWeapon;          // ���� ������ ����
    int weaponIndex = 0;               // ���� ������ ������ �ε���

    float changeDelay = 3.0f;          // ���� �����, ���� ��������� ������
    bool isChangeDelay = false;        // ���� ���� ���������� üũ�ϴ� �÷���

    GameObject bullet;                 // �Ѿ� ������Ʈ
    Transform bulletSpawnPoint;        // �Ѿ��� �����Ǵ� ��ġ

    public int[] bulletMaxCount;       // �ִ� �Ѿ� ���� ���� �迭
    public int[] bulletCurrentCount;   // ���� �Ѿ� ���� ���� �迭

    public float[] fireDelay;          // �Ѿ� �߻� ������ (���� ������ ���� �޶���)
    bool isFireDelay = false;          // �Ѿ� �߻� ���������� üũ�ϴ� �÷���

    float reloadWarningDelay = 1.0f;   // ������ ��� �޼����� �� �ִ� ������
    bool activeReloadWarning = false;  // ������ �޼��� Ȱ��ȭ �÷���

    Transform muzzlePoint;             // ���� ����Ʈ�� �����Ǵ� ��ġ

    private void Awake()
    {
        // ���� ������ �ε�
        weapons = Resources.LoadAll<GameObject>("Prefabs/Gun");

        // �Ѿ� ������ �ε�
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");

        // ���� ���� ��ġ �ε�
        weaponPosition = this.gameObject;
    }

    void Start()
    {
        // ���� ������ ���� ������Ʈ ���� & ���� ������ �ʱ�ȭ
        weaponIndex = 0;

        currentWeapon = Instantiate(weapons[weaponIndex], weaponPosition.transform);
        bulletSpawnPoint = currentWeapon.transform.Find("BulletSpawnPoint");

        bulletMaxCount = new int[3] { 10, 20, 30 };
        bulletCurrentCount = (int[])bulletMaxCount.Clone();

        fireDelay = new float[3] { 0.4f, 0.2f, 0.1f };

        reloadWarningDelay = 1.0f;
        activeReloadWarning = false;

        muzzlePoint = currentWeapon.transform.Find("MuzzlePoint");

        // �Ѿ� �ؽ�Ʈ UI ����
        UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);
    }

    void Update()
    {
        ChangeWeapon(); // ���� ������ ���� ����
        Fire();         // �Ѿ� �߻�
        Reload();       // ������
    }

    // ���� ������ ���� ����
    void ChangeWeapon()
    {
        // ���� ���� ���°� �ƴϰ� ���� ���� �����̰� �ƴ� ��쿡�� ���� ���� ����
        if (!GameManager.instance.isGameOver && !isChangeDelay)
        {
            // ���콺 �� �Է��� �޾ƿ�
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (wheelInput > 0) // ���콺 ���� ���� ������ ��
            {
                weaponIndex++;

                if (weaponIndex > weapons.Length - 1)
                {
                    weaponIndex = 0;
                }

                isChangeDelay = true;

                ChangeWeaponModel(); // ���� ������Ʈ ����
            }
            else if (wheelInput < 0) // ���콺 ���� �Ʒ��� ������ ��
            {
                weaponIndex--;

                if (weaponIndex < 0)
                {
                    weaponIndex = weapons.Length - 1;
                }

                isChangeDelay = true;

                ChangeWeaponModel(); // ���� ������Ʈ ����
            }
        }
    }

    // ���� ������Ʈ ����
    void ChangeWeaponModel()
    {
        Destroy(currentWeapon); // ���� ������ ���� ������Ʈ ����

        // ���� ������ ���� ������Ʈ ���� & ���� ������ ����
        currentWeapon = Instantiate(weapons[weaponIndex], weaponPosition.transform);
        bulletSpawnPoint = currentWeapon.transform.Find("BulletSpawnPoint");
        muzzlePoint = currentWeapon.transform.Find("MuzzlePoint");

        // ���� ���� ȿ���� ���
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.weaponChangeSound, 0, SoundManager.instance.sfxVolum - 0.2f);

        // �Ѿ� �ؽ�Ʈ UI ����
        UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);

        // ���� ���� ������ ����
        StartCoroutine(WaitChangeDelay());
    }

    // ���� ���� ������ ����
    private IEnumerator WaitChangeDelay()
    {
        // ���� ���� �Ұ� �ؽ�Ʈ UI Ȱ��ȭ
        UIManager.instance.ActiveWeaponDelayText(true);

        yield return new WaitForSeconds(changeDelay);

        isChangeDelay = false;

        // ���� ���� �Ұ� �ؽ�Ʈ UI ��Ȱ��ȭ
        UIManager.instance.ActiveWeaponDelayText(false);
    }

    // �Ѿ� �߻�
    private void Fire()
    {
        // �Ѿ� �߻� �����̰� �ƴϰ�, ���콺 ���� ��ư �Է��� ������ ���� �Ѿ� �߻�
        if (!GameManager.instance.isGameOver && !isFireDelay && Input.GetMouseButton(0))
        {
            if (bulletCurrentCount[weaponIndex] > 0) // ���� ������ ������ �Ѿ��� �����ִ� ���
            {
                // �Ѿ� ���� �� ���� ������ ������ �Ѿ� ���� ����
                Instantiate(bullet, bulletSpawnPoint);
                bulletCurrentCount[weaponIndex]--;

                // ���� ����Ʈ ���� �� 0.2�� �� ����
                GameObject muzzleObject = Instantiate(ParticleManager.instance.muzzleParticle, muzzlePoint);
                Destroy(muzzleObject, 0.2f);

                // �ѼҸ� ȿ���� ���
                SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.shootingSound[weaponIndex], 0, SoundManager.instance.shootingVolum[weaponIndex]);

                //Debug.Log(weaponIndex + ": " + bulletCurrentCount[weaponIndex] + "/" + bulletMaxCount[weaponIndex]);

                // �Ѿ� �ؽ�Ʈ UI ����
                UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);

                // �Ѿ� �߻� ������ ����
                StartCoroutine(WaitFireDelay());
            }
            else // �ش� ������ �Ѿ��� �������� ���� ���
            {
                //Debug.Log("�Ѿ� ����");

                // ������ ��� �޼��� ������ ����
                StartCoroutine(ShowReloadWarning());
            }
        }
    }

    // �Ѿ� �߻� ������ ���� (���� ������ ���� �޶���)
    private IEnumerator WaitFireDelay()
    {
        isFireDelay = true;

        yield return new WaitForSeconds(fireDelay[weaponIndex]);

        isFireDelay = false;
    }

    // ������
    private void Reload()
    {
        // ���콺 ������ ��ư�� Ŭ������ ��
        if (!GameManager.instance.isGameOver && Input.GetMouseButtonDown(1))
        {
            //Debug.Log("������");

            // ���� ������ ������ ���� �Ѿ� ������ �ִ� �Ѿ� ������ ����
            bulletCurrentCount[weaponIndex] = bulletMaxCount[weaponIndex];

            // ������ ȿ���� ���
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.weaponReloadSound, 0, SoundManager.instance.sfxVolum);

            // �Ѿ� �ؽ�Ʈ UI ����
            UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);
        }
    }

    // ������ ��� �޼��� ������ ����
    private IEnumerator ShowReloadWarning()
    {
        // ������ �޼����� ��Ȱ��ȭ�� ��
        if (!activeReloadWarning)
        {
            activeReloadWarning = true;

            // ������ ��� �޼��� �ؽ�Ʈ UI Ȱ��ȭ
            UIManager.instance.ActiveReloadWarningText(activeReloadWarning);

            yield return new WaitForSeconds(reloadWarningDelay);
          
            activeReloadWarning = false;

            // ������ ��� �޼��� �ؽ�Ʈ UI ��Ȱ��ȭ
            UIManager.instance.ActiveReloadWarningText(activeReloadWarning);
        }
    }
}