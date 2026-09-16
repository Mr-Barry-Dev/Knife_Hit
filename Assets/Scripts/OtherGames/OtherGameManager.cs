// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
// using UnityEngine.Networking;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class OtherGameManager : MonoBehaviour
// {
//     [Header("===== CIRCLE =====")]
//     public Transform rotatingCircle;
//     public float rotateSpeed = 200f;

//     [Header("===== KNIFE =====")]
//     public GameObject knifePrefab;
//     public Transform knifeSpawnPoint;
//     public float knifeSpeed = 30f;

//     [Header("===== TOTAL KNIVES =====")]
//     public int totalKnives = 9;
//     public Slider knifeSlider;

//     [Header("===== HIT EFFECT =====")]
//     public float knifeInsertDistance = 0.15f;
//     public float insertSpeed = 20f;

//     [Header("===== GAME OVER PANEL =====")]
//     public RectTransform gameOverPanel;
//     public float popupSpeed = 8f;

//     [Header("===== GAME OVER PANEL TEXTS =====")]
//     public TMP_Text gameOverStageText;
//     public TMP_Text gameOverScoreText;

//     [Header("===== GAME OVER SOUND =====")]
//     public AudioSource audioSource;
//     public AudioClip gameOverSound;

//     [Header("===== SOCIAL BUTTONS =====")]
//     public Button instaButton;
//     public Button facebookButton;
//     public Button mailButton;
//     public Button exitButton;


//     [Header("===== MORE GAMES =====")]
// public Button[] moreGameButtons;

// [TextArea]
// public string[] moreGameLinks;

// public float popupDuration = 2f;
// public float popupAnimSpeed = 8f;

//     [Header("===== KNIFE COUNT TEXT =====")]
// public TMP_Text knifeCountText;
// private int lastShownIndex = -1;

// [HideInInspector]
// public int stageTotalKnives;

//     [Header("===== SOCIAL LINKS =====")]
//     public string instagramURL = "https://www.instagram.com/yourpage";
//     public string facebookURL  = "https://www.facebook.com/yourpage";
//     public string mailURL      = "mailto:youremail@example.com";

//     [Header("===== NO INTERNET TEXT =====")]
//     public TMP_Text noInternetText;

//     [HideInInspector] public bool challengeCompleted = false;
//     [HideInInspector] public int  displayScore = 0;
//     [HideInInspector] public int  displayStage = 1;
//     [HideInInspector] public bool gameStarted  = false;

//     private GameObject currentKnife;
//     private bool gameOver = false;

    

//     // =========================================================================
//     void Start()
//     {
//         // GameOver panel shuru mein band
//         if (gameOverPanel != null)
//         {
//             gameOverPanel.gameObject.SetActive(false);
//             gameOverPanel.localScale = Vector3.zero;
//         }

//         if (noInternetText != null)
//             noInternetText.gameObject.SetActive(false);

//         // Social / exit button listeners
//         if (instaButton    != null) instaButton.onClick.AddListener(() => OpenURL(instagramURL));
//         if (facebookButton != null) facebookButton.onClick.AddListener(() => OpenURL(facebookURL));
//         if (mailButton     != null) mailButton.onClick.AddListener(() => OpenURL(mailURL));
//         if (exitButton     != null) exitButton.onClick.AddListener(OnExitButtonClick);

//         UpdateKnifeSlider(true);
//         // SpawnKnife nahi — Play button ka wait karo

//         stageTotalKnives = totalKnives;
//         UpdateKnifeCountText();

//        gameStarted = true;
// gameOver = false;

// StartCoroutine(StartGameDelay());

// StartCoroutine(CheckInternetAndStartSuggestions());
//     }

//     // =========================================================================
//     void Update()
//     {
//         if (!gameStarted || gameOver) return;

       
//         if (Input.GetMouseButtonDown(0) && currentKnife != null && totalKnives > 0)
//             ThrowKnife();
//     }

//     // ===================== START GAME ========================================
//     // UtilityButtonManager ke Play button se call hoga
//     IEnumerator StartGameDelay()
// {
//     yield return null;

//     SpawnKnife();
// }
//     // ===================== THROW =============================================
//     void ThrowKnife()
// {
//     Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
//     rb.linearVelocity = Vector2.up * knifeSpeed;

//    totalKnives--;

// UpdateKnifeSlider(false);
// UpdateKnifeCountText();

//     currentKnife = null;
// }

//     // ===================== SPAWN =============================================
//     void SpawnKnife()
//     {
//         if (totalKnives <= 0) return;
//         currentKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
//         currentKnife.GetComponent<Knife1>().manager = this;
//     }

//     // ===================== SLIDER ============================================
//     void UpdateKnifeSlider(bool resetMax)
//     {
//         if (knifeSlider == null) return;
//         if (resetMax) knifeSlider.maxValue = totalKnives;
//         knifeSlider.value = totalKnives;
//     }

//     // ===================== KNIFE HIT =========================================
//     public void KnifeHitCircle(GameObject knifeObj)
//     {
//         StartCoroutine(InsertKnifeRoutine(knifeObj));
//     }

//     IEnumerator InsertKnifeRoutine(GameObject knifeObj)
// {
//     if (knifeObj == null)
//         yield break;

//     Rigidbody2D rb = knifeObj.GetComponent<Rigidbody2D>();

//     if (rb == null)
//         yield break;

//     rb.linearVelocity = Vector2.zero;
//     rb.angularVelocity = 0f;
//     rb.bodyType = RigidbodyType2D.Kinematic;

//     Vector3 startPos = knifeObj.transform.position;
//     Vector3 targetPos = startPos + knifeObj.transform.up * knifeInsertDistance;

//     float t = 0f;

//     while (t < 1f)
//     {
//         if (knifeObj == null)
//             yield break;

//         t += Time.deltaTime * insertSpeed;
//         knifeObj.transform.position = Vector3.Lerp(startPos, targetPos, t);

//         yield return null;
//     }

//     if (knifeObj == null || rotatingCircle == null)
//         yield break;

//     knifeObj.transform.SetParent(rotatingCircle);

//     OtherGameManager2 gm2 = FindFirstObjectByType<OtherGameManager2>();

//     if (gm2 != null)
//         gm2.AddKnifeScore();

//     if (totalKnives <= 0)
//     {
//         challengeCompleted = true;

//         yield return new WaitForSeconds(1f);

//         if (gm2 != null)
//             gm2.ChallengeCompleted();
//     }
//     else
//     {
//         SpawnKnife();
//     }
// }

//     // ===================== RESET (round reset, OtherGameManager2 se call) =========
//     public void ResetChallenge()
//     {
//         StopAllCoroutines();

//         challengeCompleted = false;
//         gameOver           = false;

//         if (currentKnife != null) { Destroy(currentKnife); currentKnife = null; }

//         for (int i = rotatingCircle.childCount - 1; i >= 0; i--)
//             Destroy(rotatingCircle.GetChild(i).gameObject);

//         UpdateKnifeSlider(true);

//         if (gameStarted) SpawnKnife();

//         UpdateKnifeCountText();
//     }

//     // ===================== GAME OVER =========================================
//     // OtherGameManager2.TriggerGameOver() se call hoga — displayScore/Stage pehle set honge
//     public void GameOver()
//     {
//         if (gameOver) return;
//         gameOver    = true;
//         gameStarted = false;

//         if (currentKnife != null) { Destroy(currentKnife); currentKnife = null; }

//         StartCoroutine(GameOverRoutine());
//     }

//     IEnumerator GameOverRoutine()
//     {
//         // Sound
//         if (audioSource != null && gameOverSound != null)
//             audioSource.PlayOneShot(gameOverSound);

//         // 1 sec wait
//         yield return new WaitForSeconds(1f);

//         // Texts
//         OtherGameManager2 gm2 =
//     FindFirstObjectByType<OtherGameManager2>();

// if (gm2 != null && gameOverStageText != null)
// {
//     gameOverStageText.text =
//         "LEVEL " + gm2.GetCurrentLevel();
// }

// if (gameOverScoreText != null)
// {
//     gameOverScoreText.text =
//         "Score " + displayScore;
// }

//         // Panel show
//         gameOverPanel.gameObject.SetActive(true);
//         gameOverPanel.localScale = Vector3.zero;

//         float duration = 0.4f;
// float timer = 0f;

// while (timer < duration)
// {
//     timer += Time.deltaTime;

//     float progress = timer / duration;

//     gameOverPanel.localScale =
//         Vector3.LerpUnclamped(
//             Vector3.zero,
//             Vector3.one,
//             Mathf.SmoothStep(0f, 1f, progress));

//     yield return null;
// }

// gameOverPanel.localScale = Vector3.one;
//         gameOverPanel.localScale = Vector3.one;
//     }

   



//     // ===================== SOCIAL / EXIT =====================================
//     void OpenURL(string url) => StartCoroutine(CheckInternetThenOpen(url));

//     IEnumerator CheckInternetThenOpen(string url)
//     {
//         using (UnityWebRequest req = UnityWebRequest.Head("https://www.google.com"))
//         {
//             req.timeout = 5;
//             yield return req.SendWebRequest();

//             if (req.result == UnityWebRequest.Result.Success)
//                 Application.OpenURL(url);
//             else if (noInternetText != null)
//                 StartCoroutine(NoInternetTextRoutine());
//         }
//     }

//     void OnExitButtonClick()
//     {
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #else
//         Application.Quit();
// #endif
//     }

//     IEnumerator NoInternetTextRoutine()
//     {
//         noInternetText.gameObject.SetActive(true);
//         noInternetText.alpha = 0f;

//         float t = 0f;
//         while (t < 1f) { t += Time.deltaTime * 5f; noInternetText.alpha = Mathf.Lerp(0f, 1f, t); yield return null; }
//         noInternetText.alpha = 1f;

//         string[] dots = { "", " .", " ..", " ..." };
//         int idx = 0; float elapsed = 0f;
//         while (elapsed < 3f)
//         {
//             noInternetText.text = "No Internet Connection" + dots[idx++ % dots.Length];
//             yield return new WaitForSeconds(0.4f);
//             elapsed += 0.4f;
//         }

//         t = 0f;
//         while (t < 1f) { t += Time.deltaTime * 4f; noInternetText.alpha = Mathf.Lerp(1f, 0f, t); yield return null; }
//         noInternetText.alpha = 0f;
//         noInternetText.gameObject.SetActive(false);
//     }

//     public void Home()
// {
//     StopAllCoroutines();

//     CancelInvoke();

//     gameStarted = false;
//     gameOver = false;

//     if (currentKnife != null)
//     {
//         Destroy(currentKnife);
//         currentKnife = null;
//     }

//     Time.timeScale = 1f;

//     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
// }

// public void UpdateKnifeCountText()
// {
//     if (knifeCountText == null)
//         return;

//     int thrownKnives = stageTotalKnives - totalKnives;

//     knifeCountText.text = thrownKnives + "/" + stageTotalKnives;
// }


// IEnumerator CheckInternetAndStartSuggestions()
// {
//     using (UnityWebRequest req = UnityWebRequest.Head("https://www.google.com"))
//     {
//         req.timeout = 5;

//         yield return req.SendWebRequest();

//         if (req.result == UnityWebRequest.Result.Success)
//         {
//             StartCoroutine(ShowMoreGamesRoutine());
//         }
//     }
// }


// IEnumerator ShowMoreGamesRoutine()
// {
//     // Sab buttons hide
//     foreach (Button btn in moreGameButtons)
//     {
//         btn.gameObject.SetActive(false);
//     }

//     while (true)
//     {
//         // =========================
//         // RANDOM BUT NO REPEAT
//         // =========================

//         int randomIndex;

//         do
//         {
//             randomIndex = Random.Range(0, moreGameButtons.Length);
//         }
//         while (randomIndex == lastShownIndex && moreGameButtons.Length > 1);

//         lastShownIndex = randomIndex;

//         // =========================
//         // HIDE ALL BUTTONS
//         // =========================

//         foreach (Button btn in moreGameButtons)
//         {
//             btn.gameObject.SetActive(false);
//         }

//         Button currentBtn = moreGameButtons[randomIndex];

//         currentBtn.gameObject.SetActive(true);

//         // =========================
//         // POPUP ANIMATION
//         // =========================

//         currentBtn.transform.localScale = Vector3.zero;

//         float t = 0f;

//         while (t < 1f)
//         {
//             t += Time.deltaTime * popupAnimSpeed;

//             currentBtn.transform.localScale =
//                 Vector3.Lerp(Vector3.zero, Vector3.one, t);

//             yield return null;
//         }

//         currentBtn.transform.localScale = Vector3.one;

//         // =========================
//         // BUTTON LINK
//         // =========================

//         currentBtn.onClick.RemoveAllListeners();

//         string link = moreGameLinks[randomIndex];

//         currentBtn.onClick.AddListener(() =>
//         {
//             Application.OpenURL(link);
//         });

//         // =========================
//         // WAIT
//         // =========================

//         yield return new WaitForSeconds(popupDuration);

//         // =========================
//         // HIDE ANIMATION
//         // =========================

//         t = 0f;

//         while (t < 1f)
//         {
//             t += Time.deltaTime * popupAnimSpeed;

//             currentBtn.transform.localScale =
//                 Vector3.Lerp(Vector3.one, Vector3.zero, t);

//             yield return null;
//         }

//         currentBtn.gameObject.SetActive(false);
//     }
   
// }

// public void RestartCurrentLevel()
// {
//     StopAllCoroutines();

//     gameStarted = false;
//     gameOver = false;

//     if (currentKnife != null)
//     {
//         Destroy(currentKnife);
//         currentKnife = null;
//     }

//     for (int i = rotatingCircle.childCount - 1; i >= 0; i--)
//     {
//         Destroy(rotatingCircle.GetChild(i).gameObject);
//     }

//     if (gameOverPanel != null)
//     {
//         gameOverPanel.gameObject.SetActive(false);
//         gameOverPanel.localScale = Vector3.zero;
//     }

//     OtherGameManager2 gm2 =
//         FindFirstObjectByType<OtherGameManager2>();

//     if (gm2 != null)
//     {
//         gm2.RestartLevel();
//     }

//     gameStarted = true;
//      SpawnKnife();
// }

//     public void ContinueButton()
// {
//     StopAllCoroutines();

//     gameStarted = false;
//     gameOver = false;

//     // ================= ENABLE OBJECTS =================

//     gameObject.SetActive(true);
//     enabled = true;

//     OtherGameManager2 gm2 = FindFirstObjectByType<OtherGameManager2>();

//     if (gm2 != null)
//     {
//         gm2.gameObject.SetActive(true);
//         gm2.enabled = true;
//     }

//     // ================= RESET GAME =================

//     // Current knife destroy
//     if (currentKnife != null)
//     {
//         Destroy(currentKnife);
//         currentKnife = null;
//     }

//     // Circle ke andar stuck knives destroy
//     for (int i = rotatingCircle.childCount - 1; i >= 0; i--)
//     {
//         Destroy(rotatingCircle.GetChild(i).gameObject);
//     }

//     // Panel hide
//     if (gameOverPanel != null)
//     {
//         gameOverPanel.gameObject.SetActive(false);
//         gameOverPanel.localScale = Vector3.zero;
//     }

//     // OtherGameManager2 reset
//    if (gm2 != null)
// {
//     gm2.ReloadCurrentLevel();
// }

//     // IMPORTANT
//     totalKnives = stageTotalKnives;

//     UpdateKnifeSlider(true);
//     UpdateKnifeCountText();

//     // ================= GAME START =================

//     gameStarted = true;

//     SpawnKnife();

  
// }

// public void GoHome()
// {
//     SceneManager.LoadScene("Game");
// }
// }

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class OtherGameManager : MonoBehaviour
{
    [Header("===== CIRCLE =====")]
    public Transform rotatingCircle;
    public float rotateSpeed = 200f;

    [Header("===== KNIFE =====")]
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public float knifeSpeed = 30f;

    [Header("===== TOTAL KNIVES =====")]
    public int totalKnives = 9;
    public Slider knifeSlider;

    [Header("===== HIT EFFECT =====")]
    public float knifeInsertDistance = 0.15f;
    public float insertSpeed = 20f;

    [Header("===== GAME OVER PANEL =====")]
    public RectTransform gameOverPanel;
    public float popupSpeed = 8f;

    [Header("===== GAME OVER PANEL TEXTS =====")]
    public TMP_Text gameOverStageText;
    public TMP_Text gameOverScoreText;

    [Header("===== GAME OVER SOUND =====")]
    public AudioSource audioSource;
    public AudioClip gameOverSound;

    [Header("===== SOCIAL BUTTONS =====")]
    public Button instaButton;
    public Button facebookButton;
    public Button mailButton;
    public Button exitButton;


    [Header("===== MORE GAMES =====")]
public Button[] moreGameButtons;

[TextArea]
public string[] moreGameLinks;

public float popupDuration = 2f;
public float popupAnimSpeed = 8f;

    [Header("===== KNIFE COUNT TEXT =====")]
public TMP_Text knifeCountText;
private int lastShownIndex = -1;

[HideInInspector]
public int stageTotalKnives;

    [Header("===== SOCIAL LINKS =====")]
    public string instagramURL = "https://www.instagram.com/yourpage";
    public string facebookURL  = "https://www.facebook.com/yourpage";
    public string mailURL      = "mailto:youremail@example.com";

    [Header("===== NO INTERNET TEXT =====")]
    public TMP_Text noInternetText;

    [HideInInspector] public bool challengeCompleted = false;
    [HideInInspector] public int  displayScore = 0;
    [HideInInspector] public int  displayStage = 1;
    [HideInInspector] public bool gameStarted  = false;

    // Stage popup jaise UI overlays ke dauraan knife throw block karne ke liye
    [HideInInspector] public bool inputBlocked = false;

    private GameObject currentKnife;
    private bool gameOver = false;

    

    // =========================================================================
    void Start()
    {
        // GameOver panel shuru mein band
        if (gameOverPanel != null)
        {
            gameOverPanel.gameObject.SetActive(false);
            gameOverPanel.localScale = Vector3.zero;
        }

        if (noInternetText != null)
            noInternetText.gameObject.SetActive(false);

        // Social / exit button listeners
        if (instaButton    != null) instaButton.onClick.AddListener(() => OpenURL(instagramURL));
        if (facebookButton != null) facebookButton.onClick.AddListener(() => OpenURL(facebookURL));
        if (mailButton     != null) mailButton.onClick.AddListener(() => OpenURL(mailURL));
        if (exitButton     != null) exitButton.onClick.AddListener(OnExitButtonClick);

        UpdateKnifeSlider(true);
        // SpawnKnife nahi — Play button ka wait karo

        stageTotalKnives = totalKnives;
        UpdateKnifeCountText();

       gameStarted = true;
gameOver = false;

StartCoroutine(StartGameDelay());

StartCoroutine(CheckInternetAndStartSuggestions());
    }

    // =========================================================================
    void Update()
    {
        if (!gameStarted || gameOver) return;

       
        if (Input.GetMouseButtonDown(0) && currentKnife != null && totalKnives > 0 && !inputBlocked)
            ThrowKnife();
    }

    // ===================== START GAME ========================================
    // UtilityButtonManager ke Play button se call hoga
    IEnumerator StartGameDelay()
{
    yield return null;

    SpawnKnife();
}
    // ===================== THROW =============================================
    void ThrowKnife()
{
    Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
    rb.linearVelocity = Vector2.up * knifeSpeed;

   totalKnives--;

UpdateKnifeSlider(false);
UpdateKnifeCountText();

    currentKnife = null;
}

    // ===================== SPAWN =============================================
    void SpawnKnife()
    {
        if (totalKnives <= 0) return;
        currentKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        currentKnife.GetComponent<Knife1>().manager = this;
    }

    // ===================== SLIDER ============================================
    void UpdateKnifeSlider(bool resetMax)
    {
        if (knifeSlider == null) return;
        if (resetMax) knifeSlider.maxValue = totalKnives;
        knifeSlider.value = totalKnives;
    }

    // ===================== KNIFE HIT =========================================
    public void KnifeHitCircle(GameObject knifeObj)
    {
        StartCoroutine(InsertKnifeRoutine(knifeObj));
    }

    IEnumerator InsertKnifeRoutine(GameObject knifeObj)
{
    if (knifeObj == null)
        yield break;

    Rigidbody2D rb = knifeObj.GetComponent<Rigidbody2D>();

    if (rb == null)
        yield break;

    rb.linearVelocity = Vector2.zero;
    rb.angularVelocity = 0f;
    rb.bodyType = RigidbodyType2D.Kinematic;

    Vector3 startPos = knifeObj.transform.position;
    Vector3 targetPos = startPos + knifeObj.transform.up * knifeInsertDistance;

    float t = 0f;

    while (t < 1f)
    {
        if (knifeObj == null)
            yield break;

        t += Time.deltaTime * insertSpeed;
        knifeObj.transform.position = Vector3.Lerp(startPos, targetPos, t);

        yield return null;
    }

    if (knifeObj == null || rotatingCircle == null)
        yield break;

    knifeObj.transform.SetParent(rotatingCircle);

    OtherGameManager2 gm2 = FindFirstObjectByType<OtherGameManager2>();

    if (gm2 != null)
        gm2.AddKnifeScore();

    if (totalKnives <= 0)
    {
        challengeCompleted = true;

        yield return new WaitForSeconds(1f);

        if (gm2 != null)
            gm2.ChallengeCompleted();
    }
    else
    {
        SpawnKnife();
    }
}

    // ===================== RESET (round reset, OtherGameManager2 se call) =========
    public void ResetChallenge()
    {
        StopAllCoroutines();

        challengeCompleted = false;
        gameOver           = false;

        if (currentKnife != null) { Destroy(currentKnife); currentKnife = null; }

        for (int i = rotatingCircle.childCount - 1; i >= 0; i--)
            Destroy(rotatingCircle.GetChild(i).gameObject);

        UpdateKnifeSlider(true);

        if (gameStarted) SpawnKnife();

        UpdateKnifeCountText();
    }

    // ===================== GAME OVER =========================================
    // OtherGameManager2.TriggerGameOver() se call hoga — displayScore/Stage pehle set honge
    public void GameOver()
    {
        if (gameOver) return;
        gameOver    = true;
        gameStarted = false;

        if (currentKnife != null) { Destroy(currentKnife); currentKnife = null; }

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // Sound
        if (audioSource != null && gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);

        // 1 sec wait
        yield return new WaitForSeconds(1f);

        // Texts
        OtherGameManager2 gm2 =
    FindFirstObjectByType<OtherGameManager2>();

if (gm2 != null && gameOverStageText != null)
{
    gameOverStageText.text =
        "LEVEL " + gm2.GetCurrentLevel();
}

if (gameOverScoreText != null)
{
    gameOverScoreText.text =
        "Score " + displayScore;
}

        // Panel show
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.localScale = Vector3.zero;

        float duration = 0.4f;
float timer = 0f;

while (timer < duration)
{
    timer += Time.deltaTime;

    float progress = timer / duration;

    gameOverPanel.localScale =
        Vector3.LerpUnclamped(
            Vector3.zero,
            Vector3.one,
            Mathf.SmoothStep(0f, 1f, progress));

    yield return null;
}

gameOverPanel.localScale = Vector3.one;
        gameOverPanel.localScale = Vector3.one;
    }

   



    // ===================== SOCIAL / EXIT =====================================
    void OpenURL(string url) => StartCoroutine(CheckInternetThenOpen(url));

    IEnumerator CheckInternetThenOpen(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Head("https://www.google.com"))
        {
            req.timeout = 5;
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                Application.OpenURL(url);
            else if (noInternetText != null)
                StartCoroutine(NoInternetTextRoutine());
        }
    }

    void OnExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator NoInternetTextRoutine()
    {
        noInternetText.gameObject.SetActive(true);
        noInternetText.alpha = 0f;

        float t = 0f;
        while (t < 1f) { t += Time.deltaTime * 5f; noInternetText.alpha = Mathf.Lerp(0f, 1f, t); yield return null; }
        noInternetText.alpha = 1f;

        string[] dots = { "", " .", " ..", " ..." };
        int idx = 0; float elapsed = 0f;
        while (elapsed < 3f)
        {
            noInternetText.text = "No Internet Connection" + dots[idx++ % dots.Length];
            yield return new WaitForSeconds(0.4f);
            elapsed += 0.4f;
        }

        t = 0f;
        while (t < 1f) { t += Time.deltaTime * 4f; noInternetText.alpha = Mathf.Lerp(1f, 0f, t); yield return null; }
        noInternetText.alpha = 0f;
        noInternetText.gameObject.SetActive(false);
    }

    public void Home()
{
    StopAllCoroutines();

    CancelInvoke();

    gameStarted = false;
    gameOver = false;

    if (currentKnife != null)
    {
        Destroy(currentKnife);
        currentKnife = null;
    }

    Time.timeScale = 1f;

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

public void UpdateKnifeCountText()
{
    if (knifeCountText == null)
        return;

    int thrownKnives = stageTotalKnives - totalKnives;

    knifeCountText.text = thrownKnives + "/" + stageTotalKnives;
}


IEnumerator CheckInternetAndStartSuggestions()
{
    using (UnityWebRequest req = UnityWebRequest.Head("https://www.google.com"))
    {
        req.timeout = 5;

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            StartCoroutine(ShowMoreGamesRoutine());
        }
    }
}


IEnumerator ShowMoreGamesRoutine()
{
    // Sab buttons hide
    foreach (Button btn in moreGameButtons)
    {
        btn.gameObject.SetActive(false);
    }

    while (true)
    {
        // =========================
        // RANDOM BUT NO REPEAT
        // =========================

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, moreGameButtons.Length);
        }
        while (randomIndex == lastShownIndex && moreGameButtons.Length > 1);

        lastShownIndex = randomIndex;

        // =========================
        // HIDE ALL BUTTONS
        // =========================

        foreach (Button btn in moreGameButtons)
        {
            btn.gameObject.SetActive(false);
        }

        Button currentBtn = moreGameButtons[randomIndex];

        currentBtn.gameObject.SetActive(true);

        // =========================
        // POPUP ANIMATION
        // =========================

        currentBtn.transform.localScale = Vector3.zero;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * popupAnimSpeed;

            currentBtn.transform.localScale =
                Vector3.Lerp(Vector3.zero, Vector3.one, t);

            yield return null;
        }

        currentBtn.transform.localScale = Vector3.one;

        // =========================
        // BUTTON LINK
        // =========================

        currentBtn.onClick.RemoveAllListeners();

        string link = moreGameLinks[randomIndex];

        currentBtn.onClick.AddListener(() =>
        {
            Application.OpenURL(link);
        });

        // =========================
        // WAIT
        // =========================

        yield return new WaitForSeconds(popupDuration);

        // =========================
        // HIDE ANIMATION
        // =========================

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * popupAnimSpeed;

            currentBtn.transform.localScale =
                Vector3.Lerp(Vector3.one, Vector3.zero, t);

            yield return null;
        }

        currentBtn.gameObject.SetActive(false);
    }
   
}

public void RestartCurrentLevel()
{
    StopAllCoroutines();

    gameStarted = false;
    gameOver = false;

    if (currentKnife != null)
    {
        Destroy(currentKnife);
        currentKnife = null;
    }

    for (int i = rotatingCircle.childCount - 1; i >= 0; i--)
    {
        Destroy(rotatingCircle.GetChild(i).gameObject);
    }

    if (gameOverPanel != null)
    {
        gameOverPanel.gameObject.SetActive(false);
        gameOverPanel.localScale = Vector3.zero;
    }

    OtherGameManager2 gm2 =
        FindFirstObjectByType<OtherGameManager2>();

    if (gm2 != null)
    {
        gm2.RestartLevel();
    }

    gameStarted = true;
     SpawnKnife();
}

    public void ContinueButton()
{
    StopAllCoroutines();

    gameStarted = false;
    gameOver = false;

    // ================= ENABLE OBJECTS =================

    gameObject.SetActive(true);
    enabled = true;

    OtherGameManager2 gm2 = FindFirstObjectByType<OtherGameManager2>();

    if (gm2 != null)
    {
        gm2.gameObject.SetActive(true);
        gm2.enabled = true;
    }

    // ================= RESET GAME =================

    // Current knife destroy
    if (currentKnife != null)
    {
        Destroy(currentKnife);
        currentKnife = null;
    }

    // Circle ke andar stuck knives destroy
    for (int i = rotatingCircle.childCount - 1; i >= 0; i--)
    {
        Destroy(rotatingCircle.GetChild(i).gameObject);
    }

    // Panel hide
    if (gameOverPanel != null)
    {
        gameOverPanel.gameObject.SetActive(false);
        gameOverPanel.localScale = Vector3.zero;
    }

    // OtherGameManager2 reset
   if (gm2 != null)
{
    gm2.ReloadCurrentLevel();
}

    // IMPORTANT
    totalKnives = stageTotalKnives;

    UpdateKnifeSlider(true);
    UpdateKnifeCountText();

    // ================= GAME START =================

    gameStarted = true;

    SpawnKnife();

  
}

public void GoHome()
{
    SceneManager.LoadScene("Game");
}
}