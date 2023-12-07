using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    private TopDownCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    public GameObject testPrefab;


    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();   //어웨이크에서 컴퍼런트의 준비를 시작하고 Start에서 활용하는 식으로 코드를 구성하는 게 좋음.
    }


    // Start is called before the first frame update
    void Start()
    {
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)  //실제로 사용해야함.
    {

        CreateProjectile();
    }

    private void CreateProjectile() //실제로 생성할거야.
    {
        Instantiate(testPrefab, projectileSpawnPosition.position, Quaternion.identity);
        //Instantiate 대부분의 것들을 복제할 수 있음.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
