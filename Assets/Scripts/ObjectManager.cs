using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public GameObject heartItemPrefab;
    
    // key 오브젝트 설정
    public void HandleObject(PlayerStatus playerStaus, ObjectItem objectItem)
    {
        switch (objectItem.objectType)
        {
            case ObjectType.Key:
                HandleKey(playerStaus, objectItem);
                break;

            case ObjectType.Goal:
                HandleGoal();
                break;
        }
    }
    // 게임 종료
    private void HandleGoal()
    {
        Debug.Log("게임 클리어");
        GameResultData.CalculateScore();
        SceneManager.LoadScene("EndScene");
    }
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
    // block 오브젝트 설정
    private void HandleBlock(PlayerStatus playerStatus, ObjectItem objectItem)
    {
        bool usedKey = playerStatus.UseKey(1);

        if (usedKey == false)
        {
            return;
        }

        Vector3 spawnPosition = objectItem.transform.position;

        Destroy(objectItem.gameObject);

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
