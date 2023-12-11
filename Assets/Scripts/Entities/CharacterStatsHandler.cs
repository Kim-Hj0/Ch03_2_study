using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    private const float MinAttackDelay = 0.03f;
    private const float MinAttackPower = 0.5f;
    private const float MinAttackSize = 0.4f;
    private const float MinAttackSpeed = .1f;

    private const float MinSpeed = 0.8f;
    private const int MinMaxHealth = 5;


    [SerializeField] private CharacterStats baseStats;
    public CharacterStats CurrentStates { get; private set; }
    public List<CharacterStats> statsModifiers = new List<CharacterStats>();
    //추가로 더 들어오게 되면 적용할 스탯.
    //많이 사용됨. 아이템 구현이나 사용할 때 이런 코드를 많이 씀.


    public void AddStatModifier(CharacterStats statModifier)    //새로운 스탯을 포함되고 내 원래있는 스탯을 추가한다.
    {
        statsModifiers.Add(statModifier);
        UpdateCharacterStats();
    }

    public void RemoveStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Remove(statModifier);
        UpdateCharacterStats();
    }

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
        UpdateStats((a, b) => b, baseStats);
        if (CurrentStates.attackSO != null) //attackSO있는지 확인
        {
            CurrentStates.attackSO.target = baseStats.attackSO.target;  //있다면
        }

        foreach (CharacterStats modifier in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            if (modifier.statsChangeType == StatsChangeType.Override)
            {
                UpdateStats((o, o1) => o1, modifier);
            }
            else if (modifier.statsChangeType == StatsChangeType.Add)
            {
                UpdateStats((o, o1) => o + o1, modifier);
            }
            else if (modifier.statsChangeType == StatsChangeType.Multiple)
            {
                UpdateStats((o, o1) => o * o1, modifier);
            }
        }
        LimitAllStats();
    }

    private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        CurrentStates.maxHealth = (int)operation(CurrentStates.maxHealth, newModifier.maxHealth);
        CurrentStates.speed = operation(CurrentStates.speed, newModifier.speed);

        if (CurrentStates.attackSO == null || newModifier.attackSO == null) 
            return;

        UpdateAttackStats(operation, CurrentStates.attackSO, newModifier.attackSO);

        if (CurrentStates.attackSO.GetType() != newModifier.attackSO.GetType())
        {
            return;
        }

        switch (CurrentStates.attackSO)
        {
            case RangedAttackData _:    //레인지에 있는
                ApplyRangedStats(operation, newModifier);
                break;
        }
    }

    private void UpdateAttackStats(Func<float, float, float> operation, AttackSO currentAttack, AttackSO newAttack) //어택
    {
        if (currentAttack == null || newAttack == null)
        {
            return;
        }

        currentAttack.delay = operation(currentAttack.delay, newAttack.delay);
        currentAttack.power = operation(currentAttack.power, newAttack.power);
        currentAttack.size = operation(currentAttack.size, newAttack.size);
        currentAttack.speed = operation(currentAttack.speed, newAttack.speed);
    }


    private void ApplyRangedStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        RangedAttackData currentRangedAttacks = (RangedAttackData)CurrentStates.attackSO;

        if (!(newModifier.attackSO is RangedAttackData))
        {
            return;
        }

        RangedAttackData rangedAttacksModifier = (RangedAttackData)newModifier.attackSO; //operation을 받아와서 쓰는 
        currentRangedAttacks.multipleProjectilesAngel =
            operation(currentRangedAttacks.multipleProjectilesAngel, rangedAttacksModifier.multipleProjectilesAngel);
        currentRangedAttacks.spread = operation(currentRangedAttacks.spread, rangedAttacksModifier.spread);
        currentRangedAttacks.duration = operation(currentRangedAttacks.duration, rangedAttacksModifier.duration);
        currentRangedAttacks.numberofProjectilesPerShot = Mathf.CeilToInt(operation(currentRangedAttacks.numberofProjectilesPerShot,
            rangedAttacksModifier.numberofProjectilesPerShot));
        currentRangedAttacks.projectileColor = UpdateColor(operation, currentRangedAttacks.projectileColor, rangedAttacksModifier.projectileColor);
    }

    private Color UpdateColor(Func<float, float, float> operation, Color currentColor, Color newColor)  //컬러
    {
        return new Color(
            operation(currentColor.r, newColor.r),
            operation(currentColor.g, newColor.g),
            operation(currentColor.b, newColor.b),
            operation(currentColor.a, newColor.a));
    }

    private void LimitStats(ref float stat, float minVal)
    {
        stat = Mathf.Max(stat, minVal);
    }

    private void LimitAllStats()    //리미트를 걸어주는 코드, 더 이상 작아지지 않는 걸 제한.
    {
        if (CurrentStates == null || CurrentStates.attackSO == null)
        {
            return;
        }

        LimitStats(ref CurrentStates.attackSO.delay, MinAttackDelay);
        LimitStats(ref CurrentStates.attackSO.power, MinAttackPower);
        LimitStats(ref CurrentStates.attackSO.size, MinAttackSize);
        LimitStats(ref CurrentStates.attackSO.speed, MinAttackSpeed);
        LimitStats(ref CurrentStates.speed, MinSpeed);
        CurrentStates.maxHealth = Mathf.Max(CurrentStates.maxHealth, MinMaxHealth);
    }

}
