using UnityEngine;

public static class GameResultData
{
    public static int hitCount = 0;
    public static int finalScore = 0;

    public static void Reset()
    {
        hitCount = 0;
        finalScore = 0;
    }

    public static void AddHit()
    {
        hitCount++;
    }

    public static void CalculateScore()
    {
        finalScore = 10 - hitCount * 2;

        if (finalScore < 0)
        {
            finalScore = 0;
        }
    }
}
