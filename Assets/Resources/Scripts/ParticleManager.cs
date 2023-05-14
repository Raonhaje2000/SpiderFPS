using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    public GameObject muzzleParticle;       // ���� ����Ʈ

    public GameObject bulletWoodParticle;   // �Ѿ��� ������ �¾��� �� ����Ʈ
    public GameObject bulletGroundParticle; // �Ѿ��� ���� �¾��� �� ����Ʈ

    public GameObject bloodParticle;        // ���� �� ����Ʈ

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // ���ҽ� �ε�
        LoadResources();
    }

    // ���ҽ� �ε�
    private void LoadResources()
    {
        muzzleParticle = Resources.Load<GameObject>("Particle/WFX_MF 4P RIFLE1");

        bulletWoodParticle = Resources.Load<GameObject>("Particle/WFX_BImpact Wood");
        bulletGroundParticle = Resources.Load<GameObject>("Particle/WFX_BImpact Sand");

        bloodParticle = Resources.Load<GameObject>("Particle/BloodSprayFX");
    }
}
