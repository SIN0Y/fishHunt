using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FishingUIManager : MonoBehaviour
{
    [Header("Main UI")]
    public Text stateText;
    public Text scoreText;
    public Slider tensionSlider;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public Text fishNameText;
    public Text catchScoreText;
    public Text totalScoreText;
    public Image fishImage;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public Text gameOverScoreText;

    [Header("Extra Panel")]
    public GameObject panel; // assign panel here

    void Start()
    {
        resultPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        tensionSlider.gameObject.SetActive(false);

        if (panel != null)
            panel.SetActive(false);
    }

    public void ShowIdle()
    {
        stateText.text = "Press SPACE To Cast";
        tensionSlider.gameObject.SetActive(false);
    }

    public void ShowWaiting()
    {
        stateText.text = "Waiting For Bite...";
        tensionSlider.gameObject.SetActive(false);
    }

    public void ShowBite()
    {
        stateText.text = "Fish On! Press SPACE!";
        tensionSlider.gameObject.SetActive(false);
    }

    public void ShowReeling()
    {
        stateText.text = "Reeling...";
        tensionSlider.gameObject.SetActive(true);
    }

    public void UpdateSlider(float value)
    {
        tensionSlider.value = value / 100f;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    public void ShowResult(FishData fish, int fishScore, int totalScore)
    {
        tensionSlider.gameObject.SetActive(false);
        resultPanel.SetActive(true);

        fishNameText.text = fish.fishName;
        catchScoreText.text = "Catch Score : +" + fishScore;
        totalScoreText.text = "Total Score : " + totalScore;

        if (fishImage != null)
            fishImage.sprite = fish.icon;

        stateText.text = "Nice Catch!";
    }

    public void HideResult()
    {
        resultPanel.SetActive(false);
    }

    public void ShowGameOver(int score)
    {
        tensionSlider.gameObject.SetActive(false);

        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Total Score : " + score;

        stateText.text = "Failed!";
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    // PANEL ON/OFF BUTTON
    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}