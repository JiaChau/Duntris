using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/StatBuff")]
public class StatBuff : ScriptableObject
{
    public enum StatType { MaxHealth, MoveSpeed, MeleeRange, MeleeDamage, ProjectileCount }
    public StatType statType;
    public float amount;

    public void Apply(PlayerStats stats)
    {
        switch (statType)
        {
            case StatType.MaxHealth: stats.maxHealth += amount; break;
            case StatType.MoveSpeed: stats.moveSpeed += amount; break;
            case StatType.MeleeRange: stats.meleeRange += amount; break;
            case StatType.MeleeDamage: stats.meleeDamage += amount; break;
            case StatType.ProjectileCount: stats.projectileCount += (int)amount; break;
        }
    }
}
