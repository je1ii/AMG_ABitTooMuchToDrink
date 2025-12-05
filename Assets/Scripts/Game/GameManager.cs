using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private string endSceneName;
    
    [Header("References")] 
    [SerializeField] private PlayerMovement playerMovement;

    private bool gameFinished = false;

    void Start()
    {
        GameStats.ResetStats();
        GameStats.StartTimer();
        gameFinished = false;
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.GetIsPlayerWon() && !gameFinished)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool isWin)
    {
        if (gameFinished) return;
        gameFinished = true;

        GameStats.StopTimer();

        Debug.Log("Game finished. Loading Results Scene...");
        SceneManager.LoadScene(endSceneName);
    }
}
