// // using UnityEngine;
// // using UnityEngine.UI;
// // using UnityEngine.EventSystems;
// // using System.Collections;
// // using TMPro;

// // public class UtilityButtonManager : MonoBehaviour
// // {
// //     // ================= PANEL SYSTEM ==========================================

// //     [Header("MAIN ARRAYS (Index must match)")]
// //     public Button[] utilityButtons;
// //     public RectTransform[] panels;

// //     [Header("CLOSE BUTTONS (Index must match panels)")]
// //     public Button[] closeButtons;

// //     [Header("OTHER OBJECTS TO DISABLE")]
// //     public GameObject[] objectsToDisable;

// //     [Header("ANIMATION")]
// //     public float animDuration = 0.35f;
// //     public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

// //     private Vector3[] panelCenterPos;
// //     private Vector3[] panelOriginalScale;

// //     private bool panelOpen = false;
// //     private int  currentPanelIndex = -1;

// //     // ================= PLAY BUTTON ===========================================

// //     [Header("PLAY BUTTON")]
// //     public Button playButton;

// //     [Tooltip("GameManager assign karo — Play press hone pe StartGame() call hoga")]
// //     public GameManager gameManager;

// //     [Header("PLAY OBJECTS")]
// //     public GameObject[] objectsToDisableOnPlay;
// //     public GameObject[] objectsToEnableOnPlay;

// //     // ================= SOUND TOGGLE ==========================================

// //     [Header("SOUND TOGGLE")]
// //     public Toggle soundToggle;

// //     private const string SOUND_KEY = "SOUND_STATE";

// //     // ================= PRIVACY POLICY & RESET GAME ==========================

// //     [Header("PRIVACY POLICY BUTTON")]
// //     public Button privacyPolicyButton;
// //     public string privacyPolicyURL = "https://yourwebsite.com/privacy-policy";

// //     [Header("RESET GAME BUTTON")]
// //     public Button resetGameButton;

// //     // ================= BUTTON SOUND ==========================================

// //     [Header("Button Click Sound")]
// //     public AudioSource buttonAudioSource;
// //     public AudioClip   buttonClickSound;

// //     [Header("IGNORE DEFAULT BUTTON SOUND PANELS")]
// //     public GameObject[] ignoreSoundPanels;

// //     // ================= PULSE ANIMATION =======================================

// //     [Header("PULSE ANIMATION")]
// //     public Button pulseButton;
// //     public Image  pulseImage;

// //     [Header("Pulse Settings")]
// //     public float pulseRestMin     = 1.5f;
// //     public float pulseRestMax     = 2.5f;
// //     public float pulseSpeed       = 1.0f;

// //     [Header("First Pop")]
// //     public float firstPopScale    = 1.12f;
// //     public float firstPopDuration = 0.14f;

// //     [Header("Second Pop")]
// //     public float secondPopScale    = 1.06f;
// //     public float secondPopDuration = 0.11f;
// //     public float gapBetweenPops    = 0.08f;

// //     private Coroutine pulseCoroutine;
// //     private Vector3   btnOriginalScale;
// //     private Vector3   imgOriginalScale;

// //     // ================= BEST SCORE & STAGE (Utility panel texts) ==============

// //     [Header("BEST SCORE & STAGE TEXTS (Utility Panel)")]
// //     [Tooltip("Utility panel mein Best Score dikhane wala TMP_Text")]
// //     public TMP_Text utilityScoreText;

// //     [Tooltip("Utility panel mein Best Stage dikhane wala TMP_Text")]
// //     public TMP_Text utilityStageText;

// //     // PlayerPrefs keys
// //     private const string BEST_SCORE_KEY = "BEST_SCORE";
// //     private const string BEST_STAGE_KEY = "BEST_STAGE";
// //     private const string COINS_KEY       = "COINS";

    

// //     // =========================================================================
// //     void Start()
// //     {
// //         int count = panels.Length;
// //         panelCenterPos     = new Vector3[count];
// //         panelOriginalScale = new Vector3[count];

// //         for (int i = 0; i < count; i++)
// //         {
// //             int index = i;
// //             panelCenterPos[i]     = panels[i].position;
// //             panelOriginalScale[i] = panels[i].localScale;
// //             panels[i].gameObject.SetActive(false);
// //             utilityButtons[i].onClick.AddListener(() => OpenPanel(index));
// //             if (closeButtons[i] != null)
// //                 closeButtons[i].onClick.AddListener(CloseCurrentPanel);
// //         }

// //         if (playButton != null)
// //             playButton.onClick.AddListener(OnPlayPressed);

// //         if (privacyPolicyButton != null)
// //             privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClick);

// //         if (resetGameButton != null)
// //             resetGameButton.onClick.AddListener(OnResetGameClick);

// //         bool isOn = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
// //         if (soundToggle != null)
// //         {
// //             soundToggle.isOn = isOn;
// //             soundToggle.onValueChanged.AddListener(OnToggleChanged);
// //         }
// //         ApplySound(isOn);
// //         AddClickSoundToAllButtons();
// //         StartPulse();

// //         // Saved best values load karke texts update karo
// //         RefreshUtilityTexts();
// //     }

// //     // ================= BEST SCORE & STAGE ====================================

// //     /// <summary>
// //     /// GameManager2 game-over pe yeh call karta hai.
// //     /// Agar naya score/stage purane se zyada hai to override karke save karo.
// //     /// </summary>
// //     public void UpdateBestScoreAndStage(int newScore, int newStage)
// //     {
// //         int savedScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
// //         int savedStage = PlayerPrefs.GetInt(BEST_STAGE_KEY, 0);

// //         bool changed = false;

// //         if (newScore > savedScore)
// //         {
// //             PlayerPrefs.SetInt(BEST_SCORE_KEY, newScore);
// //             changed = true;
// //         }

// //         if (newStage > savedStage)
// //         {
// //             PlayerPrefs.SetInt(BEST_STAGE_KEY, newStage);
// //             changed = true;
// //         }

// //         if (changed)
// //         {
// //             PlayerPrefs.Save();
// //             RefreshUtilityTexts();
// //         }
// //     }

// //     /// PlayerPrefs se padh ke utility panel ke dono texts update karo
// //    void RefreshUtilityTexts()
// // {
// //     int bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
// //     int bestStage = PlayerPrefs.GetInt(BEST_STAGE_KEY, 0);

// //     if (utilityScoreText != null)
// //         utilityScoreText.text = "Score " + bestScore;

// //     if (utilityStageText != null)
// //         utilityStageText.text = "Stage " + bestStage;
// // }

// //     // ================= PRIVACY POLICY =======================================

// //     void OnPrivacyPolicyClick()
// //     {
// //         Application.OpenURL(privacyPolicyURL);
// //     }

// //     // ================= RESET GAME ============================================

// //     // void OnResetGameClick()
// //     // {
// //     //     // Score, Stage, Coins — sab PlayerPrefs se delete karo
// //     //     PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
// //     //     PlayerPrefs.DeleteKey(BEST_STAGE_KEY);
// //     //     PlayerPrefs.DeleteKey(COINS_KEY);

// //     //     // Agar aur bhi koi game data keys hain to yahan add karo
// //     //     // PlayerPrefs.DeleteAll();  // ye sab kuch delete karta hai (sound setting bhi)

// //     //     PlayerPrefs.Save();

// //     //     // Texts bhi zero pe reset karo
// //     //     RefreshUtilityTexts();

// //     //     // GameManager bhi reset karo
// //     //     if (gameManager != null)
// //     //         gameManager.ReloadGame();

// //     //     Debug.Log("Game Reset: Score, Stage, Coins sab clear ho gaye.");
// //     // }

// // void OnResetGameClick()
// // {
// //     // Best Score & Stage Reset
// //     PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
// //     PlayerPrefs.DeleteKey(BEST_STAGE_KEY);
// //     PlayerPrefs.DeleteKey(COINS_KEY);

// //     // Gems Reset
// //     if (GemManager.Instance != null)
// //     {
// //         GemManager.Instance.ResetGems();
// //     }

// //     PlayerPrefs.Save();

// //     // Utility Panel Text Refresh
// //     RefreshUtilityTexts();

// //     // Game Reload
// //     if (gameManager != null)
// //         gameManager.ReloadGame();

// //     Debug.Log("Game Reset Complete");
// // }
// //     // ================= PULSE SYSTEM ==========================================

// //     void StartPulse()
// //     {
// //         if (pulseButton == null && pulseImage == null) return;

// //         btnOriginalScale = pulseButton != null
// //             ? pulseButton.transform.localScale : Vector3.one;
// //         imgOriginalScale = pulseImage != null
// //             ? pulseImage.transform.localScale  : Vector3.one;

// //         if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
// //         pulseCoroutine = StartCoroutine(PulseLoop());
// //     }

// //     IEnumerator PulseLoop()
// //     {
// //         while (true)
// //         {
// //             float rest = Random.Range(pulseRestMin, pulseRestMax);
// //             yield return new WaitForSeconds(rest);

// //             yield return StartCoroutine(ScaleTo(
// //                 btnOriginalScale, imgOriginalScale,
// //                 firstPopScale, firstPopDuration / pulseSpeed, EaseOutBack));

// //             yield return StartCoroutine(ScaleTo(
// //                 btnOriginalScale * firstPopScale, imgOriginalScale * firstPopScale,
// //                 1f / firstPopScale, 0.10f / pulseSpeed, EaseOutSine));

// //             SetScale(btnOriginalScale, imgOriginalScale);

// //             yield return new WaitForSeconds(gapBetweenPops);

// //             yield return StartCoroutine(ScaleTo(
// //                 btnOriginalScale, imgOriginalScale,
// //                 secondPopScale, secondPopDuration / pulseSpeed, EaseOutBackSoft));

// //             yield return StartCoroutine(ScaleTo(
// //                 btnOriginalScale * secondPopScale, imgOriginalScale * secondPopScale,
// //                 1f / secondPopScale, 0.13f / pulseSpeed, EaseOutSine));

// //             SetScale(btnOriginalScale, imgOriginalScale);
// //         }
// //     }

// //     IEnumerator ScaleTo(Vector3 fromBtn, Vector3 fromImg,
// //                         float multiplier, float duration,
// //                         System.Func<float, float> easeFn)
// //     {
// //         float t = 0f;
// //         while (t < 1f)
// //         {
// //             t += Time.deltaTime / duration;
// //             float eval = easeFn(Mathf.Clamp01(t));

// //             if (pulseButton != null)
// //                 pulseButton.transform.localScale =
// //                     Vector3.LerpUnclamped(fromBtn, fromBtn * multiplier, eval);

// //             if (pulseImage != null)
// //                 pulseImage.transform.localScale =
// //                     Vector3.LerpUnclamped(fromImg, fromImg * multiplier, eval);

// //             yield return null;
// //         }
// //     }

// //     void SetScale(Vector3 btn, Vector3 img)
// //     {
// //         if (pulseButton != null) pulseButton.transform.localScale = btn;
// //         if (pulseImage  != null) pulseImage.transform.localScale  = img;
// //     }

// //     float EaseOutBack(float t)
// //     {
// //         float c1 = 1.70158f, c3 = c1 + 1f;
// //         return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
// //     }

// //     float EaseOutBackSoft(float t)
// //     {
// //         float c1 = 0.8f, c3 = c1 + 1f;
// //         return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
// //     }

// //     float EaseOutSine(float t) => Mathf.Sin((t * Mathf.PI) / 2f);

// //     // ================= SOUND CONTROL =========================================

// //     void OnToggleChanged(bool value)
// //     {
// //         PlayerPrefs.SetInt(SOUND_KEY, value ? 1 : 0);
// //         PlayerPrefs.Save();
// //         ApplySound(value);
// //     }

// //     void ApplySound(bool isOn) => AudioListener.volume = isOn ? 1f : 0f;

// //     public static void PlaySound(AudioSource source, AudioClip clip)
// //     {
// //         if (AudioListener.volume == 0f || source == null || clip == null) return;
// //         source.PlayOneShot(clip);
// //     }

// //     // ================= BUTTON SOUND ==========================================

// //     void AddClickSoundToAllButtons()
// //     {
// //         Button[] allButtons = FindObjectsOfType<Button>(true);
// //         foreach (Button btn in allButtons)
// //         {
// //             GameObject currentButton = btn.gameObject;
// //             btn.onClick.AddListener(() => PlayButtonSound(currentButton));
// //         }
// //     }

// //     void PlayButtonSound(GameObject clickedObject)
// //     {
// //         if (AudioListener.volume == 0f) return;

// //         foreach (GameObject panel in ignoreSoundPanels)
// //             if (panel != null && clickedObject.transform.IsChildOf(panel.transform)) return;

// //         if (buttonAudioSource != null && buttonClickSound != null)
// //             buttonAudioSource.PlayOneShot(buttonClickSound);
// //     }

// //     // ================= PLAY BUTTON ===========================================

// //     void OnPlayPressed() => StartCoroutine(HandlePlayObjects());

// //     IEnumerator HandlePlayObjects()
// //     {
// //         // Pehle UI objects animate karo
// //         foreach (GameObject obj in objectsToDisableOnPlay)
// //             if (obj != null) StartCoroutine(ScaleOut(obj));

// //         foreach (GameObject obj in objectsToEnableOnPlay)
// //             if (obj != null) { obj.SetActive(true); StartCoroutine(ScaleIn(obj)); }

// //         // GameManager ko batao ki game shuru ho gaya — knife spawn hogi
// //         if (gameManager != null)
// //             gameManager.StartGame();

// //         yield return null;
// //     }

// //     IEnumerator ScaleOut(GameObject obj)
// //     {
// //         Transform t = obj.transform;
// //         Vector3 startScale = t.localScale;
// //         float tVal = 0f;
// //         while (tVal < 1f)
// //         {
// //             tVal += Time.deltaTime / animDuration;
// //             t.localScale = Vector3.Lerp(startScale, Vector3.zero, animCurve.Evaluate(tVal));
// //             yield return null;
// //         }
// //         obj.SetActive(false);
// //     }

// //     IEnumerator ScaleIn(GameObject obj)
// //     {
// //         Transform t = obj.transform;
// //         Vector3 targetScale = t.localScale;
// //         t.localScale = Vector3.zero;
// //         float tVal = 0f;
// //         while (tVal < 1f)
// //         {
// //             tVal += Time.deltaTime / animDuration;
// //             t.localScale = Vector3.Lerp(Vector3.zero, targetScale, animCurve.Evaluate(tVal));
// //             yield return null;
// //         }
// //         t.localScale = targetScale;
// //     }

// //     // ================= PANEL SYSTEM ==========================================

// //     void Update()
// //     {
// //         if (panelOpen && Input.GetMouseButtonDown(0))
// //             if (!IsPointerOverPanel())
// //                 CloseCurrentPanel();
// //     }

// //     bool IsPointerOverPanel()
// //     {
// //         PointerEventData eventData = new PointerEventData(EventSystem.current)
// //         {
// //             position = Input.mousePosition
// //         };
// //         var results = new System.Collections.Generic.List<RaycastResult>();
// //         EventSystem.current.RaycastAll(eventData, results);

// //         foreach (var r in results)
// //             if (r.gameObject.transform.IsChildOf(panels[currentPanelIndex]))
// //                 return true;

// //         return false;
// //     }

// //     public void OpenPanel(int index)
// //     {
// //         if (panelOpen) return;

// //         panelOpen = true;
// //         currentPanelIndex = index;

// //         DisableAllButtons();
// //         DisableOtherObjects();

// //         RectTransform panel   = panels[index];
// //         RectTransform btnRect = utilityButtons[index].GetComponent<RectTransform>();

// //         panel.gameObject.SetActive(true);
// //         panel.position   = btnRect.position;
// //         panel.localScale = Vector3.zero;

// //         StartCoroutine(AnimatePanel(panel, panelCenterPos[index], panelOriginalScale[index], true));
// //     }

// //     public void CloseCurrentPanel()
// //     {
// //         if (currentPanelIndex == -1) return;

// //         RectTransform panel   = panels[currentPanelIndex];
// //         RectTransform btnRect = utilityButtons[currentPanelIndex].GetComponent<RectTransform>();

// //         StartCoroutine(AnimatePanel(panel, btnRect.position, Vector3.zero, false));
// //     }

// //     IEnumerator AnimatePanel(RectTransform panel, Vector3 targetPos,
// //                               Vector3 targetScale, bool opening)
// //     {
// //         Vector3 startPos   = panel.position;
// //         Vector3 startScale = panel.localScale;
// //         float t = 0f;

// //         while (t < 1f)
// //         {
// //             t += Time.unscaledDeltaTime / animDuration;
// //             float eval = animCurve.Evaluate(t);
// //             panel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
// //             panel.localScale = Vector3.Lerp(startScale, targetScale, eval);
// //             yield return null;
// //         }

// //         panel.position   = targetPos;
// //         panel.localScale = targetScale;

// //         if (!opening)
// //         {
// //             panel.gameObject.SetActive(false);
// //             EnableAllButtons();
// //             EnableOtherObjects();
// //             panelOpen = false;
// //             currentPanelIndex = -1;
// //         }
// //     }

// //     void DisableAllButtons()
// //     {
// //         foreach (Button btn in utilityButtons)
// //             if (btn != null) btn.gameObject.SetActive(false);
// //     }

// //     void EnableAllButtons()
// //     {
// //         foreach (Button btn in utilityButtons)
// //             if (btn != null) btn.gameObject.SetActive(true);
// //     }

// //     void DisableOtherObjects()
// //     {
// //         foreach (GameObject obj in objectsToDisable)
// //             if (obj != null) obj.SetActive(false);
// //     }

// //     void EnableOtherObjects()
// //     {
// //         foreach (GameObject obj in objectsToDisable)
// //             if (obj != null) obj.SetActive(true);
// //     }
// // }


// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using System.Collections;
// using TMPro;

// public class UtilityButtonManager : MonoBehaviour
// {
//     // ================= PANEL SYSTEM ==========================================

//     [Header("MAIN ARRAYS (Index must match)")]
//     public Button[] utilityButtons;
//     public RectTransform[] panels;

//     [Header("CLOSE BUTTONS (Index must match panels)")]
//     public Button[] closeButtons;

//     [Header("OTHER OBJECTS TO DISABLE")]
//     public GameObject[] objectsToDisable;

//     [Header("ANIMATION")]
//     public float animDuration = 0.35f;
//     public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

//     private Vector3[] panelCenterPos;
//     private Vector3[] panelOriginalScale;

//     private bool panelOpen = false;
//     private int  currentPanelIndex = -1;

//     // ================= PLAY BUTTON ===========================================

//     [Header("PLAY BUTTON")]
//     public Button playButton;

//     [Tooltip("GameManager assign karo — Play press hone pe StartGame() call hoga")]
//     public GameManager gameManager;

//     [Header("PLAY OBJECTS")]
//     public GameObject[] objectsToDisableOnPlay;
//     public GameObject[] objectsToEnableOnPlay;

//     // ================= SOUND TOGGLE ==========================================

//     [Header("SOUND TOGGLE")]
//     public Toggle soundToggle;

//     private const string SOUND_KEY = "SOUND_STATE";

//     // ================= PRIVACY POLICY & RESET GAME ==========================

//     [Header("PRIVACY POLICY BUTTON")]
//     public Button privacyPolicyButton;
//     public string privacyPolicyURL = "https://yourwebsite.com/privacy-policy";

//     [Header("RESET GAME BUTTON")]
//     public Button resetGameButton;

//     // ================= BUTTON SOUND ==========================================

//     [Header("Button Click Sound")]
//     public AudioSource buttonAudioSource;
//     public AudioClip   buttonClickSound;

//     [Header("IGNORE DEFAULT BUTTON SOUND PANELS")]
//     public GameObject[] ignoreSoundPanels;

//     // ================= PULSE ANIMATION =======================================

//     [Header("PULSE ANIMATION")]
//     public Button pulseButton;
//     public Image  pulseImage;

//     [Header("Pulse Settings")]
//     public float pulseRestMin     = 1.5f;
//     public float pulseRestMax     = 2.5f;
//     public float pulseSpeed       = 1.0f;

//     [Header("First Pop")]
//     public float firstPopScale    = 1.12f;
//     public float firstPopDuration = 0.14f;

//     [Header("Second Pop")]
//     public float secondPopScale    = 1.06f;
//     public float secondPopDuration = 0.11f;
//     public float gapBetweenPops    = 0.08f;

//     private Coroutine pulseCoroutine;
//     private Vector3   btnOriginalScale;
//     private Vector3   imgOriginalScale;

//     // ================= BEST SCORE & STAGE (Utility panel texts) ==============

//     [Header("BEST SCORE & STAGE TEXTS (Utility Panel)")]
//     [Tooltip("Utility panel mein Best Score dikhane wala TMP_Text")]
//     public TMP_Text utilityScoreText;

//     [Tooltip("Utility panel mein Best Stage dikhane wala TMP_Text")]
//     public TMP_Text utilityStageText;

//     // PlayerPrefs keys
//     private const string BEST_SCORE_KEY = "BEST_SCORE";
//     private const string BEST_STAGE_KEY = "BEST_STAGE";
//     private const string COINS_KEY       = "COINS";
//     public int openChallengePanelOnStart = 0;
    

//     // =========================================================================
//     void Start()
//     {
//         openChallengePanelOnStart = PlayerPrefs.GetInt("OpenChallengePanel", 0);

// if (openChallengePanelOnStart == 1)
// {
//     StartCoroutine(OpenChallengePanelFromGame());
//     PlayerPrefs.SetInt("OpenChallengePanel", 0);
//     PlayerPrefs.Save();
// }

//         int count = panels.Length;
//         panelCenterPos     = new Vector3[count];
//         panelOriginalScale = new Vector3[count];

//         for (int i = 0; i < count; i++)
//         {
//             int index = i;
//             panelCenterPos[i]     = panels[i].position;
//             panelOriginalScale[i] = panels[i].localScale;
//             panels[i].gameObject.SetActive(false);
//             utilityButtons[i].onClick.AddListener(() => OpenPanel(index));
//             if (closeButtons[i] != null)
//                 closeButtons[i].onClick.AddListener(CloseCurrentPanel);
//         }

//         if (playButton != null)
//             playButton.onClick.AddListener(OnPlayPressed);

//         if (privacyPolicyButton != null)
//             privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClick);

//         if (resetGameButton != null)
//             resetGameButton.onClick.AddListener(OnResetGameClick);

//         bool isOn = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
//         if (soundToggle != null)
//         {
//             soundToggle.isOn = isOn;
//             soundToggle.onValueChanged.AddListener(OnToggleChanged);
//         }
//         ApplySound(isOn);
//         AddClickSoundToAllButtons();
//         StartPulse();

//         // Saved best values load karke texts update karo
//         RefreshUtilityTexts();
//     }

//     // ================= BEST SCORE & STAGE ====================================

//     /// <summary>
//     /// GameManager2 game-over pe yeh call karta hai.
//     /// Agar naya score/stage purane se zyada hai to override karke save karo.
//     /// </summary>
//     public void UpdateBestScoreAndStage(int newScore, int newStage)
//     {
//         int savedScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
//         int savedStage = PlayerPrefs.GetInt(BEST_STAGE_KEY, 0);

//         bool changed = false;

//         if (newScore > savedScore)
//         {
//             PlayerPrefs.SetInt(BEST_SCORE_KEY, newScore);
//             changed = true;
//         }

//         if (newStage > savedStage)
//         {
//             PlayerPrefs.SetInt(BEST_STAGE_KEY, newStage);
//             changed = true;
//         }

//         if (changed)
//         {
//             PlayerPrefs.Save();
//             RefreshUtilityTexts();
//         }
//     }

//     /// PlayerPrefs se padh ke utility panel ke dono texts update karo
//    void RefreshUtilityTexts()
// {
//     int bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
//     int bestStage = PlayerPrefs.GetInt(BEST_STAGE_KEY, 0);

//     if (utilityScoreText != null)
//         utilityScoreText.text = "Score " + bestScore;

//     if (utilityStageText != null)
//         utilityStageText.text = "Stage " + bestStage;
// }

//     // ================= PRIVACY POLICY =======================================

//     void OnPrivacyPolicyClick()
//     {
//         Application.OpenURL(privacyPolicyURL);
//     }

//     // ================= RESET GAME ============================================

//     // void OnResetGameClick()
//     // {
//     //     // Score, Stage, Coins — sab PlayerPrefs se delete karo
//     //     PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
//     //     PlayerPrefs.DeleteKey(BEST_STAGE_KEY);
//     //     PlayerPrefs.DeleteKey(COINS_KEY);

//     //     // Agar aur bhi koi game data keys hain to yahan add karo
//     //     // PlayerPrefs.DeleteAll();  // ye sab kuch delete karta hai (sound setting bhi)

//     //     PlayerPrefs.Save();

//     //     // Texts bhi zero pe reset karo
//     //     RefreshUtilityTexts();

//     //     // GameManager bhi reset karo
//     //     if (gameManager != null)
//     //         gameManager.ReloadGame();

//     //     Debug.Log("Game Reset: Score, Stage, Coins sab clear ho gaye.");
//     // }

// void OnResetGameClick()
// {
//     // Best Score & Stage Reset
//     PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
//     PlayerPrefs.DeleteKey(BEST_STAGE_KEY);
//     PlayerPrefs.DeleteKey(COINS_KEY);

//     // Gems Reset
//     if (GemManager.Instance != null)
//     {
//         GemManager.Instance.ResetGems();
//     }

//     PlayerPrefs.Save();

//     // Utility Panel Text Refresh
//     RefreshUtilityTexts();

//     // Game Reload
//     if (gameManager != null)
//         gameManager.ReloadGame();

//     Debug.Log("Game Reset Complete");
// }
//     // ================= PULSE SYSTEM ==========================================

//     void StartPulse()
//     {
//         if (pulseButton == null && pulseImage == null) return;

//         btnOriginalScale = pulseButton != null
//             ? pulseButton.transform.localScale : Vector3.one;
//         imgOriginalScale = pulseImage != null
//             ? pulseImage.transform.localScale  : Vector3.one;

//         if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
//         pulseCoroutine = StartCoroutine(PulseLoop());
//     }

//     IEnumerator PulseLoop()
//     {
//         while (true)
//         {
//             float rest = Random.Range(pulseRestMin, pulseRestMax);
//             yield return new WaitForSeconds(rest);

//             yield return StartCoroutine(ScaleTo(
//                 btnOriginalScale, imgOriginalScale,
//                 firstPopScale, firstPopDuration / pulseSpeed, EaseOutBack));

//             yield return StartCoroutine(ScaleTo(
//                 btnOriginalScale * firstPopScale, imgOriginalScale * firstPopScale,
//                 1f / firstPopScale, 0.10f / pulseSpeed, EaseOutSine));

//             SetScale(btnOriginalScale, imgOriginalScale);

//             yield return new WaitForSeconds(gapBetweenPops);

//             yield return StartCoroutine(ScaleTo(
//                 btnOriginalScale, imgOriginalScale,
//                 secondPopScale, secondPopDuration / pulseSpeed, EaseOutBackSoft));

//             yield return StartCoroutine(ScaleTo(
//                 btnOriginalScale * secondPopScale, imgOriginalScale * secondPopScale,
//                 1f / secondPopScale, 0.13f / pulseSpeed, EaseOutSine));

//             SetScale(btnOriginalScale, imgOriginalScale);
//         }
//     }

//     IEnumerator ScaleTo(Vector3 fromBtn, Vector3 fromImg,
//                         float multiplier, float duration,
//                         System.Func<float, float> easeFn)
//     {
//         float t = 0f;
//         while (t < 1f)
//         {
//             t += Time.deltaTime / duration;
//             float eval = easeFn(Mathf.Clamp01(t));

//             if (pulseButton != null)
//                 pulseButton.transform.localScale =
//                     Vector3.LerpUnclamped(fromBtn, fromBtn * multiplier, eval);

//             if (pulseImage != null)
//                 pulseImage.transform.localScale =
//                     Vector3.LerpUnclamped(fromImg, fromImg * multiplier, eval);

//             yield return null;
//         }
//     }

//     void SetScale(Vector3 btn, Vector3 img)
//     {
//         if (pulseButton != null) pulseButton.transform.localScale = btn;
//         if (pulseImage  != null) pulseImage.transform.localScale  = img;
//     }

//     float EaseOutBack(float t)
//     {
//         float c1 = 1.70158f, c3 = c1 + 1f;
//         return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
//     }

//     float EaseOutBackSoft(float t)
//     {
//         float c1 = 0.8f, c3 = c1 + 1f;
//         return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
//     }

//     float EaseOutSine(float t) => Mathf.Sin((t * Mathf.PI) / 2f);

//     // ================= SOUND CONTROL =========================================

//     void OnToggleChanged(bool value)
//     {
//         PlayerPrefs.SetInt(SOUND_KEY, value ? 1 : 0);
//         PlayerPrefs.Save();
//         ApplySound(value);
//     }

//     void ApplySound(bool isOn) => AudioListener.volume = isOn ? 1f : 0f;

//     public static void PlaySound(AudioSource source, AudioClip clip)
//     {
//         if (AudioListener.volume == 0f || source == null || clip == null) return;
//         source.PlayOneShot(clip);
//     }

//     // ================= BUTTON SOUND ==========================================

//     void AddClickSoundToAllButtons()
//     {
//         Button[] allButtons = FindObjectsOfType<Button>(true);
//         foreach (Button btn in allButtons)
//         {
//             GameObject currentButton = btn.gameObject;
//             btn.onClick.AddListener(() => PlayButtonSound(currentButton));
//         }
//     }

//     void PlayButtonSound(GameObject clickedObject)
//     {
//         if (AudioListener.volume == 0f) return;

//         foreach (GameObject panel in ignoreSoundPanels)
//             if (panel != null && clickedObject.transform.IsChildOf(panel.transform)) return;

//         if (buttonAudioSource != null && buttonClickSound != null)
//             buttonAudioSource.PlayOneShot(buttonClickSound);
//     }

//     // ================= PLAY BUTTON ===========================================

//     void OnPlayPressed() => StartCoroutine(HandlePlayObjects());

//     IEnumerator HandlePlayObjects()
//     {
//         // Pehle UI objects animate karo
//         foreach (GameObject obj in objectsToDisableOnPlay)
//             if (obj != null) StartCoroutine(ScaleOut(obj));

//         foreach (GameObject obj in objectsToEnableOnPlay)
//             if (obj != null) { obj.SetActive(true); StartCoroutine(ScaleIn(obj)); }

//         // GameManager ko batao ki game shuru ho gaya — knife spawn hogi
//         if (gameManager != null)
//             gameManager.StartGame();

//         yield return null;
//     }

//     IEnumerator ScaleOut(GameObject obj)
//     {
//         Transform t = obj.transform;
//         Vector3 startScale = t.localScale;
//         float tVal = 0f;
//         while (tVal < 1f)
//         {
//             tVal += Time.deltaTime / animDuration;
//             t.localScale = Vector3.Lerp(startScale, Vector3.zero, animCurve.Evaluate(tVal));
//             yield return null;
//         }
//         obj.SetActive(false);
//     }

//     IEnumerator ScaleIn(GameObject obj)
//     {
//         Transform t = obj.transform;
//         Vector3 targetScale = t.localScale;
//         t.localScale = Vector3.zero;
//         float tVal = 0f;
//         while (tVal < 1f)
//         {
//             tVal += Time.deltaTime / animDuration;
//             t.localScale = Vector3.Lerp(Vector3.zero, targetScale, animCurve.Evaluate(tVal));
//             yield return null;
//         }
//         t.localScale = targetScale;
//     }

//     // ================= PANEL SYSTEM ==========================================

//     void Update()
//     {
//         if (panelOpen && Input.GetMouseButtonDown(0))
//             if (!IsPointerOverPanel())
//                 CloseCurrentPanel();
//     }

//     bool IsPointerOverPanel()
//     {
//         PointerEventData eventData = new PointerEventData(EventSystem.current)
//         {
//             position = Input.mousePosition
//         };
//         var results = new System.Collections.Generic.List<RaycastResult>();
//         EventSystem.current.RaycastAll(eventData, results);

//         foreach (var r in results)
//             if (r.gameObject.transform.IsChildOf(panels[currentPanelIndex]))
//                 return true;

//         return false;
//     }

//     public void OpenPanel(int index)
//     {
//         if (panelOpen) return;

//         panelOpen = true;
//         currentPanelIndex = index;

//         DisableAllButtons();
//         DisableOtherObjects();

//         RectTransform panel   = panels[index];
//         RectTransform btnRect = utilityButtons[index].GetComponent<RectTransform>();

//         panel.gameObject.SetActive(true);
//         panel.position   = btnRect.position;
//         panel.localScale = Vector3.zero;

//         StartCoroutine(AnimatePanel(panel, panelCenterPos[index], panelOriginalScale[index], true));
//     }

//     public void CloseCurrentPanel()
//     {
//         if (currentPanelIndex == -1) return;

//         RectTransform panel   = panels[currentPanelIndex];
//         RectTransform btnRect = utilityButtons[currentPanelIndex].GetComponent<RectTransform>();

//         StartCoroutine(AnimatePanel(panel, btnRect.position, Vector3.zero, false));
//     }

//     IEnumerator AnimatePanel(RectTransform panel, Vector3 targetPos,
//                               Vector3 targetScale, bool opening)
//     {
//         Vector3 startPos   = panel.position;
//         Vector3 startScale = panel.localScale;
//         float t = 0f;

//         while (t < 1f)
//         {
//             t += Time.unscaledDeltaTime / animDuration;
//             float eval = animCurve.Evaluate(t);
//             panel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
//             panel.localScale = Vector3.Lerp(startScale, targetScale, eval);
//             yield return null;
//         }

//         panel.position   = targetPos;
//         panel.localScale = targetScale;

//         if (!opening)
//         {
//             panel.gameObject.SetActive(false);
//             EnableAllButtons();
//             EnableOtherObjects();
//             panelOpen = false;
//             currentPanelIndex = -1;
//         }
//     }

//     void DisableAllButtons()
//     {
//         foreach (Button btn in utilityButtons)
//             if (btn != null) btn.gameObject.SetActive(false);
//     }

//     void EnableAllButtons()
//     {
//         foreach (Button btn in utilityButtons)
//             if (btn != null) btn.gameObject.SetActive(true);
//     }

//     void DisableOtherObjects()
//     {
//         foreach (GameObject obj in objectsToDisable)
//             if (obj != null) obj.SetActive(false);
//     }

//     void EnableOtherObjects()
//     {
//         foreach (GameObject obj in objectsToDisable)
//             if (obj != null) obj.SetActive(true);
//     }


//     IEnumerator OpenChallengePanelFromGame()
// {
//     yield return null; // wait 1 frame UI load

//     // Utility buttons safe disable state ensure
//     panelOpen = false;
//     currentPanelIndex = -1;

//     // Challenge panel open via your existing system
//     // assuming index 0 = challenge panel button
//     OpenPanel(2);
// }
// }



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class UtilityButtonManager : MonoBehaviour
{
    // ================= PANEL SYSTEM ==========================================

    [Header("MAIN ARRAYS (Index must match)")]
    public Button[] utilityButtons;
    public RectTransform[] panels;

    [Header("CLOSE BUTTONS (Index must match panels)")]
    public Button[] closeButtons;

    [Header("OTHER OBJECTS TO DISABLE")]
    public GameObject[] objectsToDisable;

    [Header("ANIMATION")]
    public float animDuration = 0.35f;
    public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3[] panelCenterPos;
    private Vector3[] panelOriginalScale;

    private bool panelOpen = false;
    private int  currentPanelIndex = -1;

    // ================= PLAY BUTTON ===========================================

    [Header("PLAY BUTTON")]
    public Button playButton;

    [Tooltip("GameManager assign karo — Play press hone pe StartGame() call hoga")]
    public GameManager gameManager;

    [Header("PLAY OBJECTS")]
    public GameObject[] objectsToDisableOnPlay;
    public GameObject[] objectsToEnableOnPlay;

    // ================= SOUND TOGGLE ==========================================

    [Header("SOUND TOGGLE")]
    public Toggle soundToggle;

    private const string SOUND_KEY = "SOUND_STATE";

    // ================= PRIVACY POLICY & RESET GAME ==========================

    [Header("PRIVACY POLICY BUTTON")]
    public Button privacyPolicyButton;
    public string privacyPolicyURL = "https://yourwebsite.com/privacy-policy";

    [Header("RESET GAME BUTTON")]
    public Button resetGameButton;

    // ================= BUTTON SOUND ==========================================

    [Header("Button Click Sound")]
    public AudioSource buttonAudioSource;
    public AudioClip   buttonClickSound;

    [Header("IGNORE DEFAULT BUTTON SOUND PANELS")]
    public GameObject[] ignoreSoundPanels;

    // ================= PULSE ANIMATION =======================================

    [Header("PULSE ANIMATION")]
    public Button pulseButton;
    public Image  pulseImage;

    [Header("Pulse Settings")]
    public float pulseRestMin     = 1.5f;
    public float pulseRestMax     = 2.5f;
    public float pulseSpeed       = 1.0f;

    [Header("First Pop")]
    public float firstPopScale    = 1.12f;
    public float firstPopDuration = 0.14f;

    [Header("Second Pop")]
    public float secondPopScale    = 1.06f;
    public float secondPopDuration = 0.11f;
    public float gapBetweenPops    = 0.08f;

    private Coroutine pulseCoroutine;
    private Vector3   btnOriginalScale;
    private Vector3   imgOriginalScale;

    // ================= BEST SCORE & STAGE (Utility panel texts) ==============

    [Header("BEST SCORE & STAGE TEXTS (Utility Panel)")]
    [Tooltip("Utility panel mein Best Score dikhane wala TMP_Text")]
    public TMP_Text utilityScoreText;

    [Tooltip("Utility panel mein Best Stage dikhane wala TMP_Text")]
    public TMP_Text utilityStageText;

    // PlayerPrefs keys
    private const string BEST_SCORE_KEY = "BEST_SCORE";
    private const string BEST_STAGE_KEY = "BEST_STAGE";
    private const string COINS_KEY       = "COINS";
    public int openChallengePanelOnStart = 0;

    public TMP_Text[] modeTexts;


    [System.Serializable]
public class ModeLevelText
{
    public string saveID;
    public TMP_Text levelText;
}

public ModeLevelText[] modeLevels;

[Header("ALL MODE SAVE IDS")]
public string[] modeSaveIDs;
    

    // =========================================================================
    void Start()
    {
        openChallengePanelOnStart = PlayerPrefs.GetInt("OpenChallengePanel", 0);

if (openChallengePanelOnStart == 1)
{
    StartCoroutine(OpenChallengePanelFromGame());
    PlayerPrefs.SetInt("OpenChallengePanel", 0);
    PlayerPrefs.Save();
}

        int count = panels.Length;
        panelCenterPos     = new Vector3[count];
        panelOriginalScale = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            int index = i;
            panelCenterPos[i]     = panels[i].position;
            panelOriginalScale[i] = panels[i].localScale;
            panels[i].gameObject.SetActive(false);
            utilityButtons[i].onClick.AddListener(() => OpenPanel(index));
            if (closeButtons[i] != null)
                closeButtons[i].onClick.AddListener(CloseCurrentPanel);
        }

        if (playButton != null)
            playButton.onClick.AddListener(OnPlayPressed);

        if (privacyPolicyButton != null)
            privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClick);

        if (resetGameButton != null)
            resetGameButton.onClick.AddListener(OnResetGameClick);

        bool isOn = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        if (soundToggle != null)
        {
            soundToggle.isOn = isOn;
            soundToggle.onValueChanged.AddListener(OnToggleChanged);
        }
        ApplySound(isOn);
        AddClickSoundToAllButtons();
        StartPulse();

        // Saved best values load karke texts update karo
        RefreshUtilityTexts();
        RefreshModeTexts();
    }

    // ================= BEST SCORE & STAGE ====================================

    /// <summary>
    /// GameManager2 game-over pe yeh call karta hai.
    /// Agar naya score/stage purane se zyada hai to override karke save karo.
    /// </summary>
    public void UpdateBestScoreAndStage(int newScore, int newStage)
    {
        int savedScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        int savedStage = PlayerPrefs.GetInt(BEST_STAGE_KEY, 0);

        bool changed = false;

        if (newScore > savedScore)
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY, newScore);
            changed = true;
        }

        if (newStage > savedStage)
        {
            PlayerPrefs.SetInt(BEST_STAGE_KEY, newStage);
            changed = true;
        }

        if (changed)
        {
            PlayerPrefs.Save();
            RefreshUtilityTexts();
        }
    }

    /// PlayerPrefs se padh ke utility panel ke dono texts update karo
   void RefreshUtilityTexts()
{
    int bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
    int bestStage = PlayerPrefs.GetInt(BEST_STAGE_KEY, 0);

    if (utilityScoreText != null)
        utilityScoreText.text = "Score " + bestScore;

    if (utilityStageText != null)
        utilityStageText.text = "Stage " + bestStage;
}

    // ================= PRIVACY POLICY =======================================

    void OnPrivacyPolicyClick()
    {
        Application.OpenURL(privacyPolicyURL);
    }

    // ================= RESET GAME ============================================

// void OnResetGameClick()
// {
//     // Best score data
//     PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
//     PlayerPrefs.DeleteKey(BEST_STAGE_KEY);
//     PlayerPrefs.DeleteKey(COINS_KEY);

//     // Challenge system
//     PlayerPrefs.DeleteKey("SavedLevel");
//     PlayerPrefs.DeleteKey("SavedChallenge");
//     PlayerPrefs.DeleteKey("WinChallenge");

//     PlayerPrefs.DeleteKey("CURRENT_MODE");
//     PlayerPrefs.DeleteKey("CURRENT_LEVEL");

//     // Mode levels
//     for (int i = 1; i <= 20; i++)
//     {
//         PlayerPrefs.DeleteKey("MODE_" + i + "_LEVEL");
//     }

//     // Gems reset
//     if (GemManager.Instance != null)
//     {
//         GemManager.Instance.ResetGems();
//     }

//     PlayerPrefs.Save();

//     RefreshUtilityTexts();
//     RefreshModeTexts();

//     if (gameManager != null)
//         gameManager.ReloadGame();

//     Debug.Log("Complete Game Reset");
// }
void OnResetGameClick()
{
    // Best score data
    PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
    PlayerPrefs.DeleteKey(BEST_STAGE_KEY);
    PlayerPrefs.DeleteKey(COINS_KEY);

    // Old save keys (optional)
    PlayerPrefs.DeleteKey("SavedLevel");
    PlayerPrefs.DeleteKey("SavedChallenge");
    PlayerPrefs.DeleteKey("WinChallenge");

    PlayerPrefs.DeleteKey("CURRENT_MODE");
    PlayerPrefs.DeleteKey("CURRENT_LEVEL");

    // Sabhi modes ka level aur challenge reset
    foreach (string id in modeSaveIDs)
    {
        PlayerPrefs.DeleteKey(id + "_Level");
        PlayerPrefs.DeleteKey(id + "_Challenge");
    }

    // Purana MODE_ system agar use hua ho
    for (int i = 1; i <= 20; i++)
    {
        PlayerPrefs.DeleteKey("MODE_" + i + "_LEVEL");
    }

    // Gems reset
    if (GemManager.Instance != null)
    {
        GemManager.Instance.ResetGems();
    }

    PlayerPrefs.Save();

    RefreshUtilityTexts();
    RefreshModeTexts();

    if (gameManager != null)
        gameManager.ReloadGame();

    Debug.Log("Complete Game Reset");
}


    // ================= PULSE SYSTEM ==========================================

    void StartPulse()
    {
        if (pulseButton == null && pulseImage == null) return;

        btnOriginalScale = pulseButton != null
            ? pulseButton.transform.localScale : Vector3.one;
        imgOriginalScale = pulseImage != null
            ? pulseImage.transform.localScale  : Vector3.one;

        if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
        pulseCoroutine = StartCoroutine(PulseLoop());
    }

    IEnumerator PulseLoop()
    {
        while (true)
        {
            float rest = Random.Range(pulseRestMin, pulseRestMax);
            yield return new WaitForSeconds(rest);

            yield return StartCoroutine(ScaleTo(
                btnOriginalScale, imgOriginalScale,
                firstPopScale, firstPopDuration / pulseSpeed, EaseOutBack));

            yield return StartCoroutine(ScaleTo(
                btnOriginalScale * firstPopScale, imgOriginalScale * firstPopScale,
                1f / firstPopScale, 0.10f / pulseSpeed, EaseOutSine));

            SetScale(btnOriginalScale, imgOriginalScale);

            yield return new WaitForSeconds(gapBetweenPops);

            yield return StartCoroutine(ScaleTo(
                btnOriginalScale, imgOriginalScale,
                secondPopScale, secondPopDuration / pulseSpeed, EaseOutBackSoft));

            yield return StartCoroutine(ScaleTo(
                btnOriginalScale * secondPopScale, imgOriginalScale * secondPopScale,
                1f / secondPopScale, 0.13f / pulseSpeed, EaseOutSine));

            SetScale(btnOriginalScale, imgOriginalScale);
        }
    }

    IEnumerator ScaleTo(Vector3 fromBtn, Vector3 fromImg,
                        float multiplier, float duration,
                        System.Func<float, float> easeFn)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eval = easeFn(Mathf.Clamp01(t));

            if (pulseButton != null)
                pulseButton.transform.localScale =
                    Vector3.LerpUnclamped(fromBtn, fromBtn * multiplier, eval);

            if (pulseImage != null)
                pulseImage.transform.localScale =
                    Vector3.LerpUnclamped(fromImg, fromImg * multiplier, eval);

            yield return null;
        }
    }

    void SetScale(Vector3 btn, Vector3 img)
    {
        if (pulseButton != null) pulseButton.transform.localScale = btn;
        if (pulseImage  != null) pulseImage.transform.localScale  = img;
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f, c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    float EaseOutBackSoft(float t)
    {
        float c1 = 0.8f, c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    float EaseOutSine(float t) => Mathf.Sin((t * Mathf.PI) / 2f);

    // ================= SOUND CONTROL =========================================

    void OnToggleChanged(bool value)
    {
        PlayerPrefs.SetInt(SOUND_KEY, value ? 1 : 0);
        PlayerPrefs.Save();
        ApplySound(value);
    }

    void ApplySound(bool isOn) => AudioListener.volume = isOn ? 1f : 0f;

    public static void PlaySound(AudioSource source, AudioClip clip)
    {
        if (AudioListener.volume == 0f || source == null || clip == null) return;
        source.PlayOneShot(clip);
    }

    // ================= BUTTON SOUND ==========================================

    void AddClickSoundToAllButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (Button btn in allButtons)
        {
            GameObject currentButton = btn.gameObject;
            btn.onClick.AddListener(() => PlayButtonSound(currentButton));
        }
    }

    void PlayButtonSound(GameObject clickedObject)
    {
        if (AudioListener.volume == 0f) return;

        foreach (GameObject panel in ignoreSoundPanels)
            if (panel != null && clickedObject.transform.IsChildOf(panel.transform)) return;

        if (buttonAudioSource != null && buttonClickSound != null)
            buttonAudioSource.PlayOneShot(buttonClickSound);
    }

    // ================= PLAY BUTTON ===========================================

    void OnPlayPressed() => StartCoroutine(HandlePlayObjects());

    IEnumerator HandlePlayObjects()
    {
        // Pehle UI objects animate karo
        foreach (GameObject obj in objectsToDisableOnPlay)
            if (obj != null) StartCoroutine(ScaleOut(obj));

        foreach (GameObject obj in objectsToEnableOnPlay)
            if (obj != null) { obj.SetActive(true); StartCoroutine(ScaleIn(obj)); }

        // GameManager ko batao ki game shuru ho gaya — knife spawn hogi
        if (gameManager != null)
            gameManager.StartGame();

        yield return null;
    }

    IEnumerator ScaleOut(GameObject obj)
    {
        Transform t = obj.transform;
        Vector3 startScale = t.localScale;
        float tVal = 0f;
        while (tVal < 1f)
        {
            tVal += Time.deltaTime / animDuration;
            t.localScale = Vector3.Lerp(startScale, Vector3.zero, animCurve.Evaluate(tVal));
            yield return null;
        }
        obj.SetActive(false);
    }

    IEnumerator ScaleIn(GameObject obj)
    {
        Transform t = obj.transform;
        Vector3 targetScale = t.localScale;
        t.localScale = Vector3.zero;
        float tVal = 0f;
        while (tVal < 1f)
        {
            tVal += Time.deltaTime / animDuration;
            t.localScale = Vector3.Lerp(Vector3.zero, targetScale, animCurve.Evaluate(tVal));
            yield return null;
        }
        t.localScale = targetScale;
    }

    // ================= PANEL SYSTEM ==========================================

    void Update()
    {
        if (panelOpen && Input.GetMouseButtonDown(0))
            if (!IsPointerOverPanel())
                CloseCurrentPanel();
    }

    bool IsPointerOverPanel()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
            if (r.gameObject.transform.IsChildOf(panels[currentPanelIndex]))
                return true;

        return false;
    }

    public void OpenPanel(int index)
    {
        if (panelOpen) return;

        panelOpen = true;
        currentPanelIndex = index;

        DisableAllButtons();
        DisableOtherObjects();

        RectTransform panel   = panels[index];
        RectTransform btnRect = utilityButtons[index].GetComponent<RectTransform>();

        panel.gameObject.SetActive(true);
        panel.position   = btnRect.position;
        panel.localScale = Vector3.zero;

        StartCoroutine(AnimatePanel(panel, panelCenterPos[index], panelOriginalScale[index], true));
         RefreshModeTexts();
    }

    public void CloseCurrentPanel()
    {
        if (currentPanelIndex == -1) return;

        RectTransform panel   = panels[currentPanelIndex];
        RectTransform btnRect = utilityButtons[currentPanelIndex].GetComponent<RectTransform>();

        StartCoroutine(AnimatePanel(panel, btnRect.position, Vector3.zero, false));
    }

    IEnumerator AnimatePanel(RectTransform panel, Vector3 targetPos,
                              Vector3 targetScale, bool opening)
    {
        Vector3 startPos   = panel.position;
        Vector3 startScale = panel.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animDuration;
            float eval = animCurve.Evaluate(t);
            panel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
            panel.localScale = Vector3.Lerp(startScale, targetScale, eval);
            yield return null;
        }

        panel.position   = targetPos;
        panel.localScale = targetScale;

        if (!opening)
        {
            panel.gameObject.SetActive(false);
            EnableAllButtons();
            EnableOtherObjects();
            panelOpen = false;
            currentPanelIndex = -1;
        }
    }

    void DisableAllButtons()
    {
        foreach (Button btn in utilityButtons)
            if (btn != null) btn.gameObject.SetActive(false);
    }

    void EnableAllButtons()
    {
        foreach (Button btn in utilityButtons)
            if (btn != null) btn.gameObject.SetActive(true);
    }

    void DisableOtherObjects()
    {
        foreach (GameObject obj in objectsToDisable)
            if (obj != null) obj.SetActive(false);
    }

    void EnableOtherObjects()
    {
        foreach (GameObject obj in objectsToDisable)
            if (obj != null) obj.SetActive(true);
    }


    IEnumerator OpenChallengePanelFromGame()
{
    yield return null; // wait 1 frame UI load

    // Utility buttons safe disable state ensure
    panelOpen = false;
    currentPanelIndex = -1;

    // Challenge panel open via your existing system
    // assuming index 0 = challenge panel button
    OpenPanel(2);
}

void RefreshModeTexts()
{
    foreach (ModeLevelText mode in modeLevels)
    {
        if (mode.levelText != null)
        {
            int level = PlayerPrefs.GetInt(mode.saveID + "_Level", 1);

            mode.levelText.text = "LEVEL " + level;
        }
    }
    
}

public static void RefreshSoundState()
{
    bool soundOn =
        PlayerPrefs.GetInt("SOUND_STATE", 1) == 1;

    AudioListener.volume = soundOn ? 1f : 0f;
}

}



