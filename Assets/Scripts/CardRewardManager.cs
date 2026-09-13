using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardRewardManager : MonoBehaviour
{
    public static CardRewardManager Instance;

    [Header("MAIN PANEL")]
    public GameObject cardPanel;

    [Header("CARDS (Assign 4 manually)")]
    public RectTransform[] cards;

    [Header("UI")]
    public Image[] diamondImages;
    public TMP_Text[] rewardTexts;

    [Header("SPRITE")]
    public Sprite blueDiamondSprite;

    private int currentCardIndex;

    // 👉 rewards store karne ke liye
    private int[] storedRewards = new int[4];

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cardPanel.SetActive(false);

        // 🔥 LOAD INDEX
        currentCardIndex = PlayerPrefs.GetInt("CARD_INDEX", 0);

        // 🔥 LOAD OLD REWARDS
        for (int i = 0; i < 4; i++)
        {
            storedRewards[i] = PlayerPrefs.GetInt("CARD_REWARD_" + i, -1);
        }

        SetupInitialState();
    }

    // ================= SHOW =================

    public void ShowCardsAfterWin()
    {
        StartCoroutine(CardFlow());
    }

    IEnumerator CardFlow()
    {
        yield return new WaitForSecondsRealtime(1f);

        cardPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(0.2f);

        // 👉 new reward
        int reward = Random.Range(0, 101);

        storedRewards[currentCardIndex] = reward;

        // 👉 SAVE
        PlayerPrefs.SetInt("CARD_REWARD_" + currentCardIndex, reward);

        yield return StartCoroutine(FlipCard(currentCardIndex, reward));

        currentCardIndex++;

        // 👉 SAVE INDEX
        PlayerPrefs.SetInt("CARD_INDEX", currentCardIndex);

        // 👉 RESET AFTER 4
        if (currentCardIndex >= 4)
        {
            yield return new WaitForSecondsRealtime(1f);

            ResetAllCards();

            currentCardIndex = 0;
            PlayerPrefs.SetInt("CARD_INDEX", 0);

            // 🔥 CLEAR OLD DATA
            for (int i = 0; i < 4; i++)
            {
                PlayerPrefs.DeleteKey("CARD_REWARD_" + i);
                storedRewards[i] = -1;
            }
        }

        PlayerPrefs.Save();
    }

    // ================= FLIP =================

    IEnumerator FlipCard(int index, int reward)
    {
        RectTransform card = cards[index];

        float duration = 0.25f;
        float t = 0;

        // shrink
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float scaleX = Mathf.Lerp(1f, 0f, t / duration);
            card.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        // reveal
        diamondImages[index].sprite = blueDiamondSprite;
        rewardTexts[index].text = reward.ToString();

        GemManager.Instance.AddBlueGems(reward);

        // expand
        t = 0;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float scaleX = Mathf.Lerp(0f, 1f, t / duration);
            card.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        card.localScale = Vector3.one;
    }

    // ================= INITIAL =================

    void SetupInitialState()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].localScale = Vector3.one;

            if (storedRewards[i] != -1)
            {
                // 👉 already opened cards show karo
                rewardTexts[i].text = storedRewards[i].ToString();
                diamondImages[i].sprite = blueDiamondSprite;
            }
            else
            {
                rewardTexts[i].text = "";
            }
        }
    }

    // ================= RESET =================

    void ResetAllCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            rewardTexts[i].text = "";
            cards[i].localScale = Vector3.one;
        }
    }
}