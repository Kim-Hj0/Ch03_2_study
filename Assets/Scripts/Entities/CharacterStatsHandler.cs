using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats baseStats;
    public CharacterStats CurrentStates { get; private set; }
    public List<CharacterStats> statsModifiers = new List<CharacterStats>();    
    //추가로 더 들어오게 되면 적용할 스탯.
    //많이 사용됨. 아이템 구현이나 사용할 때 이런 코드를 많이 씀.

    private void Awake()
    {
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats()
    {
        AttackSO attackSO = null;
        if (baseStats.attackSO != null) //null이 아니라면 새로 만들거다.
        {
            attackSO = Instantiate(baseStats.attackSO);//baseStats 걸려있는 얘를 가상으로 하나 더 복제하는 것. 메모리상으로 하나 더 복제한 것.
        }

        CurrentStates = new CharacterStats { attackSO = attackSO }; //생성하면서 초기화하려면 중괄호로 해야함.
        // TODO
        CurrentStates.statsChangeType = baseStats.statsChangeType; 
        CurrentStates.maxHealth = baseStats.maxHealth;
        CurrentStates.speed = baseStats.speed;
    }

}
