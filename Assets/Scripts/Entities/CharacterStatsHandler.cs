using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats baseStats;
    public CharacterStats CurrentStates { get; private set; }
    public List<CharacterStats> statsModifiers = new List<CharacterStats>();    
    //�߰��� �� ������ �Ǹ� ������ ����.
    //���� ����. ������ �����̳� ����� �� �̷� �ڵ带 ���� ��.

    private void Awake()
    {
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats()
    {
        AttackSO attackSO = null;
        if (baseStats.attackSO != null) //null�� �ƴ϶�� ���� ����Ŵ�.
        {
            attackSO = Instantiate(baseStats.attackSO);//baseStats �ɷ��ִ� �긦 �������� �ϳ� �� �����ϴ� ��. �޸𸮻����� �ϳ� �� ������ ��.
        }

        CurrentStates = new CharacterStats { attackSO = attackSO }; //�����ϸ鼭 �ʱ�ȭ�Ϸ��� �߰�ȣ�� �ؾ���.
        // TODO
        CurrentStates.statsChangeType = baseStats.statsChangeType; 
        CurrentStates.maxHealth = baseStats.maxHealth;
        CurrentStates.speed = baseStats.speed;
    }

}
