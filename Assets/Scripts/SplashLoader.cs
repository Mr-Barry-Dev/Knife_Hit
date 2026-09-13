using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashLoader : MonoBehaviour
{
    [Header("UI")]
    public Slider loadingSlider;

    [Header("SETTINGS")]
    public float loadingTime = 3f; // total loading time

    [Header("SCENE")]
    public string nextSceneName = "Game"; // apna scene name yaha likho

    void Start()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        float timer = 0f;

        while (timer < loadingTime)
        {
            timer += Time.deltaTime;

            float progress = timer / loadingTime;

            if (loadingSlider != null)
                loadingSlider.value = progress;

            yield return null;
        }

        loadingSlider.value = 1f;

        // Scene load
        SceneManager.LoadScene(nextSceneName);
    }
}