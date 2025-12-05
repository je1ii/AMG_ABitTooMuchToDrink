 using UnityEngine;
 using TMPro;
 using UnityEngine.SceneManagement;

 public class EndGameManager : MonoBehaviour
 { 
     [Header("Results Screen")]
     [SerializeField] private TextMeshProUGUI finalTimeText;
     [SerializeField] private TextMeshProUGUI finalScoreText;
     
     [Header("Game Scene Name")]
     [SerializeField] private string gameSceneName;

     void Start()
     {
         DisplayFinalResults();
     }
    
     private void DisplayFinalResults()
     {
         float finalTime = GameStats.GetFinalTime();
         int finalScore = GameStats.GetScore();

         if (finalScoreText != null)
         {
             finalScoreText.text = $"{finalScore}";
         }
         if (finalTimeText != null)
         {
             finalTimeText.text = $"{finalTime:F2}s";
         }
        
         Debug.Log($"Results Displayed: Score {finalScore}, Time {finalTime:F2}s");
     }
     
     public void TryAgain()
     {
         GameStats.ResetStats(); 
        
         Debug.Log("Restarting game...");
         SceneManager.LoadScene(gameSceneName);
     }
 }
