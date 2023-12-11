using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impactParticleSystem;

    
    public static ProjectileManager instance; //�̱����� ����. ���� �������� ������ �͸� ó���� �� �ִ�. �׷��� �̱����� ����� ���� ���ϰ�ü�� �߽��Ѵ�.(���ϰ�ü�� ���� �ϳ��� �����.)

    private ObjectPool objectPool;

    private void Awake()    //�̱���
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void ShootBullet(Vector2 startPostiion, Vector2 direction, RangedAttackData attackData)  
    {
        GameObject obj = objectPool.SpawnFromPool(attackData.bulletNameTag);

        obj.transform.position = startPostiion;
        RangedAttackController attackController = obj.GetComponent<RangedAttackController>();
        attackController.InitializeAttack(direction, attackData, this);

        obj.SetActive(true);

    }

    public void CreateImpactParticlesAtPostion(Vector3 position, RangedAttackData attackData)
    {
        _impactParticleSystem.transform.position = position;    //������ ��ġ�� �ٲٴ� ��.
        ParticleSystem.EmissionModule em = _impactParticleSystem.emission; //emission �����ϴ�
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5))); // ������ ���� ũ�Ⱑ �ٸ���, ����İ� Ŀ����.
        ParticleSystem.MainModule mainModule = _impactParticleSystem.main;
        mainModule.startSpeedMultiplier = attackData.size * 10f;
        _impactParticleSystem.Play();
    }
    
}
