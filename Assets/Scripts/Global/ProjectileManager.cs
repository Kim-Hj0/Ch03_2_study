using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impactParticleSystem;

    
    public static ProjectileManager instance; //싱글톤의 단점. 가장 마지막에 접근한 것만 처리할 수 있다. 그래서 싱글톤을 사용할 때는 단일객체를 중시한다.(단일객체를 만들어서 하나만 만든다.)

    private ObjectPool objectPool;

    private void Awake()    //싱글톤
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void ShootBullet(Vector2 startPostiion, Vector2 direction, RangedAttackData attackData)  
    {
        GameObject obj = objectPool.SpawnFromPool(attackData.bulletNameTag);

        obj.transform.position = startPostiion;
        RangedAttackController attackController = obj.GetComponent<RangedAttackController>();
        attackController.InitializeAttack(direction, attackData, this);

        obj.SetActive(true);

    }
    
}
