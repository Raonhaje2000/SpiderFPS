using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform tr;
    Rigidbody rbody;

    public float bulletSpeed = 1500;  // �Ѿ� �ӵ�

    public float bulletDamage = 1.0f; // �Ѿ��� �ִ� ������

    void Start()
    {
        tr = this.transform; // �������� ����ĳ�� (����ȭ ���)
        rbody = GetComponent<Rigidbody>();

        // �������� ���� ����
        rbody.AddForce(tr.forward * bulletSpeed); // �׸��� ������ �ٵ� ����

        // 0.5�� �� �Ѿ� ����
        Destroy(tr.gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") // �浹�� ������Ʈ�� Enemy�� ��� �ش� ������Ʈ�� �������� ��
        {
            //Debug.Log("Enemy");

            // ���Ϳ��� ���ظ� ����
            other.GetComponent<SpiderState>().TakeDamageSpider(bulletDamage);

            // ���� �� ����Ʈ �����ֱ�
            ShowParticle(ParticleManager.instance.bloodParticle, 0.3f);
        }
        else if (other.gameObject.tag == "Ground") // �浹�� ������Ʈ�� Ground�� ���
        {
            //Debug.Log("Ground");

            // �Ѿ��� ���� �¾��� ���� ����Ʈ �����ֱ�
            ShowParticle(ParticleManager.instance.bulletGroundParticle, 1.0f);
        }
        else if (other.gameObject.tag == "Tree") // �浹�� ������Ʈ�� Tree�� ���
        {
            //Debug.Log("Tree");

            // �Ѿ��� ������ �¾��� ���� ����Ʈ �����ֱ�
            ShowParticle(ParticleManager.instance.bulletWoodParticle, 1.0f);
        }

        // �浹�� �Ѿ� ����
        Destroy(tr.gameObject);
    }

    // ��ƼŬ ����Ʈ �����ֱ�
    private void ShowParticle(GameObject particle, float time)
    {
        // ��ƼŬ ȸ�� ����
        Vector3 randomRotation = Vector3.one * Random.Range(200, 300);

        // ��ƼŬ GameObject ����
        GameObject particleObject = Instantiate(particle, tr.position, tr.rotation);
        particleObject.transform.rotation = Quaternion.Euler(randomRotation);

        // ���� �ð� ���� ��ƼŬ ����
        Destroy(particleObject.gameObject, time);
    }
}