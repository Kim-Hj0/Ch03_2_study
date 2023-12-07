using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsChangeType
{
    Add,    //���ϳ�
    Multiple,   //���ϳ�
    Override,   //�ٲ㾲��.
}

[Serializable]
public class CharacterStats
{
    public StatsChangeType statsChangeType;
    [Range(1, 100)] public int maxHealth;
    [Range(1f, 20f)] public float speed;

    //���ݿ� ���� ������ ����
    public AttackSO attackSO;
}
