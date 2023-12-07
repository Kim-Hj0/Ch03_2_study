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
        _camera = Camera.main; //이 씬에 존재하는 태그가 메인카메라인 걸 가져오겠다.
    }

    public void OnMove(InputValue value)    //이 앞에 on을 붙이면 실행이 되었을 때 돌려받는 함수를 만드는 것.
    {
        //Debug.Log("OnMove" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized; //.normalized; 하는 이유, 한 곳으로 가면 상관없지만, 그게 아닐 경우.
        CallMoveEvent(moveInput);                           //.normalized 하지 않으면,직선 방향은 느린데 대각선 방향이 매우 빨라져버림.
    }

    public void OnLook(InputValue value)
    {
        //Debug.Log("OnLook" + value.ToString());
        Vector2 newAim = value.Get<Vector2>();  //마우스 포지션을 가져옴.
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);  //그걸 월드 포지션으로 바꿈.
        newAim = (worldPos - (Vector2)transform.position).normalized;   //나한테서 마우스를 바라보는 방향.

        if (newAim.magnitude >= .9f)
        {
            CallLookEvent(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        
        IsAttacking = value.isPressed;  //실제로 키가 입력이 되면.
    }

}
