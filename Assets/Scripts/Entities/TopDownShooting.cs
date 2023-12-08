//using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    private ProjectileManager _projectileManager;
    private TopDownCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

 

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();   //�����ũ���� ���۷�Ʈ�� �غ� �����ϰ� Start���� Ȱ���ϴ� ������ �ڵ带 �����ϴ� �� ����.
    }


    // Start is called before the first frame update
    void Start()
    {
        _projectileManager = ProjectileManager.instance;    //�̱���, ���ϰ�ü�� ������ �� �ֵ��� �������.
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)  //������ ����ؾ���.
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngel;
        int numberOfProjectilesPerShot = rangedAttackData.numberofProjectilesPerShot;

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace + 0.5f * rangedAttackData.multipleProjectilesAngel; 
        
        for (int i = 0; i < numberOfProjectilesPerShot; i++) //ȭ���� ���� �� �����ϰ� ���� ����.
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
            angle += randomSpread;  //�������� �������� ������ ���ؼ�, �������� ��������� ����� ��.
            CreateProjectile(rangedAttackData, angle);
        }


        
    }

    private void CreateProjectile(RangedAttackData rangedAttackData, float angle) //������ �����Ұž�.
    {
        //ȭ���� ��� ���� ����.
        //����ü�� �����ϴ�.
        _projectileManager.ShootBullet
            (projectileSpawnPosition.position,
            RotateVector2(_aimDirection, angle),
            rangedAttackData);
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;  //���͸� ȸ����Ű�� ���� ���� Quaternion�� ����ϱ�.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
