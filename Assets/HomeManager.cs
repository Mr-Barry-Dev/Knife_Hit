// using UnityEngine;
// using System.Collections;

// public class HomeManager : MonoBehaviour
// {
//     public GameObject challengePanel;

//     private IEnumerator Start()
//     {
//         yield return null;

//         if (PlayerPrefs.GetInt("OpenChallengePanel", 0) == 1)
//         {
//             challengePanel.SetActive(true);

//             PlayerPrefs.SetInt("OpenChallengePanel", 0);
//             PlayerPrefs.Save();
//         }
//     }
// }


using UnityEngine;
using System.Collections;

public class HomeManager : MonoBehaviour
{
    public GameObject challengePanel;

    void Start()
    {
        StartCoroutine(HandlePanel());
    }

    IEnumerator HandlePanel()
    {
        yield return null;

        if (PlayerPrefs.GetInt("OpenChallengePanel", 0) == 1)
        {
            challengePanel.SetActive(true);
            PlayerPrefs.SetInt("OpenChallengePanel", 0);
        }
        else
        {
            challengePanel.SetActive(false);
        }
    }
}