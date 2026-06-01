using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public GameObject heartItemPrefab;
    
    // key 오브젝트 설정
    public void HandleObject(PlayerContorl player, ObjectItem objectItem)
    {
        switch (objectItem.objectType)
        {
            case ObjectType.Key:
                HandleKey(player, objectItem);
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
    public void HandleCollisionObject(PlayerContorl player, ObjectItem objectItem)
    {
        switch (objectItem.objectType)
        {
            case ObjectType.Block:
                HandleBlock(player, objectItem);
                break;
        }
    }
    private void HandleKey(PlayerContorl player, ObjectItem objectItem)
    {
        player.AddKey(1);
        Destroy(objectItem.gameObject);
    }
    // block 오브젝트 설정
    private void HandleBlock(PlayerContorl player, ObjectItem objectItem)
    {
        bool usedKey = player.UseKey(1);

        if (usedKey == false)
        {
            return;
        }

        Vector3 spawnPosition = objectItem.transform.position;

        Destroy(objectItem.gameObject);

        if (heartItemPrefab != null)
        {
            StartCoroutine(SpawnHeartAndHeal(player, spawnPosition));
        }
        else
        {
            player.Heal(1);
        }
    }

    private IEnumerator SpawnHeartAndHeal(PlayerContorl player, Vector3 spawnPosition)
    {
        GameObject heart = Instantiate(heartItemPrefab, spawnPosition, Quaternion.identity);

        player.Heal(1);

        yield return new WaitForSeconds(0.5f);

        Destroy(heart);
    }

}
