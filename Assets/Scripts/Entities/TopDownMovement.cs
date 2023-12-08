using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TopDownMovement : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private CharacterStatsHandler _stats;   // stats 추가


    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private Vector2 _knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();   // 상위에 있는 TopDownCharacterController 가져오게 만드는 것.
        _stats = GetComponent<CharacterStatsHandler>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
    }

    private void FixedUpdate()
    {
        ApplyMovment(_movementDirection);   //키보드로 입력한 값을 _movementDirection로.
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        _knockback = -(other.position - transform.position).normalized * power;
    }

    private void ApplyMovment(Vector2 direction)
    {
        direction = direction * _stats.CurrentStates.speed;

        if (knockbackDuration > 0.0f)
        {
            direction += _knockback;
        }

        _rigidbody.velocity = direction;
    }
}

