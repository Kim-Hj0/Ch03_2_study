using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStatModifiers : PickupItem
{
    [SerializeField] private List<CharacterStats> statsModifier;
    protected override void OnPickedUp(GameObject receiver) //어떤 스탯을 가지고 있다가, haracterStatsHandler에 넣어주기만 하면 됨.
    {
        CharacterStatsHandler statsHandler = receiver.GetComponent<CharacterStatsHandler>();
        foreach (CharacterStats stat in statsModifier) 
        {
            statsHandler.AddStatModifier(stat);
        }
    }
}
