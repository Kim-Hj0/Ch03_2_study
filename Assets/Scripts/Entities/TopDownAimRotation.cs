using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{

    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;
    //SerializeField ����ȭ ��Ű�� ��.

    [SerializeField] private SpriteRenderer characterRenderer;

    private TopDownCharacterController _contrllor;
    //TopDownCharacterController�ʿ�. �˾ƺ����ϴϱ�.

    private void Awake()
    {
        _contrllor = GetComponent<TopDownCharacterController>();  //ž�ٿ� ��Ʈ�ѷ��� ã��.
    }

    // Start is called before the first frame update
    void Start()    //���콺 ������ ���� ����.
    {
        _contrllor.OnLookEvent += OnAim;    //OnAim�� �����ϱ� ������ ��.
    }

    public void OnAim(Vector2 newAimDirection)  //���콺�� ������ ������ 
    {
        RotateArm(newAimDirection); //�̰͵� �����ϱ� �� �����.
    }

    private void RotateArm(Vector2 direction)   //���콺�� �ٶ󺸴� �������� ĳ������ ���� ������ ������ �ٲ��ش�.
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Atan2�� ���ϴ� ��. ������ ���ϴ� ��.

        //������
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;  //flipY, Y���� �������� ������ ��ư.
        characterRenderer.flipX = armRenderer.flipY;    //ĳ���͸� �������� Ȱ ������ �ٲٰڴ�. �������� �� ���� ������ �� ��, Ȱ ������ �ٸ��� ������ �װ� �����ֱ� ���ؼ�.
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
