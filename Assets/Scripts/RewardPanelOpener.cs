
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RewardPanelOpener : MonoBehaviour
{
    [Header("Panels")]
    public GameObject currentPanel;
    public GameObject rewardPanel;

    [Header("Costs")]
    public int blueCost1 = 250;
    public int blueCost2 = 500;
    public int purpleCost = 150;

    [Header("Popup Message")]
    public GameObject messagePanel;
    public TMP_Text messageText;

    [Header("Buttons")]
    public Button openButton1; // 250 Blue
    public Button openButton2; // 500 Blue
    public Button openButton3; // 150 Purple
    public Button closeButton;

    [Header("Bonus Reward")]
    public Button bonusButton;
    public int bonusAmount = 40;

    public enum GemType
    {
        Blue,
        Purple
    }

    public GemType bonusGemType = GemType.Blue;

    void Start()
    {
        rewardPanel.SetActive(false);
        messagePanel.SetActive(false);

        openButton1.onClick.AddListener(() => OpenWithBlue(blueCost1));
        openButton2.onClick.AddListener(() => OpenWithBlue(blueCost2));
        openButton3.onClick.AddListener(OpenWithPurple);

        closeButton.onClick.AddListener(ClosePanel);

        if (bonusButton != null)
            bonusButton.onClick.AddListener(GiveBonus);
    }

    // 🔵 BLUE OPEN (250 / 500)
    void OpenWithBlue(int cost)
    {
        if (GemManager.Instance.blueGems >= cost)
        {
            GemManager.Instance.AddBlueGems(-cost);
            StartCoroutine(OpenAnim());
        }
        else
        {
            ShowMessage("Not enough Blue Gems!");
        }
    }

    // 🟣 PURPLE OPEN (150)
    void OpenWithPurple()
    {
        if (GemManager.Instance.purpleGems >= purpleCost)
        {
            GemManager.Instance.AddPurpleGems(-purpleCost);
            StartCoroutine(OpenAnim());
        }
        else
        {
            ShowMessage("Not enough Purple Gems!");
        }
    }

    // 🔓 OPEN PANEL
    IEnumerator OpenAnim()
    {
        Transform cur = currentPanel.transform;

        float time = 0;
        float duration = 0.25f;

        while (time < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, time / duration);
            cur.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        currentPanel.SetActive(false);

        rewardPanel.SetActive(true);

        Transform panel = rewardPanel.transform;
        panel.localScale = Vector3.zero;

        time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(0f, 1f, time / duration);
            panel.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        panel.localScale = Vector3.one;
    }

    // 🔒 CLOSE PANEL
    void ClosePanel()
    {
        StartCoroutine(CloseAnim());
    }

    IEnumerator CloseAnim()
    {
        Transform panel = rewardPanel.transform;

        float time = 0;
        float duration = 0.25f;

        while (time < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, time / duration);
            panel.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        rewardPanel.SetActive(false);

        currentPanel.SetActive(true);

        Transform cur = currentPanel.transform;
        cur.localScale = Vector3.zero;

        time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(0f, 1f, time / duration);
            cur.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        cur.localScale = Vector3.one;
    }

    // 🎁 BONUS
    void GiveBonus()
    {
        if (bonusGemType == GemType.Blue)
        {
            GemManager.Instance.AddBlueGems(bonusAmount);
        }
        else
        {
            GemManager.Instance.AddPurpleGems(bonusAmount);
        }
    }

    // 💬 POPUP
    void ShowMessage(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(PopupAnim(msg));
    }

    IEnumerator PopupAnim(string msg)
    {
        messagePanel.SetActive(true);
        messageText.text = msg;

        Transform panel = messagePanel.transform;
        panel.localScale = Vector3.zero;

        float time = 0;
        float duration = 0.2f;

        while (time < duration)
        {
            float scale = Mathf.Lerp(0f, 1.2f, time / duration);
            panel.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        panel.localScale = Vector3.one;

        yield return new WaitForSeconds(2f);

        time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, time / duration);
            panel.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        messagePanel.SetActive(false);
    }
}