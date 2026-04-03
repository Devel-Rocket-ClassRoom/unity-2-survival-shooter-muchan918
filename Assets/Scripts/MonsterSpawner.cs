using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    public Monster[] prefab;
    public Transform player;
    public GameManager gameManager;
    public float minSpawnRadius = 5f;
    public float spawnRadius = 20f;
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
            lastSpawnTime[i] = -prefab[i].spawnInterval;
        }
    }

    private void Update()
    {
        for (int i = 0; i < count; i++)
        {
            if (Time.time > lastSpawnTime[i] + monsterSpawnIntervals[i])
            {
                CreateMonster(i);
                lastSpawnTime[i] = Time.time;
            }
        }
    }

    private void CreateMonster(int index)
    {
        if (TryGetRandomNavMeshPoint(out Vector3 spawnPosition))
        {
            var monster = Instantiate(prefab[index], spawnPosition, Quaternion.identity);
            monster.target = player;

            monster.OnDead.AddListener(() => gameManager.AddScore(monster.score));
        }
    }

    private bool TryGetRandomNavMeshPoint(out Vector3 result)
    {
        int maxAttempts = 10; // 무한루프 방지

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPoint = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y,  // Y는 고정
                transform.position.z + randomCircle.y
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas))
            {
                // 플레이어와의 거리 체크
                float distToPlayer = Vector3.Distance(hit.position, player.position);
                if (distToPlayer >= minSpawnRadius)
                {
                    result = hit.position;
                    return true;
                }
            }
        }

        result = Vector3.zero;
        return false;
    }
}
