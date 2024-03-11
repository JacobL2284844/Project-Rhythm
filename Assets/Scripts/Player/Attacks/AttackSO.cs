using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/ Attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOverride;
    public float attackDistanceToEnemy = 1.3f;
    public float pointInAnimationToSkipPerfectHit;
    public Animation[] enemyReactions; // levels low to high strength
}
