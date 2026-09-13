using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneButtonGroup
    {
        public Button button1;
        public Button button2;

        public string sceneName;
    }

    public SceneButtonGroup[] buttonGroups;

    private void Start()
    {
        for (int i = 0; i < buttonGroups.Length; i++)
        {
            int index = i;

            if (buttonGroups[index].button1 != null)
            {
                buttonGroups[index].button1.onClick.AddListener(() =>
                {
                    LoadScene(buttonGroups[index].sceneName);
                });
            }

            if (buttonGroups[index].button2 != null)
            {
                buttonGroups[index].button2.onClick.AddListener(() =>
                {
                    LoadScene(buttonGroups[index].sceneName);
                });
            }
        }
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}