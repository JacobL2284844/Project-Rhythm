using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/ Combo")]
public class ComboSO : ScriptableObject
{
    public List<AttackSO> combo;
}
