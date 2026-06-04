using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// 아이템, 블록, 골인 지점 같은 오브젝트 처리를 담당
public class ObjectManager : MonoBehaviour
{
    public GameObject heartItemPrefab;

    // Trigger 오브젝트 처리: Key, Goal
    public void HandleObject(PlayerStatus playerStatus, ObjectItem objectItem)
    {
        switch (objectItem.objectType)
        {
            case ObjectType.Key:
                HandleKey(playerStatus, objectItem);
                break;

            case ObjectType.Goal:
                HandleGoal();
                break;
        }
    }

    private void HandleGoal()
    {
        Debug.Log("Game clear");

        GameResultData.CalculateScore();
        SceneManager.LoadScene("EndScene");
    }

    // Collision 오브젝트 처리: Block
    public void HandleCollisionObject(PlayerStatus playerStatus, ObjectItem objectItem)
    {
        switch (objectItem.objectType)
        {
            case ObjectType.Block:
                HandleBlock(playerStatus, objectItem);
                break;
        }
    }

    private void HandleKey(PlayerStatus playerStatus, ObjectItem objectItem)
    {
        playerStatus.AddKey(1);
        Destroy(objectItem.gameObject);
    }

    private void HandleBlock(PlayerStatus playerStatus, ObjectItem objectItem)
    {
        // 열쇠가 없으면 블록을 제거하지 않음
        bool usedKey = playerStatus.UseKey(1);

        if (usedKey == false)
        {
            return;
        }

        Vector3 spawnPosition = objectItem.transform.position;

        Destroy(objectItem.gameObject);

        // 하트 프리팹이 있으면 잠깐 보여준 뒤 회복, 없으면 바로 회복
        if (heartItemPrefab != null)
        {
            StartCoroutine(SpawnHeartAndHeal(playerStatus, spawnPosition));
        }
        else
        {
            playerStatus.Heal(1);
        }
    }

    private IEnumerator SpawnHeartAndHeal(PlayerStatus playerStatus, Vector3 spawnPosition)
    {
        GameObject heart = Instantiate(heartItemPrefab, spawnPosition, Quaternion.identity);

        playerStatus.Heal(1);

        yield return new WaitForSeconds(0.5f);

        Destroy(heart);
    }
}