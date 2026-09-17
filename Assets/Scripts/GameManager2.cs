using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager2 : MonoBehaviour
{
    [Header("===== GAME MANAGER =====")]
    public GameManager gameManager;

    [Header("===== CIRCLE =====")]
    public SpriteRenderer circleRenderer;

    // =========================================================================
    // =================== CIRCLE SKINS - SPLIT INTO 2 RANDOM GROUPS ==========
    // =========================================================================

    [System.Serializable]
    public class SkinEntry
    {
        [Tooltip("Circle ki sprite")]
        public Sprite skin;

        [Tooltip("Is skin ke saath is stage mein kitni knives honi chahiye")]
        public int knifeCount;
    }

    [Header("===== CIRCLE SKINS - RANDOM GROUP 1 (TOP SKINS, STAGE 1-3) =====")]
    [Tooltip("Pehle 3 stages mein inme se koi bhi entry random order mein aayegi (jaise skin2, skin3, skin1) — har entry ka apna knife count hoga")]
    public SkinEntry[] topCircleSkins;

    [Header("===== CIRCLE SKINS - RANDOM GROUP 2 (BAAKI SKINS, STAGE 4+) =====")]
    [Tooltip("Stage 3 ke baad har stage mein inme se ek random entry spawn hogi — har entry ka apna knife count hoga")]
    public SkinEntry[] remainingCircleSkins;

    [Header("===== TOTAL STAGES =====")]
    [Tooltip("Total kitne stages honge. Progress dots, circleHitEffects sab isi count ke hisaab se kaam karenge (skin selection se independent)")]
    public int totalStages = 10;

    // Group1 aur Group2 dono ka shuffled order (har group ke saare elements
    // ek-ek baar, random order mein, repeat nahi hoga jab tak group khatam na ho)
    private int[] topSkinOrder;
    private int[] remainingSkinOrder;

    // Skin shuffle ke liye alag, independent Random — UnityEngine.Random se
    // bilkul alag, GUID se seed hota hai (bahut zyada entropy), isliye koi
    // aur script iske Random calls ko interfere/predictable nahi bana sakta
    private System.Random skinShuffleRng;

    // Current stage ke liye jo entry select hui thi, wo cache — knife count isi se milega
    private SkinEntry currentSkinEntry;

    [System.Serializable]
    public class CircleHitEffect
    {
        public GameObject hitEffect;
        public AudioClip hitSound;
    }

    [Header("===== ROTATION SCRIPTS =====")]
    public MonoBehaviour[] rotationScripts;
    private int[] rotationOrder;

    [Header("===== CIRCLE HIT EFFECTS =====")]
    public CircleHitEffect[] circleHitEffects;

    [Header("===== AUDIO =====")]
    public AudioSource audioSource;

    [Header("===== CHALLENGE TEXTS =====")]
    public TextMeshProUGUI[] challengeTexts;

    [Header("===== CURRENT CHALLENGE =====")]
    public int currentChallenge = 0;

    [System.Serializable]
    public class StagePopup
    {
        public GameObject panel;
        public TextMeshProUGUI stageText;

        [Header("Sound")]
        public AudioClip popupSound;
    }

    [Header("===== STAGE POPUPS =====")]
    public StagePopup[] stagePopups;

    [Header("===== PANEL ANIMATION =====")]
    public float panelShowTime  = 1f;
    public float animationTime  = 0.25f;

    // ── Session tracking ─────────────────────────────────────────────────────
    private int sessionScore = 0;
    private int sessionStage = 1;

    [Header("===== PRE ATTACHED KNIVES =====")]
    public GameObject knifePrefab;
    public float attachedKnifeRadius = 1.2f;
    public float knifeInsertDepth = 0.25f;

    [Tooltip("Last challenge ke alawa baaki mein knife attach hone ki chance (0=kabhi nahi, 1=hamesha)")]
    [Range(0f, 1f)]
    public float attachedKnifeSpawnChance = 0.5f;

    private int winChallengeNumber = 1;

    private int savedLevel;

    [Header("===== SKULL SETTINGS =====")]
    public GameObject skullPrefab;

    public float skullRadius = 1.2f;
    public float skullInsertDepth = 0.2f;

    [Header("===== SKULL RANDOM =====")]
    public int minSkulls = 1;
    public int maxSkulls = 3;

    [Range(0f, 1f)]
    public float earlyStageSpawnChance = 0.5f;

    [Header("===== SKULL SCORE =====")]
    public int totalSkulls = 0;

    // =========================================================================
    // =================== PROGRESS DOTS (RESTORED) ============================
    // =========================================================================

    [Header("===== PROGRESS DOTS =====")]
    [Tooltip("Har stage ke liye ek GameObject drag karo — circle, star, diamond kuch bhi")]
    public GameObject[] progressDots;

    [Tooltip("Inactive dot ka color (white)")]
    public Color dotInactiveColor = Color.white;

    [Tooltip("Completed dot ka color (yellow)")]
    public Color dotActiveColor   = new Color(1f, 0.85f, 0f, 1f);

    [Tooltip("Dot scale animation ka duration")]
    public float dotAnimDuration  = 0.3f;

    // Runtime pe cache hogi Images/SpriteRenderers
    private Image[]          dotImages;
    private SpriteRenderer[] dotSprites;

    // Jab poora dots array fill (ek cycle complete) ho jata hai to turant
    // LoadChallenge ke andar wale UpdateProgressDots() ko skip karte hain,
    // taaki last dot ki completion animation pehle poori dikh jaye — uske
    // baad ResetProgressDotsAfterCycle() khud sab dots ko reset karta hai.
    private bool skipDotUpdateOnNextLoad = false;

    // =========================================================================
    // =================== STAGE PROGRESS SLIDER (NEW) =========================
    // =========================================================================

    [Header("===== STAGE PROGRESS SLIDER =====")]
    [Tooltip("Poora slider panel — stage complete hote hi thodi der ke liye popup hoga")]
    public GameObject progressSliderPanel;

    [Tooltip("Actual UI Slider jo stage ke hisaab se fill hoga")]
    public Slider progressSlider;

    [Tooltip("Optional — panel ke fade in/out ke liye CanvasGroup. Na ho to sirf scale animation chalegi")]
    public CanvasGroup progressSliderCanvasGroup;

    [Tooltip("Slider fully show hone ke baad kitni der ruka rahega")]
    public float sliderShowTime = 1f;

    [Tooltip("Slider ko purane value se naye value tak fill hone mein kitna time lagega")]
    public float sliderFillDuration = 0.5f;

    [Tooltip("Panel ke popup/close (scale+fade) animation ka duration")]
    public float sliderPanelAnimTime = 0.25f;

    // Ek time pe sirf ek slider animation chale — overlap na ho isliye guard
    private bool isSliderAnimating = false;

    [Header("===== STAGE PROGRESS SHOW SETTINGS =====")]

[Tooltip("Is stage se slider show hona start hoga")]
public int showSliderFromStage = 5;

[Tooltip("Slider popup sound")]
public AudioClip sliderPopupSound;

    // =========================================================================
    void Start()
    {
        // Random ko system time se re-seed karo — taaki har game reload/restart
        // pe naya random order mile, chahe koi aur script bhi Random use kar
        // raha ho ya Editor mein domain reload disabled ho
        Random.InitState(System.Environment.TickCount);

        // Skin shuffle ke liye alag Random instance, GUID se seed — sabse
        // strong entropy source, isliye order guaranteed alag hoga har baar
        skinShuffleRng = new System.Random(System.Guid.NewGuid().GetHashCode());

        UtilityButtonManager.RefreshSoundState();

        // Game hamesha shuru se load hoga — Level resume nahi hoga.
        // Highscore alag se UtilityButtonManager mein save/track hota hai.
        winChallengeNumber = 1;

        CreateRandomRotationOrder();
        CreateSkinOrders();
        UpdateChallengeTexts();
        NewSession();

        foreach (StagePopup popup in stagePopups)
            if (popup.panel != null)
                popup.panel.SetActive(false);

        // Progress dots spawn karo
        BuildProgressDots();

        // Progress slider panel shuru mein hidden aur khali rahega
        if (progressSliderPanel != null)
            progressSliderPanel.SetActive(false);

        if (progressSlider != null)
            progressSlider.value = 0f;
    }

    // ── Naya session ─────────────────────────────────────────────────────────
    void NewSession()
    {
        sessionScore = 0;
        sessionStage = 1;

        // Game hamesha stage 0 (Stage 1) se hi start hoga — resume nahi hoga
        currentChallenge = 0;

        LoadChallenge();
    }

    // =========================================================================
    // =================== SKIN SELECTION (GROUP 1 / GROUP 2) =================
    // =========================================================================

    // Group1 aur Group2 dono ke shuffled orders banao — sirf ek baar Start() mein
    void CreateSkinOrders()
    {
        topSkinOrder = ShuffledIndices(topCircleSkins != null ? topCircleSkins.Length : 0);
        remainingSkinOrder = ShuffledIndices(remainingCircleSkins != null ? remainingCircleSkins.Length : 0);
    }

    // 0..length-1 ke indices ka Fisher-Yates shuffled array banata hai
    // (dedicated skinShuffleRng use karta hai, UnityEngine.Random nahi)
    int[] ShuffledIndices(int length)
    {
        int[] arr = new int[length];
        for (int i = 0; i < length; i++)
            arr[i] = i;

        for (int i = length - 1; i > 0; i--)
        {
            int rand = skinShuffleRng.Next(0, i + 1);
            int temp = arr[i];
            arr[i] = arr[rand];
            arr[rand] = temp;
        }

        return arr;
    }

    // Stage index (0-based) ke hisaab se sahi entry (skin + knifeCount) return karo.
    //
    // Group1 ke SAARE elements pehle aayenge — random order mein, ek-ek karke,
    // koi repeat nahi jab tak group1 khatam na ho jaye.
    // Uske baad Group2 ke SAARE elements aayenge — waise hi random order mein,
    // ek-ek karke, koi repeat nahi.
    // Agar total stages in dono groups ke total se bhi zyada hain, to us point
    // ke baad Group2 se purely random (repeat allowed) continue hota hai.
    SkinEntry GetSkinEntryForStage(int stageIndex)
    {
        int group1Count = topCircleSkins != null ? topCircleSkins.Length : 0;
        int group2Count = remainingCircleSkins != null ? remainingCircleSkins.Length : 0;

        // Pehle Group1 ke saare elements, ek-ek karke
        if (stageIndex < group1Count)
        {
            int entryIdx = topSkinOrder[stageIndex];
            return topCircleSkins[entryIdx];
        }

        // Uske baad Group2 ke saare elements, ek-ek karke
        int group2Index = stageIndex - group1Count;
        if (group2Index < group2Count)
        {
            int entryIdx = remainingSkinOrder[group2Index];
            return remainingCircleSkins[entryIdx];
        }

        // Dono groups khatam ho gaye — ab Group2 se purely random continue karo
        if (group2Count > 0)
            return remainingCircleSkins[Random.Range(0, group2Count)];

        // Fallback: agar group2 khali hai to group1 se hi de do
        if (group1Count > 0)
            return topCircleSkins[Random.Range(0, group1Count)];

        return null;
    }

    // =========================================================================
    // =================== PROGRESS DOTS (RESTORED) ============================
    // =========================================================================

    // Inspector mein assign kiye GameObjects se Image/SpriteRenderer cache karo
    void BuildProgressDots()
    {
        if (progressDots == null || progressDots.Length == 0) return;

        dotImages  = new Image[progressDots.Length];
        dotSprites = new SpriteRenderer[progressDots.Length];

        for (int i = 0; i < progressDots.Length; i++)
        {
            if (progressDots[i] == null) continue;

            // UI Image try karo pehle
            Image img = progressDots[i].GetComponent<Image>();
            if (img == null) img = progressDots[i].GetComponentInChildren<Image>();

            if (img != null)
            {
                dotImages[i] = img;
                img.color = dotInactiveColor;
                continue;
            }

            // Warna SpriteRenderer (world-space objects ke liye)
            SpriteRenderer sr = progressDots[i].GetComponent<SpriteRenderer>();
            if (sr == null) sr = progressDots[i].GetComponentInChildren<SpriteRenderer>();

            if (sr != null)
            {
                dotSprites[i] = sr;
                sr.color = dotInactiveColor;
            }
        }
    }

    // Sab dots ka color current state ke hisaab se set karo (animation ke bina)
    // NOTE: Yeh hamesha currentChallenge (sequential stage counter) use karta hai,
    // skin index se koi lena dena nahi — isliye dots hamesha line se hi fill honge
    // chahe koi bhi random skin us stage par dikhi ho.
    void UpdateProgressDots()
{
    if(progressDots == null || progressDots.Length == 0)
        return;

    int filledDots = currentChallenge % progressDots.Length;

    for(int i = 0; i < progressDots.Length; i++)
    {
        Color c = (i < filledDots) ? dotActiveColor : dotInactiveColor;
        SetDotColor(i, c);
    }
}

    // Ek dot ka color set karo (Image ya SpriteRenderer dono handle)
    void SetDotColor(int i, Color c)
    {
        if (dotImages  != null && i < dotImages.Length  && dotImages[i]  != null)
            dotImages[i].color = c;

        if (dotSprites != null && i < dotSprites.Length && dotSprites[i] != null)
            dotSprites[i].color = c;
    }

    // Dot ko scale animation ke saath yellow karo
    IEnumerator AnimateDotComplete(int index)
    {
        if (progressDots == null || index >= progressDots.Length || progressDots[index] == null)
            yield break;

        Transform t = progressDots[index].transform;
        float elapsed = 0f;

        // Scale up
        while (elapsed < dotAnimDuration * 0.5f)
        {
            elapsed += Time.deltaTime;
            float s = Mathf.Lerp(1f, 1.5f, elapsed / (dotAnimDuration * 0.5f));
            t.localScale = Vector3.one * s;
            yield return null;
        }

        SetDotColor(index, dotActiveColor);
        elapsed = 0f;

        // Scale back
        while (elapsed < dotAnimDuration * 0.5f)
        {
            elapsed += Time.deltaTime;
            float s = Mathf.Lerp(1.5f, 1f, elapsed / (dotAnimDuration * 0.5f));
            t.localScale = Vector3.one * s;
            yield return null;
        }

        t.localScale = Vector3.one;
    }

    // Jab poora dots array ek baar fill ho jata hai (ek cycle complete), tab
    // yeh coroutine last dot ki completion animation khatam hone ka intezaar
    // karta hai, uske baad SAARE dots ko wapas inactive color/scale mein
    // reset kar deta hai — taaki agla cycle bilkul shuru (khaali) se fill ho.
    IEnumerator ResetProgressDotsAfterCycle()
    {
        yield return new WaitForSeconds(dotAnimDuration + 0.05f);

        if (progressDots != null)
        {
            for (int i = 0; i < progressDots.Length; i++)
            {
                SetDotColor(i, dotInactiveColor);

                if (progressDots[i] != null)
                    progressDots[i].transform.localScale = Vector3.one;
            }
        }

        skipDotUpdateOnNextLoad = false;
    }

    // =========================================================================
    // =================== STAGE PROGRESS SLIDER (NEW) =========================
    // =========================================================================

    // Stage complete hone par slider panel ko popup karo, use naye stage tak
    // fill karo, thodi der dikhao, fir animate karke band kar do.
    // completedStageIndex : jo dot/stage abhi complete hui uska 0-based index
    // cycleCompleted      : true jab is completion se poora dots array bhar gaya
    IEnumerator ShowProgressSlider(int completedStageIndex, bool cycleCompleted)
    {
        if (progressSliderPanel == null || progressSlider == null || totalStages <= 0)
            yield break;

        // Agar pehle se koi slider animation chal rahi hai to naya call skip karo
        if (isSliderAnimating)
            yield break;

        isSliderAnimating = true;

        float startValue  = progressSlider.value;
       float targetValue = (float)(completedStageIndex + 1) / totalStages;

        Transform panelT = progressSliderPanel.transform;
        panelT.localScale = Vector3.one * 0.7f;

        if (progressSliderCanvasGroup != null)
            progressSliderCanvasGroup.alpha = 0f;

        progressSliderPanel.SetActive(true);
        if(audioSource != null && sliderPopupSound != null)
{
    audioSource.PlayOneShot(sliderPopupSound);
}

        // ---- Panel popup animation (scale + fade in) ----
        float elapsed = 0f;
        while (elapsed < sliderPanelAnimTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sliderPanelAnimTime;
            panelT.localScale = Vector3.one * Mathf.Lerp(0.7f, 1f, t);

            if (progressSliderCanvasGroup != null)
                progressSliderCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }
        panelT.localScale = Vector3.one;
        if (progressSliderCanvasGroup != null)
            progressSliderCanvasGroup.alpha = 1f;

        // ---- Slider fill animation (purane value se naye value tak) ----
        elapsed = 0f;
        while (elapsed < sliderFillDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sliderFillDuration;
            progressSlider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }
        progressSlider.value = targetValue;

        // Poora fill hone ke baad thodi der wahi dikhao
        yield return new WaitForSeconds(sliderShowTime);

        // ---- Panel close animation (scale + fade out) ----
        elapsed = 0f;
        while (elapsed < sliderPanelAnimTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sliderPanelAnimTime;
            panelT.localScale = Vector3.one * Mathf.Lerp(1f, 0.7f, t);

            if (progressSliderCanvasGroup != null)
                progressSliderCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        progressSliderPanel.SetActive(false);
        panelT.localScale = Vector3.one;
        if (progressSliderCanvasGroup != null)
            progressSliderCanvasGroup.alpha = 1f;

        // Agar poora cycle complete ho gaya tha (dots bhi reset ho rahe hain),
        // to slider ko bhi 0 par reset karo taaki agla cycle fresh se fill ho
        if (cycleCompleted)
            progressSlider.value = 0f;

        isSliderAnimating = false;
    }

    // =========================================================================
    // =================== EXISTING METHODS ====================================
    // =========================================================================

    public void AddKnifeScore()
    {
        sessionScore += 2;
    }

    void LoadChallenge()
    {
        if (totalStages <= 0) return;

        if (currentChallenge >= totalStages)
            currentChallenge = 0;

        savedLevel = currentChallenge;

        currentSkinEntry = GetSkinEntryForStage(currentChallenge);
        circleRenderer.sprite = currentSkinEntry != null ? currentSkinEntry.skin : null;

        UpdateRotation();

        if (currentSkinEntry != null)
        {
            gameManager.totalKnives = currentSkinEntry.knifeCount;
            gameManager.stageTotalKnives = currentSkinEntry.knifeCount;
            gameManager.UpdateKnifeCountText();
        }

        UpdateChallengeTexts();

        // Stage text GameManager mein update karo
        // NOTE: currentChallenge PlayerPrefs se aata hai (saved array index),
        // isliye stage number hamesha sessionStage se lo — wo har naye
        // session/restart pe 1 se hi start hota hai.
        if (gameManager != null)
            gameManager.UpdateStageText(sessionStage);

        gameManager.ResetChallenge();
        SpawnAttachedKnives();
        SpawnAttachedSkulls();

        // Dots update karo current state ke hisaab se.
        // Agar abhi-abhi ek poora cycle complete hokar reset hua hai, to yeh
        // skip karo — ResetProgressDotsAfterCycle() khud sahi time par
        // (last dot ki completion animation ke baad) sab dots reset karega.
        if (!skipDotUpdateOnNextLoad)
            UpdateProgressDots();
    }

    public void ChallengeCompleted()
    {
        int completedStageIndex = currentChallenge; // dot index jo complete hua
        int completedStage = sessionStage; // display ke liye — abhi jo stage complete hui

        currentChallenge++;
        sessionStage++; // hamesha ek se aage badhao, array index se independent

        // Kya is completion se poora dots array bhar gaya (ek cycle complete)?
        bool cycleCompleted = false;

if(progressDots != null && progressDots.Length > 0)
{
    cycleCompleted = ((completedStageIndex + 1) % progressDots.Length) == 0;
}

        // Completed dot ko animate karo
       int dotIndex = completedStageIndex % progressDots.Length;
StartCoroutine(AnimateDotComplete(dotIndex));

        // Progress slider popup karo aur naye stage tak fill karo
if(sessionStage >= showSliderFromStage)
{
    StartCoroutine(ShowProgressSlider(completedStageIndex, cycleCompleted));
}
        if (cycleCompleted)
{
    skipDotUpdateOnNextLoad = true;
    StartCoroutine(ResetProgressDotsAfterCycle());
}

        LoadChallenge();
        StartCoroutine(ShowStagePanel(completedStage, currentChallenge));
    }

    public void TriggerGameOver()
    {
        gameManager.displayScore = sessionScore;
        gameManager.displayStage = sessionStage;

        UtilityButtonManager utility = FindFirstObjectByType<UtilityButtonManager>();
        if (utility != null)
            utility.UpdateBestScoreAndStage(sessionScore, sessionStage);

        gameManager.GameOver();
    }

    // ── Win panel: NEXT CHALLENGE button ──────────────────────────────────────
    public void NextChallenge()
    {
        winChallengeNumber++;

        UpdateChallengeTexts();

        if (currentChallenge >= totalStages)
            currentChallenge = 0;

        sessionScore = 0;
        sessionStage++; // hamesha ek se aage badhao

        LoadChallenge();
    }

    // ── Win panel: REPEAT button ───────────────────────────────────────────────
    public void RepeatChallenge()
    {
        currentChallenge = Mathf.Max(0, currentChallenge - 1);

        sessionScore = 0;
        sessionStage = Mathf.Max(1, sessionStage - 1); // ek peeche jao, 1 se neeche na jaye

        LoadChallenge();
    }

    // ── Legacy restart (pehle wala method, zaroorat pade to) ─────────────────
    public void RestartFromChallenge1()
    {
        currentChallenge = 0;

        winChallengeNumber = 1;

        // Reload/Restart pe skins ka order bhi dobara shuffle karo — warna
        // Start() sirf ek baar chalta hai to purana hi order reuse hota rehta
        CreateSkinOrders();

        UpdateChallengeTexts();

        NewSession();

        skipDotUpdateOnNextLoad = false;
        UpdateProgressDots();

        if (progressSlider != null)
            progressSlider.value = 0f;
    }

    public void ChallengeCompletedFromReload()
    {
        RestartFromChallenge1();
    }

    // ── Stage Panel ───────────────────────────────────────────────────────────
    IEnumerator ShowStagePanel(int stageNumber, int challengeIndex)
    {
        if (stagePopups.Length == 0) yield break;

        int index = Mathf.Clamp(challengeIndex, 0, stagePopups.Length - 1);
        StagePopup popup = stagePopups[index];

        if (popup.panel == null) yield break;

        popup.panel.SetActive(true);

        if (popup.stageText != null)
            popup.stageText.text = "STAGE " + stageNumber;

        if (audioSource != null && popup.popupSound != null)
            audioSource.PlayOneShot(popup.popupSound);

        yield return new WaitForSeconds(panelShowTime);

        popup.panel.SetActive(false);
    }

    // ── Spawn Knives ──────────────────────────────────────────────────────────
    void SpawnAttachedKnives()
    {
        if (knifePrefab == null) return;

        int knifeCount = GetPreAttachedKnifeCount();
        if (knifeCount <= 0) return;

        float angleStep = 360f / knifeCount;

        for (int i = 0; i < knifeCount; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
            Vector3 pos = circleRenderer.transform.position + dir * attachedKnifeRadius;
            pos -= dir * knifeInsertDepth;

            GameObject knife = Instantiate(knifePrefab, pos, Quaternion.identity);
            knife.tag = "Knife";
            knife.transform.rotation = Quaternion.Euler(0f, 0f, angle - 180f);
            knife.transform.SetParent(circleRenderer.transform);

            Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            Knife knifeScript = knife.GetComponent<Knife>();

            if (knifeScript != null)
                knifeScript.manager = gameManager;
        }
    }

    int GetPreAttachedKnifeCount()
    {
        bool isLastStage = (currentChallenge == totalStages - 1);

        if (isLastStage)
        {
            return Random.Range(1, 4);
        }
        else
        {
            return (Random.value <= attachedKnifeSpawnChance) ? Random.Range(1, 4) : 0;
        }
    }

    public CircleHitEffect GetCurrentHitEffect()
    {
        if (circleHitEffects == null || circleHitEffects.Length == 0)
            return null;

        int index = Mathf.Clamp(currentChallenge, 0, circleHitEffects.Length - 1);
        return circleHitEffects[index];
    }

    void UpdateChallengeTexts()
    {
        foreach (TextMeshProUGUI txt in challengeTexts)
        {
            if (txt != null)
                txt.text = "LEVEL " + winChallengeNumber;
        }
    }

    void UpdateRotation()
    {
        // Sab scripts disable karo
        for (int i = 0; i < rotationScripts.Length; i++)
        {
            if (rotationScripts[i] != null)
                rotationScripts[i].enabled = false;
        }

        // Normal random rotation — har stage ke liye, boss ka koi special case nahi
        if (rotationOrder != null && rotationOrder.Length > 0)
        {
            int randomIndex = rotationOrder[currentChallenge % rotationOrder.Length];

            if (rotationScripts[randomIndex] != null)
            {
                rotationScripts[randomIndex].enabled = true;

                Debug.Log("Enabled Random: " +
                          rotationScripts[randomIndex].GetType().Name);
            }
        }
    }

    void CreateRandomRotationOrder()
    {
        rotationOrder = new int[rotationScripts.Length];

        for (int i = 0; i < rotationScripts.Length; i++)
        {
            rotationOrder[i] = i;
        }

        // Fisher-Yates shuffle
        for (int i = 0; i < rotationOrder.Length; i++)
        {
            int rand = Random.Range(i, rotationOrder.Length);

            int temp = rotationOrder[i];
            rotationOrder[i] = rotationOrder[rand];
            rotationOrder[rand] = temp;
        }
    }

    // ── saveID logic removed: fixed keys ab use ho rahe hain ──────────────────
    // NOTE: Level/Challenge ab save nahi hote — game hamesha shuru se load hota
    // hai. Ye keys sirf purane saved data ko cleanup karne ke liye rakhi hain
    // (dekho ResetProgress neeche).
    string LevelKey
    {
        get { return "Level"; }
    }

    string ChallengeKey
    {
        get { return "Challenge"; }
    }

    public void ResetProgress()
    {
        currentChallenge = 0;
        winChallengeNumber = 1;

        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.DeleteKey(ChallengeKey);

        UpdateChallengeTexts();
    }

    public void ReloadCurrentLevel()
    {
        sessionScore = 0;
        LoadChallenge();
    }

    public void RestartLevel()
    {
        sessionScore = 0;
        sessionStage = 1;

        currentChallenge = 0;

        // Yahan bhi reshuffle karo, taaki restart pe fresh random order mile
        CreateSkinOrders();

        skipDotUpdateOnNextLoad = false;

        if (progressSlider != null)
            progressSlider.value = 0f;

        LoadChallenge();
    }

    public int GetCurrentLevel()
    {
        return winChallengeNumber;
    }

    void SpawnAttachedSkulls()
    {
        if (skullPrefab == null)
            return;

        int currentStage = currentChallenge + 1;

        bool shouldSpawn = false;

        // Stage 1-3
        if (currentStage < 4)
        {
            shouldSpawn = Random.value <= earlyStageSpawnChance;
        }
        // Stage 4+
        else
        {
            shouldSpawn = true;
        }

        if (!shouldSpawn)
            return;

        int skullCount = Random.Range(minSkulls, maxSkulls + 1);

        float angleStep = 360f / skullCount;

        float randomOffset = Random.Range(0f, 360f);

        for (int i = 0; i < skullCount; i++)
        {
            float angle = randomOffset + (i * angleStep);

            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;

            Vector3 pos = circleRenderer.transform.position + dir * skullRadius;

            pos -= dir * skullInsertDepth;

            GameObject skull = Instantiate(skullPrefab, pos, Quaternion.identity);

            skull.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            skull.transform.SetParent(circleRenderer.transform);

           
        }
    }
}