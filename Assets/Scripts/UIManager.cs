using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 하트 체력 UI와 열쇠 개수 UI를 갱신
public class UIManager : MonoBehaviour
{
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TMP_Text keyText;

    public void UpdateHeart(int hp)
    {
        // 현재 HP보다 작은 인덱스는 빨간 하트, 나머지는 회색 하트로 표시
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void UpdateKeyCount(int keyCount)
    {
        keyText.text = keyCount.ToString();
    }
}