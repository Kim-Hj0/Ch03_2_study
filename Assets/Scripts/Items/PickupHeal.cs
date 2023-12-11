using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : PickupItem
{
    [SerializeField] int healValue = 10;
    private HealthSystem _healthSystem;
    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();  //+10을 해주는 것, 랜덤회복으로도 가능.
        _healthSystem.ChangeHealth(healValue);
    }

}
