using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform tr;
    Rigidbody rbody;

    public float bulletSpeed = 1500;  // 총알 속도

    public float bulletDamage = 1.0f; // 총알이 주는 데미지

    void Start()
    {
        tr = this.transform; // 직접적인 참조캐싱 (최적화 기법)
        rbody = GetComponent<Rigidbody>();

        // 전방으로 힘을 가함
        rbody.AddForce(tr.forward * bulletSpeed); // 그리고 리지드 바디 관련

        // 0.5초 후 총알 제거
        Destroy(tr.gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") // 충돌한 오브젝트가 Enemy인 경우 해당 오브젝트에 데미지를 줌
        {
            //Debug.Log("Enemy");

            // 몬스터에게 피해를 입힘
            other.GetComponent<SpiderState>().TakeDamageSpider(bulletDamage);

            // 몬스터 피 이펙트 보여주기
            ShowParticle(ParticleManager.instance.bloodParticle, 0.3f);
        }
        else if (other.gameObject.tag == "Ground") // 충돌한 오브젝트가 Ground인 경우
        {
            //Debug.Log("Ground");

            // 총알이 땅에 맞았을 때의 이펙트 보여주기
            ShowParticle(ParticleManager.instance.bulletGroundParticle, 1.0f);
        }
        else if (other.gameObject.tag == "Tree") // 충돌한 오브젝트가 Tree인 경우
        {
            //Debug.Log("Tree");

            // 총알이 나무에 맞았을 때의 이펙트 보여주기
            ShowParticle(ParticleManager.instance.bulletWoodParticle, 1.0f);
        }

        // 충돌한 총알 제거
        Destroy(tr.gameObject);
    }

    // 파티클 이펙트 보여주기
    private void ShowParticle(GameObject particle, float time)
    {
        // 파티클 회전 각도
        Vector3 randomRotation = Vector3.one * Random.Range(200, 300);

        // 파티클 GameObject 생성
        GameObject particleObject = Instantiate(particle, tr.position, tr.rotation);
        particleObject.transform.rotation = Quaternion.Euler(randomRotation);

        // 일정 시간 이후 파티클 제거
        Destroy(particleObject.gameObject, time);
    }
}