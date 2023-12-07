using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    // event �ܺο����� ȣ������ ���ϰ� �����ִ� ���.
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    //�̷� �� ������� ���� ���� �� �� �ֵ��� ����� �� ����.(�÷��̾�)
    public event Action OnAttackEvent;

    private float _timeSinceLastAttack = float.MaxValue;
    protected bool IsAttacking {  get; set; }    //���ÿ� ���� ������Ƽ

    protected CharacterStatsHandler Stats {  get; private set; }

    protected virtual void Awake()  //����ϴ� ��ϱ�, virtual
    {
        Stats = GetComponent<CharacterStatsHandler>();
    }

    protected  virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStates.attackSO == null)   //���� ������ ���ٸ� ������ ���� �ʰڴ�.
            return;

        if (_timeSinceLastAttack <= Stats.CurrentStates.attackSO.delay)  
        {
            _timeSinceLastAttack += Time.deltaTime;
        }
        if (IsAttacking && _timeSinceLastAttack > Stats.CurrentStates.attackSO.delay)
        {
            _timeSinceLastAttack = 0;   //0���� �ʱ�ȭ�������.
            CallAttackEvent();  //������ �����Ѵ�.
        }
    }

    public void CallMoveEvent(Vector2 direction)    //?. �տ� �ִ� �� null�� �ƴ� ���� �����ϰڴ�.
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



//�̵� ���� �ڵ�.
//[SerializeField] private float speed = 5f;

// Start is called before the first frame update //ù��° �ൿ.
//void Start()    
//{

//}

//// Update is called once per frame  //�� �����Ӹ��� �����Ѵ�.
//void Update()  
//{
//    //Drag, Ctrl + K C �ּ�  K U Ǯ��
//    //float x = Input.GetAxisRaw("Horizontal");  //����
//    //float y = Input.GetAxisRaw("Vertical");    //����

//    //transform.position += new Vector3(x, y) * speed * Time.deltaTime; //���� �����Ӱ� ���� �������� �ð�.
//}

