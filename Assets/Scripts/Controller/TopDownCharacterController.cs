using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    // event 외부에서는 호출하지 못하게 막아주는 기능.
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    //이런 걸 만들었을 때는 나만 쓸 수 있도록 만드는 게 좋다.(플레이어)
    public event Action OnAttackEvent;

    private float _timeSinceLastAttack = float.MaxValue;
    protected bool IsAttacking {  get; set; }    //어택에 대한 프로퍼티

    protected CharacterStatsHandler Stats {  get; private set; }

    protected virtual void Awake()  //상속하는 얘니까, virtual
    {
        Stats = GetComponent<CharacterStatsHandler>();
    }

    protected  virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStates.attackSO == null)   //어택 정보가 없다면 공격을 하지 않겠다.
            return;

        if (_timeSinceLastAttack <= Stats.CurrentStates.attackSO.delay)  
        {
            _timeSinceLastAttack += Time.deltaTime;
        }
        if (IsAttacking && _timeSinceLastAttack > Stats.CurrentStates.attackSO.delay)
        {
            _timeSinceLastAttack = 0;   //0으로 초기화해줘야함.
            CallAttackEvent();  //공격을 시전한다.
        }
    }

    public void CallMoveEvent(Vector2 direction)    //?. 앞에 있는 게 null이 아닐 때만 동작하겠다.
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }





}



//이동 연습 코드.
//[SerializeField] private float speed = 5f;

// Start is called before the first frame update //첫번째 행동.
//void Start()    
//{

//}

//// Update is called once per frame  //매 프레임마다 동작한다.
//void Update()  
//{
//    //Drag, Ctrl + K C 주석  K U 풀기
//    //float x = Input.GetAxisRaw("Horizontal");  //가로
//    //float y = Input.GetAxisRaw("Vertical");    //세로

//    //transform.position += new Vector3(x, y) * speed * Time.deltaTime; //이전 프레임과 현재 프레임의 시간.
//}

