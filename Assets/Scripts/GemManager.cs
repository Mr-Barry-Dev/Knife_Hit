
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;

    public int blueGems = 0;
    public int purpleGems = 0;

    [Header("UI (Multiple Panels Support)")]
    public TextMeshProUGUI[] blueGemTexts;
    public TextMeshProUGUI[] purpleGemTexts;

    [Header("Animation Settings")]
    public float animationDuration = 0.5f;

    int displayedBlueGems = 0;
    int displayedPurpleGems = 0;

    Coroutine blueAnim;
    Coroutine purpleAnim;

    void Awake()  
    {
        Instance = this;
        LoadGems();

        displayedBlueGems = blueGems;
        displayedPurpleGems = purpleGems;

        UpdateUIInstant();
    }

    // ================= ADD GEMS =================

    public void AddBlueGems(int amount)
    {
        blueGems += amount;
        SaveGems();

        if (blueAnim != null) StopCoroutine(blueAnim);
        blueAnim = StartCoroutine(AnimateBlueGems(displayedBlueGems, blueGems));
    }

    public void AddPurpleGems(int amount)
    {
        purpleGems += amount;
        SaveGems();

        if (purpleAnim != null) StopCoroutine(purpleAnim);
        purpleAnim = StartCoroutine(AnimatePurpleGems(displayedPurpleGems, purpleGems));
    }

    // ================= ANIMATION =================

    IEnumerator AnimateBlueGems(int start, int end)
    {
        float time = 0;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            displayedBlueGems = Mathf.RoundToInt(Mathf.Lerp(start, end, t));
            UpdateBlueUI();

            yield return null;
        }

        displayedBlueGems = end;
        UpdateBlueUI();
    }

    IEnumerator AnimatePurpleGems(int start, int end)
    {
        float time = 0;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            displayedPurpleGems = Mathf.RoundToInt(Mathf.Lerp(start, end, t));
            UpdatePurpleUI();

            yield return null;
        }

        displayedPurpleGems = end;
        UpdatePurpleUI();
    }

    // ================= 🔥 FORMAT FUNCTION =================

    string FormatNumber(int num)
    {
        if (num >= 1000000000)
            return (num / 1000000000f).ToString("0.#") + "B";
        else if (num >= 1000000)
            return (num / 1000000f).ToString("0.#") + "M";
        else if (num >= 1000)
            return (num / 1000f).ToString("0.#") + "K";
        else
            return num.ToString();
    }

    // ================= SAVE / LOAD =================

    void SaveGems()
    {
        PlayerPrefs.SetInt("BlueGems", blueGems);
        PlayerPrefs.SetInt("PurpleGems", purpleGems);
        PlayerPrefs.Save();
    }

    void LoadGems()
    {
        blueGems = PlayerPrefs.GetInt("BlueGems", 0);
        purpleGems = PlayerPrefs.GetInt("PurpleGems", 0);
    }

    // ================= UI =================

    void UpdateUIInstant()
    {
        UpdateBlueUI();
        UpdatePurpleUI();
    }

    void UpdateBlueUI()
    {
        if (blueGemTexts != null)
        {
            foreach (TextMeshProUGUI txt in blueGemTexts)
            {
                if (txt != null)
                    txt.text = FormatNumber(displayedBlueGems);
            }
        }
    }

    void UpdatePurpleUI()
    {
        if (purpleGemTexts != null)
        {
            foreach (TextMeshProUGUI txt in purpleGemTexts)
            {
                if (txt != null)
                    txt.text = FormatNumber(displayedPurpleGems);
            }
        }
    }

  
public void ResetGems()
{
    PlayerPrefs.DeleteKey("BlueGems");
    PlayerPrefs.DeleteKey("PurpleGems");

    blueGems = 0;
    purpleGems = 0;

    displayedBlueGems = 0;
    displayedPurpleGems = 0;

    UpdateUIInstant();

    PlayerPrefs.Save();

    
}

public void AddSkullReward()
{
    AddBlueGems(5);
}

}