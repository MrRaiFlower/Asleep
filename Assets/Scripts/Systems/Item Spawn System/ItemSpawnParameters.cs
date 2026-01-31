using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpawnParameters", menuName = "Scriptable Objects/ItemSpawnParameters")]
public class ItemSpawnParameters : ScriptableObject
{
    public int maxAmount;
    public float spawnChance;
}
