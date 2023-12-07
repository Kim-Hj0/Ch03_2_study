using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{

    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;
    //SerializeField 정렬화 시키는 것.

    [SerializeField] private SpriteRenderer characterRenderer;

    private TopDownCharacterController _contrllor;
    //TopDownCharacterController필요. 알아봐야하니까.

    private void Awake()
    {
        _contrllor = GetComponent<TopDownCharacterController>();  //탑다운 컨트롤러를 찾음.
    }

    // Start is called before the first frame update
    void Start()    //마우스 움직일 때에 가동.
    {
        _contrllor.OnLookEvent += OnAim;    //OnAim이 없으니까 만들어야 함.
    }

    public void OnAim(Vector2 newAimDirection)  //마우스가 움직일 때마다 
    {
        RotateArm(newAimDirection); //이것도 없으니까 또 만들기.
    }

    private void RotateArm(Vector2 direction)   //마우스를 바라보는 방향으로 캐릭터의 몸과 무기의 방향을 바꿔준다.
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Atan2을 구하는 것. 각도를 구하는 것.

        //각도가
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;  //flipY, Y축을 기준으로 뒤집는 버튼.
        characterRenderer.flipX = armRenderer.flipY;    //캐릭터를 기준으로 활 방향을 바꾸겠다. 오른쪽을 볼 때와 왼쪽을 볼 때, 활 방향이 다르기 때문에 그걸 보여주기 위해서.
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
