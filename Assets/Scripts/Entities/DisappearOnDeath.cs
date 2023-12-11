using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnDeath : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();   //필요한 거에 대한 설정 가져오기.
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()  //일어났을 떄.
    {
        _rigidbody.velocity = Vector3.zero; //죽으면 움직이지 못하게 고정.

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())    //나를 포함한 모든 SpriteRenderer를 가져와라.
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>()) //ehaviour를 가져와서 꺼줘라.
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
