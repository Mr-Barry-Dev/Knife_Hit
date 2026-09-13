// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;

// public class RandomPickReward : MonoBehaviour
// {
//     [Header("Cards")]
//     public Button[] buttons;

//     [Header("Diamond UI")]
//     public Image[] diamondImages;
//     public TMP_Text[] rewardTexts;

//     [Header("Sprites")]
//     public Sprite redHeartSprite;
//     public Sprite skullSprite;

//     [Header("Buttons")]
//     public Button collectButton;
//     public Button doubleCollectButton;
//     public Button goBackButton;

//     [Header("Red Heart Reward Range")]
//     public int redHeartMin = 50;
//     public int redHeartMax = 200;

//     [Header("Skull Reward Range")]
//     public int skullMin = 10;
//     public int skullMax = 20;

//     [Header("Skull Chance (%)")]
//     [Range(0, 100)]
//     public int skullChance = 8;

//     [Header("Sounds")]
//     public AudioSource audioSource;
//     public AudioClip pickSound;
//     public AudioClip collectSound;
//     public AudioClip doubleCollectSound;
//     public AudioClip goBackSound;

//     private int[] rewards;
//     private int[] rewardTypes; // 0 = RedHeart, 1 = Skull

//     private bool hasPicked = false;
//     private int selectedReward = 0;
//     private int selectedType = 0;

//     private ColorBlock[] defaultColors;

//     void Start()
//     {
//         rewards = new int[buttons.Length];
//         rewardTypes = new int[buttons.Length];
//         defaultColors = new ColorBlock[buttons.Length];

//         // Sab buttons hidden at start
//         collectButton.gameObject.SetActive(false);
//         doubleCollectButton.gameObject.SetActive(false);
//         goBackButton.gameObject.SetActive(false);

//         // Scale zero karo taaki animation se aayein
//         collectButton.transform.localScale = Vector3.zero;
//         doubleCollectButton.transform.localScale = Vector3.zero;
//         goBackButton.transform.localScale = Vector3.zero;

//         GenerateAllRewards();

//         for (int i = 0; i < buttons.Length; i++)
//         {
//             diamondImages[i].gameObject.SetActive(false);
//             rewardTexts[i].text = "";
//             defaultColors[i] = buttons[i].colors;

//             int index = i;
//             buttons[i].onClick.AddListener(() => OnPick(index));
//         }

//         collectButton.onClick.AddListener(OnCollect);
//         doubleCollectButton.onClick.AddListener(OnDoubleCollect);
//         goBackButton.onClick.AddListener(OnGoBack);
//     }

//     void GenerateAllRewards()
//     {
//         for (int i = 0; i < buttons.Length; i++)
//         {
//             GenerateReward(i);
//         }
//     }

//     void GenerateReward(int i)
//     {
//         int roll = Random.Range(0, 100);

//         if (roll < skullChance)
//         {
//             rewardTypes[i] = 1;
//             rewards[i] = Random.Range(skullMin, skullMax + 1);
//         }
//         else
//         {
//             rewardTypes[i] = 0;
//             rewards[i] = Random.Range(redHeartMin, redHeartMax + 1);
//         }
//     }

//     void OnPick(int index)
//     {
//         if (hasPicked) return;

//         hasPicked = true;
//         selectedReward = rewards[index];
//         selectedType = rewardTypes[index];

//         if (audioSource && pickSound)
//             audioSource.PlayOneShot(pickSound);

//         DisableAllButtonsVisual();
//         StartCoroutine(FlipCard(index));
//     }

//     IEnumerator FlipCard(int index)
//     {
//         Transform card = buttons[index].transform;
//         float duration = 0.25f;
//         float time = 0;

//         // Scale X → 0
//         while (time < duration)
//         {
//             float scale = Mathf.Lerp(1f, 0f, time / duration);
//             card.localScale = new Vector3(scale, 1f, 1f);
//             time += Time.deltaTime;
//             yield return null;
//         }
//         card.localScale = new Vector3(0f, 1f, 1f);

//         // Reward dikhao
//         diamondImages[index].gameObject.SetActive(true);
//         diamondImages[index].sprite = (selectedType == 0) ? redHeartSprite : skullSprite;
//         rewardTexts[index].text = selectedReward.ToString();

//         time = 0;

//         // Scale X → 1
//         while (time < duration)
//         {
//             float scale = Mathf.Lerp(0f, 1f, time / duration);
//             card.localScale = new Vector3(scale, 1f, 1f);
//             time += Time.deltaTime;
//             yield return null;
//         }
//         card.localScale = Vector3.one;

//         // Collect button pop-in show karo
//         yield return StartCoroutine(ShowButtonPopIn(collectButton));
//     }

//     // ─── POP-IN ANIMATION ───────────────────────────────────────────────────────
//     IEnumerator ShowButtonPopIn(Button btn)
//     {
//         btn.gameObject.SetActive(true);
//         btn.interactable = true;
//         btn.transform.localScale = Vector3.zero;

//         float duration = 0.3f;
//         float time = 0;

//         while (time < duration)
//         {
//             float t = time / duration;
//             // Overshoot spring feel
//             float scale = Mathf.LerpUnclamped(0f, 1f, EaseOutBack(t));
//             btn.transform.localScale = Vector3.one * scale;
//             time += Time.deltaTime;
//             yield return null;
//         }

//         btn.transform.localScale = Vector3.one;
//     }

//     IEnumerator HideButtonPopOut(Button btn)
//     {
//         float duration = 0.2f;
//         float time = 0;

//         while (time < duration)
//         {
//             float t = time / duration;
//             float scale = Mathf.Lerp(1f, 0f, t);
//             btn.transform.localScale = Vector3.one * scale;
//             time += Time.deltaTime;
//             yield return null;
//         }

//         btn.transform.localScale = Vector3.zero;
//         btn.gameObject.SetActive(false);
//     }

//     float EaseOutBack(float t)
//     {
//         float c1 = 1.70158f;
//         float c3 = c1 + 1f;
//         return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
//     }

//     // ─── COLLECT ────────────────────────────────────────────────────────────────
//     void OnCollect()
//     {
//         collectButton.interactable = false;

//         if (audioSource && collectSound)
//             audioSource.PlayOneShot(collectSound);

//         // Reward add karo
//         if (selectedType == 0)
//             GemManager.Instance.AddBlueGems(selectedReward);
//         else
//             GemManager.Instance.AddPurpleGems(selectedReward);

//         StartCoroutine(CollectSequence());
//     }

//     IEnumerator CollectSequence()
//     {
//         // Collect button hide
//         yield return StartCoroutine(HideButtonPopOut(collectButton));

//         // Skull pe double collect nahi milega (optional — hatana ho to yeh block hata do)
//         if (selectedType == 1)
//         {
//             // Skull mila → seedha goback
//             yield return new WaitForSeconds(1f);
//             yield return StartCoroutine(ShowButtonPopIn(goBackButton));
//             yield break;
//         }

//         // Double Collect button show
//         yield return StartCoroutine(ShowButtonPopIn(doubleCollectButton));
//     }

//     // ─── DOUBLE COLLECT ─────────────────────────────────────────────────────────
//     void OnDoubleCollect()
//     {
//         doubleCollectButton.interactable = false;

//         if (audioSource && doubleCollectSound)
//             audioSource.PlayOneShot(doubleCollectSound);

//         // Double reward add karo (sirf RedHeart)
//         if (selectedType == 0)
//             GemManager.Instance.AddBlueGems(selectedReward); // pehle wala already add hua, ab double ka extra

//         StartCoroutine(DoubleCollectSequence());
//     }

//     IEnumerator DoubleCollectSequence()
//     {
//         // Double Collect button hide
//         yield return StartCoroutine(HideButtonPopOut(doubleCollectButton));

//         // 1 sec wait
//         yield return new WaitForSeconds(1f);

//         // GoBack button show
//         yield return StartCoroutine(ShowButtonPopIn(goBackButton));
//     }

//     // ─── GO BACK ────────────────────────────────────────────────────────────────
//     void OnGoBack()
//     {
//         if (audioSource && goBackSound)
//             audioSource.PlayOneShot(goBackSound);

//         StartCoroutine(GoBackSequence());
//     }

//     IEnumerator GoBackSequence()
//     {
//         yield return StartCoroutine(HideButtonPopOut(goBackButton));
//         ResetSystem();
//     }

//     // ─── HELPERS ────────────────────────────────────────────────────────────────
//     void DisableAllButtonsVisual()
//     {
//         foreach (Button b in buttons)
//         {
//             b.interactable = false;
//             ColorBlock cb = b.colors;
//             cb.normalColor = new Color(0.6f, 0.6f, 0.6f);
//             cb.disabledColor = new Color(0.6f, 0.6f, 0.6f);
//             b.colors = cb;
//         }
//     }

//     void ResetSystem()
//     {
//         hasPicked = false;

//         GenerateAllRewards();

//         for (int i = 0; i < buttons.Length; i++)
//         {
//             diamondImages[i].gameObject.SetActive(false);
//             rewardTexts[i].text = "";
//             buttons[i].interactable = true;
//             buttons[i].colors = defaultColors[i];
//             buttons[i].transform.localScale = Vector3.one;
//         }

//         collectButton.gameObject.SetActive(false);
//         collectButton.interactable = true;
//         collectButton.transform.localScale = Vector3.zero;

//         doubleCollectButton.gameObject.SetActive(false);
//         doubleCollectButton.interactable = true;
//         doubleCollectButton.transform.localScale = Vector3.zero;

//         goBackButton.gameObject.SetActive(false);
//         goBackButton.transform.localScale = Vector3.zero;
//     }
// }


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RandomPickReward : MonoBehaviour
{
    [Header("Cards")]
    public Button[] buttons;

    [Header("Diamond UI")]
    public Image[] diamondImages;
    public TMP_Text[] rewardTexts;

    [Header("Sprites")]
    public Sprite redHeartSprite;
    public Sprite skullSprite;

    [Header("Buttons")]
    public Button collectButton;
    public Button doubleCollectButton;
    public Button goBackButton;

    [Header("Double Collect Text")]
    public TMP_Text doubleCollectText;

    [Header("Auto Toggle Delay")]
    public float toggleDelay = 2f;

    [Header("Red Heart Reward Range")]
    public int redHeartMin = 50;
    public int redHeartMax = 200;

    [Header("Skull Reward Range")]
    public int skullMin = 10;
    public int skullMax = 20;

    [Header("Skull Chance (%)")]
    [Range(0, 100)]
    public int skullChance = 8;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip pickSound;
    public AudioClip collectSound;
    public AudioClip doubleCollectSound;
    public AudioClip goBackSound;

    private int[] rewards;
    private int[] rewardTypes;

    private bool hasPicked = false;
    private int selectedReward = 0;
    private int selectedType = 0;

    private ColorBlock[] defaultColors;

    private Coroutine toggleLoopCoroutine;
    private bool rewardCollected = false;

    void Start()
    {
        rewards = new int[buttons.Length];
        rewardTypes = new int[buttons.Length];
        defaultColors = new ColorBlock[buttons.Length];

        collectButton.gameObject.SetActive(false);
        doubleCollectButton.gameObject.SetActive(false);
        goBackButton.gameObject.SetActive(false);

        collectButton.transform.localScale = Vector3.zero;
        doubleCollectButton.transform.localScale = Vector3.zero;
        goBackButton.transform.localScale = Vector3.zero;

        if (doubleCollectText != null)
            doubleCollectText.text = "";

        GenerateAllRewards();

        for (int i = 0; i < buttons.Length; i++)
        {
            diamondImages[i].gameObject.SetActive(false);
            rewardTexts[i].text = "";
            defaultColors[i] = buttons[i].colors;

            int index = i;
            buttons[i].onClick.AddListener(() => OnPick(index));
        }

        collectButton.onClick.AddListener(OnCollect);
        doubleCollectButton.onClick.AddListener(OnDoubleCollect);
        goBackButton.onClick.AddListener(OnGoBack);
    }

    // ─── REWARD GENERATE ────────────────────────────────────────────────────────

    void GenerateAllRewards()
    {
        for (int i = 0; i < buttons.Length; i++)
            GenerateReward(i);
    }

    void GenerateReward(int i)
    {
        int roll = Random.Range(0, 100);

        if (roll < skullChance)
        {
            rewardTypes[i] = 1;
            rewards[i] = Random.Range(skullMin, skullMax + 1);
        }
        else
        {
            rewardTypes[i] = 0;
            rewards[i] = Random.Range(redHeartMin, redHeartMax + 1);
        }
    }

    // ─── CARD PICK ──────────────────────────────────────────────────────────────

    void OnPick(int index)
    {
        if (hasPicked) return;

        hasPicked = true;
        rewardCollected = false;
        selectedReward = rewards[index];
        selectedType = rewardTypes[index];

        if (audioSource && pickSound)
            audioSource.PlayOneShot(pickSound);

        DisableAllButtonsVisual();
        StartCoroutine(FlipCard(index));
    }

    IEnumerator FlipCard(int index)
    {
        Transform card = buttons[index].transform;
        float duration = 0.25f;
        float time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, time / duration);
            card.localScale = new Vector3(scale, 1f, 1f);
            time += Time.deltaTime;
            yield return null;
        }
        card.localScale = new Vector3(0f, 1f, 1f);

        diamondImages[index].gameObject.SetActive(true);
        diamondImages[index].sprite = (selectedType == 0) ? redHeartSprite : skullSprite;
        rewardTexts[index].text = selectedReward.ToString();

        time = 0;
        while (time < duration)
        {
            float scale = Mathf.Lerp(0f, 1f, time / duration);
            card.localScale = new Vector3(scale, 1f, 1f);
            time += Time.deltaTime;
            yield return null;
        }
        card.localScale = Vector3.one;

        // Double collect text set kar do pehle se
        if (doubleCollectText != null)
            doubleCollectText.text = "2X" + selectedReward.ToString();

        // Toggle loop shuru karo
        toggleLoopCoroutine = StartCoroutine(ToggleLoop());
    }

    // ─── TOGGLE LOOP: Collect ↔ Double Collect ──────────────────────────────────

    IEnumerator ToggleLoop()
    {
        // Skull ko sirf collect dikhao, double nahi
        if (selectedType == 1)
        {
            yield return StartCoroutine(ShowButtonPopIn(collectButton));
            yield break; // loop nahi, bas collect dikhao
        }

        while (!rewardCollected)
        {
            // Collect show karo
            yield return StartCoroutine(ShowButtonPopIn(collectButton));

            if (rewardCollected) break;

            // toggleDelay tak wait karo, agar press hua to loop break
            float waited = 0f;
            while (waited < toggleDelay)
            {
                if (rewardCollected) break;
                waited += Time.deltaTime;
                yield return null;
            }

            if (rewardCollected) break;

            // Collect hide karo
            yield return StartCoroutine(HideButtonPopOut(collectButton));

            if (rewardCollected) break;

            // Double Collect show karo
            yield return StartCoroutine(ShowButtonPopIn(doubleCollectButton));

            if (rewardCollected) break;

            // toggleDelay tak wait karo
            waited = 0f;
            while (waited < toggleDelay)
            {
                if (rewardCollected) break;
                waited += Time.deltaTime;
                yield return null;
            }

            if (rewardCollected) break;

            // Double Collect hide karo
            yield return StartCoroutine(HideButtonPopOut(doubleCollectButton));
        }
    }

    // ─── COLLECT ────────────────────────────────────────────────────────────────

    void OnCollect()
    {
        if (rewardCollected) return;
        rewardCollected = true;

        if (toggleLoopCoroutine != null)
        {
            StopCoroutine(toggleLoopCoroutine);
            toggleLoopCoroutine = null;
        }

        collectButton.interactable = false;

        if (audioSource && collectSound)
            audioSource.PlayOneShot(collectSound);

        if (selectedType == 0)
            GemManager.Instance.AddBlueGems(selectedReward);
        else
            GemManager.Instance.AddPurpleGems(selectedReward);

        StartCoroutine(AfterCollect());
    }

    IEnumerator AfterCollect()
    {
        yield return StartCoroutine(HideButtonPopOut(collectButton));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowButtonPopIn(goBackButton));
    }

    // ─── DOUBLE COLLECT ─────────────────────────────────────────────────────────

    void OnDoubleCollect()
    {
        if (rewardCollected) return;
        rewardCollected = true;

        if (toggleLoopCoroutine != null)
        {
            StopCoroutine(toggleLoopCoroutine);
            toggleLoopCoroutine = null;
        }

        doubleCollectButton.interactable = false;

        if (audioSource && doubleCollectSound)
            audioSource.PlayOneShot(doubleCollectSound);

        // Pehle wala + double = total double reward
        if (selectedType == 0)
            GemManager.Instance.AddBlueGems(selectedReward * 2);

        StartCoroutine(AfterDoubleCollect());
    }

    IEnumerator AfterDoubleCollect()
    {
        yield return StartCoroutine(HideButtonPopOut(doubleCollectButton));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowButtonPopIn(goBackButton));
    }

    // ─── GO BACK ────────────────────────────────────────────────────────────────

    void OnGoBack()
    {
        if (audioSource && goBackSound)
            audioSource.PlayOneShot(goBackSound);

        StartCoroutine(GoBackSequence());
    }

    IEnumerator GoBackSequence()
    {
        yield return StartCoroutine(HideButtonPopOut(goBackButton));
        ResetSystem();
    }

    // ─── ANIMATIONS ─────────────────────────────────────────────────────────────

    IEnumerator ShowButtonPopIn(Button btn)
    {
        btn.gameObject.SetActive(true);
        btn.interactable = true;
        btn.transform.localScale = Vector3.zero;

        float duration = 0.3f;
        float time = 0;

        while (time < duration)
        {
            float t = time / duration;
            float scale = Mathf.LerpUnclamped(0f, 1f, EaseOutBack(t));
            btn.transform.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        btn.transform.localScale = Vector3.one;
    }

    IEnumerator HideButtonPopOut(Button btn)
    {
        btn.interactable = false;

        float duration = 0.2f;
        float time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, time / duration);
            btn.transform.localScale = Vector3.one * scale;
            time += Time.deltaTime;
            yield return null;
        }

        btn.transform.localScale = Vector3.zero;
        btn.gameObject.SetActive(false);
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    // ─── HELPERS ────────────────────────────────────────────────────────────────

    void DisableAllButtonsVisual()
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.6f, 0.6f, 0.6f);
            cb.disabledColor = new Color(0.6f, 0.6f, 0.6f);
            b.colors = cb;
        }
    }

    void ResetSystem()
    {
        hasPicked = false;
        rewardCollected = false;

        GenerateAllRewards();

        for (int i = 0; i < buttons.Length; i++)
        {
            diamondImages[i].gameObject.SetActive(false);
            rewardTexts[i].text = "";
            buttons[i].interactable = true;
            buttons[i].colors = defaultColors[i];
            buttons[i].transform.localScale = Vector3.one;
        }

        collectButton.gameObject.SetActive(false);
        collectButton.interactable = true;
        collectButton.transform.localScale = Vector3.zero;

        doubleCollectButton.gameObject.SetActive(false);
        doubleCollectButton.interactable = true;
        doubleCollectButton.transform.localScale = Vector3.zero;

        goBackButton.gameObject.SetActive(false);
        goBackButton.transform.localScale = Vector3.zero;

        if (doubleCollectText != null)
            doubleCollectText.text = "";
    }
}