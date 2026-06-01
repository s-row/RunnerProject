using UnityEngine;

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public float spawnPlayerX;
    public bool hasSpawned;
}

public class MonsterSpawnManager : MonoBehaviour
{
    public Transform player;
    public MonsterSpawnData[] monsterSpawnDatas;

    private void Update()
    {
        for (int i = 0; i < monsterSpawnDatas.Length; i++)
        {
            MonsterSpawnData data = monsterSpawnDatas[i];

            if (data.hasSpawned == true)
            {
                continue;
            }

            if (player.position.x >= data.spawnPlayerX)
            {
                SpawnMonster(data);
                data.hasSpawned = true;
            }
        }
    }

    private void SpawnMonster(MonsterSpawnData data)
    {
        GameObject monster = Instantiate(
            data.monsterPrefab,
            data.spawnPoint.position,
            Quaternion.identity
        );

        MonsterMove monsterMove = monster.GetComponent<MonsterMove>();

        if (monsterMove != null)
        {
            monsterMove.Init(player);
        }
    }
}