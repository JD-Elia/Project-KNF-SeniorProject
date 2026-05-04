using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private TextMeshProUGUI gamesOverScoreUI;
    [SerializeField] private TextMeshProUGUI gamesOverHighscoreUI;

    GameManager gm;
    
    private void Start() {
        gm = GameManager.Instance;
        gm.onGameOver.AddListener(ActivateGameOverUI);
    }

    public void ActivateGameOverUI() {
        gameOverUI.SetActive(true);

        gamesOverScoreUI.text = "Score: " + gm.prettyScore();
        gamesOverHighscoreUI.text = "Highscore: " + gm.prettyHighscore();
    }

    public void ReplayHandler() {
        SceneManager.LoadScene(1);
    }

    public void BackToMenuHandler() {
        SceneManager.LoadScene(0);
    }

    private void OnGUI() {
        scoreUI.text = gm.prettyScore();
    }
}