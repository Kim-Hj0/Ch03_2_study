using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerInputController : TopDownCharacterController
{
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main; //�� ���� �����ϴ� �±װ� ����ī�޶��� �� �������ڴ�.
    }

    public void OnMove(InputValue value)    //�� �տ� on�� ���̸� ������ �Ǿ��� �� �����޴� �Լ��� ����� ��.
    {
        //Debug.Log("OnMove" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized; //.normalized; �ϴ� ����, �� ������ ���� ���������, �װ� �ƴ� ���.
        CallMoveEvent(moveInput);                           //.normalized ���� ������,���� ������ ������ �밢�� ������ �ſ� ����������.
    }

    public void OnLook(InputValue value)
    {
        //Debug.Log("OnLook" + value.ToString());
        Vector2 newAim = value.Get<Vector2>();  //���콺 �������� ������.
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);  //�װ� ���� ���������� �ٲ�.
        newAim = (worldPos - (Vector2)transform.position).normalized;   //�����׼� ���콺�� �ٶ󺸴� ����.

        if (newAim.magnitude >= .9f)
        {
            CallLookEvent(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        
        IsAttacking = value.isPressed;  //������ Ű�� �Է��� �Ǹ�.
    }

}
