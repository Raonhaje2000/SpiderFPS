using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    GameObject[] weapons;              // 무기 종류 저장 배열
    GameObject weaponPosition;         // 무기가 생성되는 위치

    GameObject currentWeapon;          // 현재 장착한 무기
    int weaponIndex = 0;               // 현재 장착한 무기의 인덱스

    float changeDelay = 3.0f;          // 무기 변경시, 다음 변경까지의 딜레이
    bool isChangeDelay = false;        // 무기 변경 딜레이인지 체크하는 플래그

    GameObject bullet;                 // 총알 오브젝트
    Transform bulletSpawnPoint;        // 총알이 생성되는 위치

    public int[] bulletMaxCount;       // 최대 총알 개수 저장 배열
    public int[] bulletCurrentCount;   // 현재 총알 개수 저장 배열

    public float[] fireDelay;          // 총알 발사 딜레이 (무기 종류에 따라 달라짐)
    bool isFireDelay = false;          // 총알 발사 딜레이인지 체크하는 플래그

    float reloadWarningDelay = 1.0f;   // 재장전 경고 메세지가 떠 있는 딜레이
    bool activeReloadWarning = false;  // 재장전 메세지 활성화 플래그

    Transform muzzlePoint;             // 머즐 이펙트가 생성되는 위치

    private void Awake()
    {
        // 무기 프리팹 로드
        weapons = Resources.LoadAll<GameObject>("Prefabs/Gun");

        // 총알 프리팹 로드
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");

        // 무기 생성 위치 로드
        weaponPosition = this.gameObject;
    }

    void Start()
    {
        // 현재 장착한 무기 오브젝트 생성 & 관련 데이터 초기화
        weaponIndex = 0;

        currentWeapon = Instantiate(weapons[weaponIndex], weaponPosition.transform);
        bulletSpawnPoint = currentWeapon.transform.Find("BulletSpawnPoint");

        bulletMaxCount = new int[3] { 10, 20, 30 };
        bulletCurrentCount = (int[])bulletMaxCount.Clone();

        fireDelay = new float[3] { 0.4f, 0.2f, 0.1f };

        reloadWarningDelay = 1.0f;
        activeReloadWarning = false;

        muzzlePoint = currentWeapon.transform.Find("MuzzlePoint");

        // 총알 텍스트 UI 변경
        UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);
    }

    void Update()
    {
        ChangeWeapon(); // 현재 장착한 무기 변경
        Fire();         // 총알 발사
        Reload();       // 재장전
    }

    // 현재 장착한 무기 변경
    void ChangeWeapon()
    {
        // 게임 오버 상태가 아니고 무기 변경 딜레이가 아닌 경우에만 무기 변경 가능
        if (!GameManager.instance.isGameOver && !isChangeDelay)
        {
            // 마우스 휠 입력을 받아옴
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (wheelInput > 0) // 마우스 휠을 위로 돌렸을 때
            {
                weaponIndex++;

                if (weaponIndex > weapons.Length - 1)
                {
                    weaponIndex = 0;
                }

                isChangeDelay = true;

                ChangeWeaponModel(); // 무기 오브젝트 변경
            }
            else if (wheelInput < 0) // 마우스 휠을 아래로 돌렸을 때
            {
                weaponIndex--;

                if (weaponIndex < 0)
                {
                    weaponIndex = weapons.Length - 1;
                }

                isChangeDelay = true;

                ChangeWeaponModel(); // 무기 오브젝트 변경
            }
        }
    }

    // 무기 오브젝트 변경
    void ChangeWeaponModel()
    {
        Destroy(currentWeapon); // 현재 장착한 무기 오브젝트 제거

        // 현재 장착한 무기 오브젝트 변경 & 관련 데이터 갱신
        currentWeapon = Instantiate(weapons[weaponIndex], weaponPosition.transform);
        bulletSpawnPoint = currentWeapon.transform.Find("BulletSpawnPoint");
        muzzlePoint = currentWeapon.transform.Find("MuzzlePoint");

        // 무기 변경 효과음 재생
        SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.weaponChangeSound, 0, SoundManager.instance.sfxVolum - 0.2f);

        // 총알 텍스트 UI 변경
        UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);

        // 무기 변경 딜레이 적용
        StartCoroutine(WaitChangeDelay());
    }

    // 무기 변경 딜레이 적용
    private IEnumerator WaitChangeDelay()
    {
        // 무기 변경 불가 텍스트 UI 활성화
        UIManager.instance.ActiveWeaponDelayText(true);

        yield return new WaitForSeconds(changeDelay);

        isChangeDelay = false;

        // 무기 변경 불가 텍스트 UI 비활성화
        UIManager.instance.ActiveWeaponDelayText(false);
    }

    // 총알 발사
    private void Fire()
    {
        // 총알 발사 딜레이가 아니고, 마우스 왼쪽 버튼 입력이 들어왔을 때만 총알 발사
        if (!GameManager.instance.isGameOver && !isFireDelay && Input.GetMouseButton(0))
        {
            if (bulletCurrentCount[weaponIndex] > 0) // 현재 장착한 무기의 총알이 남아있는 경우
            {
                // 총알 생성 후 현재 장착한 무기의 총알 개수 감소
                Instantiate(bullet, bulletSpawnPoint);
                bulletCurrentCount[weaponIndex]--;

                // 머즐 이펙트 생성 후 0.2초 뒤 제거
                GameObject muzzleObject = Instantiate(ParticleManager.instance.muzzleParticle, muzzlePoint);
                Destroy(muzzleObject, 0.2f);

                // 총소리 효과음 재생
                SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.shootingSound[weaponIndex], 0, SoundManager.instance.shootingVolum[weaponIndex]);

                //Debug.Log(weaponIndex + ": " + bulletCurrentCount[weaponIndex] + "/" + bulletMaxCount[weaponIndex]);

                // 총알 텍스트 UI 변경
                UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);

                // 총알 발사 딜레이 적용
                StartCoroutine(WaitFireDelay());
            }
            else // 해당 무기의 총알이 남아있지 않은 경우
            {
                //Debug.Log("총알 부족");

                // 재장전 경고 메세지 딜레이 적용
                StartCoroutine(ShowReloadWarning());
            }
        }
    }

    // 총알 발사 딜레이 적용 (무기 종류에 따라 달라짐)
    private IEnumerator WaitFireDelay()
    {
        isFireDelay = true;

        yield return new WaitForSeconds(fireDelay[weaponIndex]);

        isFireDelay = false;
    }

    // 재장전
    private void Reload()
    {
        // 마우스 오른쪽 버튼을 클릭했을 때
        if (!GameManager.instance.isGameOver && Input.GetMouseButtonDown(1))
        {
            //Debug.Log("재장전");

            // 현재 장착한 무기의 현재 총알 개수를 최대 총알 개수로 변경
            bulletCurrentCount[weaponIndex] = bulletMaxCount[weaponIndex];

            // 재장전 효과음 재생
            SoundManager.instance.PlaySfx(transform.position, SoundManager.instance.weaponReloadSound, 0, SoundManager.instance.sfxVolum);

            // 총알 텍스트 UI 변경
            UIManager.instance.ChangeWeaponBulletText(bulletCurrentCount[weaponIndex], bulletMaxCount[weaponIndex]);
        }
    }

    // 재장전 경고 메세지 딜레이 적용
    private IEnumerator ShowReloadWarning()
    {
        // 재장전 메세지가 비활성화일 때
        if (!activeReloadWarning)
        {
            activeReloadWarning = true;

            // 재장전 경고 메세지 텍스트 UI 활성화
            UIManager.instance.ActiveReloadWarningText(activeReloadWarning);

            yield return new WaitForSeconds(reloadWarningDelay);
          
            activeReloadWarning = false;

            // 재장전 경고 메세지 텍스트 UI 비활성화
            UIManager.instance.ActiveReloadWarningText(activeReloadWarning);
        }
    }
}