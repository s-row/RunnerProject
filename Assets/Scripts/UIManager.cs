using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image[] hearts;

    public Sprite fullHeart;   // 빨간 하트
    public Sprite emptyHeart;  // 회색 하트

    public TMP_Text keyText;

    public void UpdateHeart(int hp)
    {
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
