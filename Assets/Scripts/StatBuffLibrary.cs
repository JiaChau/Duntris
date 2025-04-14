// StatBuffLibrary.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Buff Library")]
public class StatBuffLibrary : ScriptableObject
{
    public StatBuff[] allBuffs;

    public StatBuff GetRandomBuff()
    {
        if (allBuffs == null || allBuffs.Length == 0) return null;
        return allBuffs[Random.Range(0, allBuffs.Length)];
    }
}
