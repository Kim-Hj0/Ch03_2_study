using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnDeath : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();   //�ʿ��� �ſ� ���� ���� ��������.
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()  //�Ͼ�� ��.
    {
        _rigidbody.velocity = Vector3.zero; //������ �������� ���ϰ� ����.

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())    //���� ������ ��� SpriteRenderer�� �����Ͷ�.
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>()) //ehaviour�� �����ͼ� �����.
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
