using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackData", menuName = "TopDownController/Attacks/Ranged", order = 1)]

public class RangedAttackData : AttackSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float duration;
    public float spread;
    public int numberofProjectilesPerShot;  //ÇÑ ¹ø¿¡ ½ò ¾ç.
    public float multipleProjectilesAngel;  //ÇÑ ¹ø¿¡ ½ò ¶§ ¾Þ±Û
    public Color projectileColor;   //»ö±ò.
}
