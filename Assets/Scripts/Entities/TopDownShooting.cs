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
        _controller = GetComponent<TopDownCharacterController>();   //어웨이크에서 컴퍼런트의 준비를 시작하고 Start에서 활용하는 식으로 코드를 구성하는 게 좋음.
    }


    // Start is called before the first frame update
    void Start()
    {
        _projectileManager = ProjectileManager.instance;    //싱글톤, 단일객체에 접근할 수 있도록 만들어줌.
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)  //실제로 사용해야함.
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngel;
        int numberOfProjectilesPerShot = rangedAttackData.numberofProjectilesPerShot;

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace + 0.5f * rangedAttackData.multipleProjectilesAngel; 
        
        for (int i = 0; i < numberOfProjectilesPerShot; i++) //화살을 여러 개 생성하게 만들 거임.
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
            angle += randomSpread;  //랜덤으로 벌어지는 각도를 정해서, 랜덤으로 쏘아지도록 만드는 것.
            CreateProjectile(rangedAttackData, angle);
        }


        
    }

    private void CreateProjectile(RangedAttackData rangedAttackData, float angle) //실제로 생성할거야.
    {
        //화살을 쏘게 만들 것임.
        //투사체를 관리하는.
        _projectileManager.ShootBullet
            (projectileSpawnPosition.position,
            RotateVector2(_aimDirection, angle),
            rangedAttackData);
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;  //벡터를 회전시키고 싶을 때는 Quaternion을 사용하기.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
