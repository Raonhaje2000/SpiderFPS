using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    public GameObject muzzleParticle;       // 머즐 이펙트

    public GameObject bulletWoodParticle;   // 총알이 나무에 맞았을 때 이펙트
    public GameObject bulletGroundParticle; // 총알이 땅에 맞았을 때 이펙트

    public GameObject bloodParticle;        // 몬스터 피 이펙트

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 리소스 로드
        LoadResources();
    }

    // 리소스 로드
    private void LoadResources()
    {
        muzzleParticle = Resources.Load<GameObject>("Particle/WFX_MF 4P RIFLE1");

        bulletWoodParticle = Resources.Load<GameObject>("Particle/WFX_BImpact Wood");
        bulletGroundParticle = Resources.Load<GameObject>("Particle/WFX_BImpact Sand");

        bloodParticle = Resources.Load<GameObject>("Particle/BloodSprayFX");
    }
}
