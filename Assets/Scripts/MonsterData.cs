using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public float maxHP = 100f;
    public float damage = 20f;
    public float speed = 2f;
    public float attackDistance = 1f;
    public float spawnInterval = 2f;
}
