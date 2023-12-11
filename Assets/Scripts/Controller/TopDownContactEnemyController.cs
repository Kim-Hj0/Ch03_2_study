using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealthSystem;
    private TopDownMovement _collidingMovement;


    protected override void Start()
    {
        base.Start();

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    private void OnDamage() //데미지를 받았을 때 무조건 따라오게
    {
        followRange = 100f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_isCollidingWithTarget)  //충돌하고 있어. 불값.(근거리도 이렇게 적용하면 됨)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if(DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotz = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotz) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision) //충돌이 들어온 것과
    {
        GameObject receiver = (collision.gameObject);

        if(!receiver.CompareTag(targetTag))
        {
            return;
        }

        _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if (_collidingTargetHealthSystem != null)
        {
            _isCollidingWithTarget = true;
        }

        _collidingMovement = receiver.GetComponent<TopDownMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision) //충돌이 나간 것.
    {
        if (!collision.CompareTag(targetTag))
        {
            return;
        }
        _isCollidingWithTarget = false;
    }

    private void ApplyHealthChange()    //공격하는 거.
    {
        AttackSO attackSO = Stats.CurrentStates.attackSO;
        bool hasBeenChanged = _collidingTargetHealthSystem.ChangeHealth(-attackSO.power);
        if (attackSO.isOnKnockback && _collidingMovement != null)
        {
            _collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
        }
    }


}
