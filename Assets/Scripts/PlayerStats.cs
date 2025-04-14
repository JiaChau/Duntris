[System.Serializable]
public class PlayerStats
{
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float meleeDamage = 10f;
    public float meleeRange = 2f;
    public float rangedDamage = 5f;
    public float projectileSpeed = 10f;
    public int projectileCount = 1;

    // Base values for penalty clamping
    public float baseHealth = 100f;
    public float baseSpeed = 5f;
    public float baseMeleeDamage = 10f;
    public float baseMeleeRange = 2f;
    public float baseRangedDamage = 5f;
    public float baseProjectileSpeed = 10f;
    public int baseProjectileCount = 1;
}
