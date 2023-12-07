using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsChangeType
{
    Add,    //더하냐
    Multiple,   //곱하냐
    Override,   //바꿔쓰냐.
}

[Serializable]
public class CharacterStats
{
    public StatsChangeType statsChangeType;
    [Range(1, 100)] public int maxHealth;
    [Range(1f, 20f)] public float speed;

    //공격에 대한 데이터 저장
    public AttackSO attackSO;
}
