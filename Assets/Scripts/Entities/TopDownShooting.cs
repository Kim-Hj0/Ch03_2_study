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
        _controller = GetComponent<TopDownCharacterController>();   //�����ũ���� ���۷�Ʈ�� �غ� �����ϰ� Start���� Ȱ���ϴ� ������ �ڵ带 �����ϴ� �� ����.
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

    private void OnShoot(AttackSO attackSO)  //������ ����ؾ���.
    {

        CreateProjectile();
    }

    private void CreateProjectile() //������ �����Ұž�.
    {
        Instantiate(testPrefab, projectileSpawnPosition.position, Quaternion.identity);
        //Instantiate ��κ��� �͵��� ������ �� ����.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
