using UnityEngine;

// Inspector에서 몬스터별 스폰 정보를 설정하기 위한 데이터 클래스
[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab;
    public Transform spawnPoint;

    // 플레이어가 이 X 좌표 이상으로 이동하면 몬스터 생성
    public float spawnPlayerX;

    // 중복 스폰 방지용
    public bool hasSpawned;
}

// 플레이어 위치에 따라 몬스터를 한 번씩 생성하는 매니저
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

            // 플레이어가 지정된 위치까지 도달하면 몬스터 스폰
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

        // 생성된 몬스터에게 플레이어 정보를 넘겨 이동 방향을 초기화
        MonsterMove monsterMove = monster.GetComponent<MonsterMove>();

        if (monsterMove != null)
        {
            monsterMove.Init(player);
        }
    }
}