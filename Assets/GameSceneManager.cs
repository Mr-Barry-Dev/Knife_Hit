using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void BackToHome()
    {
        PlayerPrefs.SetInt("OpenChallengePanel", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }
}