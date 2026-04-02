using System.Threading;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster[] prefab;
    public Transform[] spawnPoints;
    public Transform player;
    private float[] monsterSpawnIntervals;
    private float[] lastSpawnTime;
    private int count;

    private void Awake()
    {
        count = prefab.Length;

        monsterSpawnIntervals = new float[prefab.Length];
        lastSpawnTime = new float[prefab.Length];

        for (int i = 0; i < count; i++)
        {
            monsterSpawnIntervals[i] = prefab[i].spawnInterval;
        }

        for (int i = 0; i < count; i++)
        {
            lastSpawnTime[i] = 0f;
        }
    }

    private void Update()
    {
        for (int i = 0; i < count; i++)
        {
            if (Time.time > lastSpawnTime[i] + monsterSpawnIntervals[i])
            {
                CreateMonster(i);
            }
        }
    }

    private void CreateMonster(int index)
    {
        var point = spawnPoints[index];
        var monster = Instantiate(prefab[index], point.position, point.rotation);
        monster.target = player;
    }
}
