using UnityEngine;

public static class GameStats
{
    private static float startTime;
    private static bool timerRunning;
    
    private static int currentScore;
    private static float finalTime;

    private const int EnemyKillPoints = 100;
    private const int ObstacleJumpPoints = 50;

    public static void ResetStats()
    {
        currentScore = 0;
        finalTime = 0;
        timerRunning = false;
        Debug.Log("Game Stats Reset.");
    }
    
    public static void StartTimer()
    {
        if (!timerRunning)
        {
            startTime = Time.time;
            timerRunning = true;
            Debug.Log("Timer Started.");
        }
    }
    
    public static void StopTimer()
    {
        if (timerRunning)
        {
            finalTime = Time.time - startTime;
            timerRunning = false;
            Debug.Log($"Timer Stopped. Final Time: {finalTime:F2} seconds.");
        }
    }
    
    public static float GetCurrentTime()
    {
        return timerRunning ? Time.time - startTime : finalTime;
    }

    public static float GetFinalTime() => finalTime;
    public static int GetScore() => currentScore;
    
    public static void AddEnemyKillScore()
    {
        currentScore += EnemyKillPoints;
    }
    
    public static void AddObstacleJumpScore()
    {
        currentScore += ObstacleJumpPoints;
    }
}
