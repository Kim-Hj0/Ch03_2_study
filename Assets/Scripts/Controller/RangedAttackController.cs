using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedAttackData _attackData;
    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;
    private ProjectileManager _projectileManager;

    public bool fxOnDestory = true;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }


    private void Update()
    {
        if(!_isReady)
        {
            return;
        }

        _currentDuration += Time.deltaTime;

        if(_currentDuration > _attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = _direction * _attackData.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) //무언가와 충돌했을 때 트리거 충돌. 맵 밖으로 나가지 않는다. 벽에 닿으면 사라짐.
    {
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, fxOnDestory);
        }
        
        //원거리 공격이 충돌을 했을 때를 의미.
        else if(_attackData.target.value == (_attackData.target.value | (1 << collision.gameObject.layer))) 
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.ChangeHealth(-_attackData.power);
                if(_attackData.isOnKnockback)
                {
                    TopDownMovement movement = collision.GetComponent<TopDownMovement>();

                    if(movement != null)
                    {
                        movement.ApplyKnockback(transform, _attackData.knockbackPower, _attackData.knockbackTime);
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory); //충돌이 끝났으니 삭제를 해야겠죠.
        }
    }

    public void InitializeAttack(Vector2 direction, RangedAttackData attackData, ProjectileManager projectileManager)
    {
        _projectileManager = projectileManager;
        _attackData = attackData;
        _direction = direction;

        UpdateProjectilSprite();
        _trailRenderer.Clear(); //재사용하는 코드를 만들기 때문에 미리 만들어놓기.
        _currentDuration = 0;
        _spriteRenderer.color = attackData.projectileColor;

        transform.right = _direction; //그 방향으로 회전

        _isReady = true;
    }

    private void UpdateProjectilSprite()
    {
        transform.localScale = Vector3.one * _attackData.size;
    }


    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            _projectileManager.CreateImpactParticlesAtPostion(position, _attackData);
        }
        gameObject.SetActive(false);
    }
}
