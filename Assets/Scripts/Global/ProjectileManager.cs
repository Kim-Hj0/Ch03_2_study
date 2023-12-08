using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impactParticleSystem;

    
    public static ProjectileManager instance; //�̱����� ����. ���� �������� ������ �͸� ó���� �� �ִ�. �׷��� �̱����� ����� ���� ���ϰ�ü�� �߽��Ѵ�.(���ϰ�ü�� ���� �ϳ��� �����.)

    [SerializeField] private GameObject testObj;

    private void Awake()    //�̱���
    {
        instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShootBullet(Vector2 startPostiion, Vector2 direction, RangedAttackData attackData)  
    {
        GameObject obj = Instantiate(testObj);

        obj.transform.position = startPostiion;
        RangedAttackController attackController = obj.GetComponent<RangedAttackController>();
        attackController.InitializeAttack(direction, attackData, this);

        obj.SetActive(true);

    }
    
}