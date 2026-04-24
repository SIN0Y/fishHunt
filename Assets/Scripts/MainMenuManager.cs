using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "SampleScene";

    [Header("Loading")]
    public GameObject loadingPanel;
    public Slider progressBar;

    void Start()
    {
        loadingPanel.SetActive(false);
        progressBar.value = 0f;
    }

    // PLAY
    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        loadingPanel.SetActive(true);
        progressBar.value = 0f;

        AsyncOperation load = SceneManager.LoadSceneAsync(gameSceneName);
        load.allowSceneActivation = false;

        float progress = 0f;

        while (!load.isDone)
        {
            float targetProgress = Mathf.Clamp01(load.progress / 0.9f);

            progress = Mathf.MoveTowards(progress, targetProgress, Time.deltaTime * 0.8f);
            progressBar.value = progress;

            if (load.progress >= 0.9f && progress >= 0.99f)
            {
                progressBar.value = 1f;
                load.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // QUIT
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}