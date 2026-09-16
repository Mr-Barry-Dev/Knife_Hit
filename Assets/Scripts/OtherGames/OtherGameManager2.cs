// // // using UnityEngine;
// // // using UnityEngine.UI;
// // // using TMPro;
// // // using System.Collections;

// // // public class OtherGameManager2 : MonoBehaviour
// // // {
// // //     [Header("===== GAME MANAGER =====")]
// // //     public OtherGameManager gameManager;

// // //     [Header("===== CIRCLE =====")]
// // //     public SpriteRenderer circleRenderer;

// // //     [Header("===== CIRCLE SKINS =====")]
// // //     public Sprite[] circleSkins;

// // //     [Header("===== KNIFE COUNTS =====")]
// // //     public int[] knifeCounts;

// // //     [System.Serializable]
// // //     public class CircleHitEffect
// // //     {
// // //         public GameObject hitEffect;
// // //         public AudioClip hitSound;
// // //     }

// // //     [Header("===== ROTATION SCRIPTS =====")]
// // // public MonoBehaviour[] rotationScripts;
// // // private int[] rotationOrder;

// // // [Header("===== BOSS ROTATION =====")]
// // // public MonoBehaviour bossRotationScript;

// // //     [Header("===== CIRCLE HIT EFFECTS =====")]
// // //     public CircleHitEffect[] circleHitEffects;

// // //     [Header("===== AUDIO =====")]
// // //     public AudioSource audioSource;

// // //     [Header("===== CHALLENGE TEXTS =====")]
// // // public TextMeshProUGUI[] challengeTexts;

// // //     [Header("===== CURRENT CHALLENGE =====")]
// // //     public int currentChallenge = 0;

   

// // //     [System.Serializable]
// // //     public class StagePopup
// // //     {
// // //         public GameObject panel;
// // //         public TextMeshProUGUI stageText;

// // //         [Header("Sound")]
// // //         public AudioClip popupSound;
// // //     }

// // //     [Header("===== STAGE POPUPS =====")]
// // //     public StagePopup[] stagePopups;

// // //     [Header("===== PANEL ANIMATION =====")]
// // //     public float panelShowTime  = 1f;
// // //     public float animationTime  = 0.25f;

// // //     // ── Session tracking ─────────────────────────────────────────────────────
// // //     private int sessionScore = 0;
// // //     private int sessionStage = 1;

// // //     [Header("===== PRE ATTACHED KNIVES =====")]
// // //     public GameObject knifePrefab;
// // //     public float attachedKnifeRadius = 1.2f;
// // //     public float knifeInsertDepth = 0.25f;

// // //     [Tooltip("Last challenge ke alawa baaki mein knife attach hone ki chance (0=kabhi nahi, 1=hamesha)")]
// // //     [Range(0f, 1f)]
// // //     public float attachedKnifeSpawnChance = 0.5f;

// // //     // =========================================================================
// // //     // =================== PROGRESS DOTS (NEW) =================================
// // //     // =========================================================================

// // //     [Header("===== PROGRESS DOTS =====")]
// // //     [Tooltip("Har stage ke liye ek GameObject drag karo — circle, star, diamond kuch bhi")]
// // //     public GameObject[] progressDots;

// // //     [Tooltip("Inactive dot ka color (white)")]
// // //     public Color dotInactiveColor = Color.white;

// // //     [Tooltip("Completed dot ka color (yellow)")]
// // //     public Color dotActiveColor   = new Color(1f, 0.85f, 0f, 1f);

// // //     [Tooltip("Dot scale animation ka duration")]
// // //     public float dotAnimDuration  = 0.3f;

// // //     // Runtime pe cache hogi Images/SpriteRenderers
// // //     private Image[]          dotImages;
// // //     private SpriteRenderer[] dotSprites;

// // //     // =========================================================================
// // //     // =================== WIN PANEL (NEW) =====================================
// // //     // =========================================================================

// // //     [Header("===== WIN PANEL =====")]
// // //     [Tooltip("Win panel ka root GameObject")]
// // //     public GameObject winPanel;

// // //     [Tooltip("Challenge number dikhane wala TMP text  e.g. 'CHALLENGE 2'")]
// // //     public TextMeshProUGUI winChallengeText;

// // //     [Tooltip("Win panel pop-in animation duration")]
// // //     public float winPanelAnimDuration = 0.5f;

// // //     [Tooltip("Win ka sound")]
// // //     public AudioClip winSound;

// // //     [Tooltip("Win panel ke baad auto-hide ho (0 = nahi hoga)")]
// // //     public float winPanelAutoHideTime = 0f;

// // //    private int winChallengeNumber = 1;

// // //    private int savedLevel;

// // //    [Header("SAVE ID")]
// // //     public string saveID = "Desert";

// // //     // =========================================================================
// // //     void Start()
// // //     {
// // //        UtilityButtonManager.RefreshSoundState();
// // //       winChallengeNumber =
// // //     PlayerPrefs.GetInt(LevelKey, 1);
     
// // //      CreateRandomRotationOrder();
// // //     UpdateChallengeTexts();
// // //         NewSession();

// // //         foreach (StagePopup popup in stagePopups)
// // //             if (popup.panel != null)
// // //                 popup.panel.SetActive(false);

// // //         // Win panel band rakho shuru mein
// // //         if (winPanel != null)
// // //             winPanel.SetActive(false);

// // //         // Progress dots spawn karo
// // //         BuildProgressDots();

        
// // //     }

// // //     // ── Naya session ─────────────────────────────────────────────────────────
// // //     void NewSession()
// // //     {
// // //         sessionScore = 0;
// // //         sessionStage = 1;

// // //         // Hamesha 0 se shuru — order wise: skin[0], skin[1], skin[2]...
// // //         // (PlayerPrefs wala resume sirf NextChallenge/RepeatChallenge ke liye kaam karta hai)
// // //         currentChallenge =
// // //     PlayerPrefs.GetInt(ChallengeKey, 0);

// // //         LoadChallenge();
// // //     }

// // //     // =========================================================================
// // //     // =================== PROGRESS DOTS (NEW) =================================
// // //     // =========================================================================

// // //     // Inspector mein assign kiye GameObjects se Image/SpriteRenderer cache karo
// // //     void BuildProgressDots()
// // //     {
// // //         if (progressDots == null || progressDots.Length == 0) return;

// // //         dotImages  = new Image[progressDots.Length];
// // //         dotSprites = new SpriteRenderer[progressDots.Length];

// // //         for (int i = 0; i < progressDots.Length; i++)
// // //         {
// // //             if (progressDots[i] == null) continue;

// // //             // UI Image try karo pehle
// // //             Image img = progressDots[i].GetComponent<Image>();
// // //             if (img == null) img = progressDots[i].GetComponentInChildren<Image>();

// // //             if (img != null)
// // //             {
// // //                 dotImages[i] = img;
// // //                 img.color = dotInactiveColor;
// // //                 continue;
// // //             }

// // //             // Warna SpriteRenderer (world-space objects ke liye)
// // //             SpriteRenderer sr = progressDots[i].GetComponent<SpriteRenderer>();
// // //             if (sr == null) sr = progressDots[i].GetComponentInChildren<SpriteRenderer>();

// // //             if (sr != null)
// // //             {
// // //                 dotSprites[i] = sr;
// // //                 sr.color = dotInactiveColor;
// // //             }
// // //         }
// // //     }

// // //     // Sab dots ka color current state ke hisaab se set karo (animation ke bina)
// // //     void UpdateProgressDots()
// // //     {
// // //         if (progressDots == null) return;

// // //         for (int i = 0; i < progressDots.Length; i++)
// // //         {
// // //             Color c = (i < currentChallenge) ? dotActiveColor : dotInactiveColor;
// // //             SetDotColor(i, c);
// // //         }
// // //     }

// // //     // Ek dot ka color set karo (Image ya SpriteRenderer dono handle)
// // //     void SetDotColor(int i, Color c)
// // //     {
// // //         if (dotImages  != null && i < dotImages.Length  && dotImages[i]  != null)
// // //             dotImages[i].color = c;

// // //         if (dotSprites != null && i < dotSprites.Length && dotSprites[i] != null)
// // //             dotSprites[i].color = c;
// // //     }

// // //     // Dot ko scale animation ke saath yellow karo
// // //     IEnumerator AnimateDotComplete(int index)
// // //     {
// // //         if (progressDots == null || index >= progressDots.Length || progressDots[index] == null)
// // //             yield break;

// // //         Transform t = progressDots[index].transform;
// // //         float elapsed = 0f;

// // //         // Scale up
// // //         while (elapsed < dotAnimDuration * 0.5f)
// // //         {
// // //             elapsed += Time.deltaTime;
// // //             float s = Mathf.Lerp(1f, 1.5f, elapsed / (dotAnimDuration * 0.5f));
// // //             t.localScale = Vector3.one * s;
// // //             yield return null;
// // //         }

// // //         SetDotColor(index, dotActiveColor);
// // //         elapsed = 0f;

// // //         // Scale back
// // //         while (elapsed < dotAnimDuration * 0.5f)
// // //         {
// // //             elapsed += Time.deltaTime;
// // //             float s = Mathf.Lerp(1.5f, 1f, elapsed / (dotAnimDuration * 0.5f));
// // //             t.localScale = Vector3.one * s;
// // //             yield return null;
// // //         }

// // //         t.localScale = Vector3.one;
// // //     }

// // //     // =========================================================================
// // //     // =================== WIN PANEL (NEW) =====================================
// // //     // =========================================================================

    
// // //    void ShowWinPanel()
// // // {
// // //     if (winPanel == null)
// // //         return;

// // //     winPanel.SetActive(true);

// // //     if (winChallengeText != null)
// // //         winChallengeText.text = "Level "+ winChallengeNumber.ToString();

// // //     if (audioSource != null && winSound != null)
// // //         audioSource.PlayOneShot(winSound);

// // //     StartCoroutine(AnimateWinPanel());
// // // }

// // //     IEnumerator AnimateWinPanel()
// // //     {
// // //         if (winPanel == null) yield break;

// // //         Transform t = winPanel.transform;
// // //         float elapsed = 0f;

// // //         // Overshoot pop-in (scale 0 -> 1.15 -> 1)
// // //         while (elapsed < winPanelAnimDuration * 0.7f)
// // //         {
// // //             elapsed += Time.deltaTime;
// // //             float p = elapsed / (winPanelAnimDuration * 0.7f);
// // //             float s = Mathf.Lerp(0f, 1.15f, p);
// // //             t.localScale = Vector3.one * s;
// // //             yield return null;
// // //         }

// // //         elapsed = 0f;
// // //         while (elapsed < winPanelAnimDuration * 0.3f)
// // //         {
// // //             elapsed += Time.deltaTime;
// // //             float p = elapsed / (winPanelAnimDuration * 0.3f);
// // //             float s = Mathf.Lerp(1.15f, 1f, p);
// // //             t.localScale = Vector3.one * s;
// // //             yield return null;
// // //         }

// // //         t.localScale = Vector3.one;

// // //         // Auto-hide (agar set ho)
// // //         if (winPanelAutoHideTime > 0f)
// // //         {
// // //             yield return new WaitForSeconds(winPanelAutoHideTime);
// // //             winPanel.SetActive(false);
// // //         }
// // //     }

// // //     // =========================================================================
// // //     // =================== EXISTING METHODS ====================================
// // //     // =========================================================================

// // //     public void AddKnifeScore()
// // //     {
// // //         sessionScore += 2;
// // //     }

// // //     void LoadChallenge()
// // //     {
// // //         if (circleSkins.Length == 0) return;

// // //         if (currentChallenge >= circleSkins.Length)
// // //             currentChallenge = 0;

// // //         savedLevel = currentChallenge;
// // //         circleRenderer.sprite = circleSkins[currentChallenge];
// // //         UpdateRotation();

// // //         if (currentChallenge < knifeCounts.Length)
// // //         {
// // //             gameManager.totalKnives = knifeCounts[currentChallenge];
// // //             gameManager.stageTotalKnives = knifeCounts[currentChallenge];
// // //             gameManager.UpdateKnifeCountText();
// // //         }

// // //        UpdateChallengeTexts();

// // //         gameManager.ResetChallenge();
// // //         SpawnAttachedKnives();

// // //         // Dots update karo current state ke hisaab se
// // //         UpdateProgressDots();

        
        

// // //         // PlayerPrefs mein current challenge save karo
// // //        PlayerPrefs.SetInt(ChallengeKey, currentChallenge);
// // //         PlayerPrefs.Save();
// // //     }

// // //     public void ChallengeCompleted()
// // //     {
// // //         int completedStageIndex = currentChallenge; // dot index jo complete hua

// // //         int completedStage = currentChallenge + 1;
// // //         currentChallenge++;
// // //         sessionStage = currentChallenge + 1;

// // //         // Completed dot ko animate karo
// // //         StartCoroutine(AnimateDotComplete(completedStageIndex));

// // //         // Saare stages complete?
// // //         if (currentChallenge >= circleSkins.Length)
// // //         {
// // //             // WIN!
// // //             StartCoroutine(HandleWin());
// // //             return;
// // //         }
                    
// // //         LoadChallenge();
// // //         StartCoroutine(ShowStagePanel(completedStage, currentChallenge));
// // //     }

// // //     // Sab stages clear hone par Win sequence
// // //     IEnumerator HandleWin()
// // //     {
// // //         // Pehle last dot yellow ho jaye
// // //         yield return new WaitForSeconds(dotAnimDuration + 0.1f);

// // //         // Stage popup dikhao (last stage ka)
// // //         if (stagePopups.Length > 0)
// // //         {
// // //             int idx = Mathf.Clamp(currentChallenge - 1, 0, stagePopups.Length - 1);
// // //             yield return StartCoroutine(ShowStagePanel(currentChallenge, idx));
// // //         }

// // //         // Ab win panel
// // //         ShowWinPanel();
// // //     }

// // //     public void TriggerGameOver()
// // //     {
// // //         gameManager.displayScore = sessionScore;
// // //         gameManager.displayStage = sessionStage;

// // //         UtilityButtonManager utility = FindFirstObjectByType<UtilityButtonManager>();
// // //         if (utility != null)
// // //             utility.UpdateBestScoreAndStage(sessionScore, sessionStage);

// // //         gameManager.GameOver();
// // //     }

// // //     // ── Win panel: NEXT CHALLENGE button ──────────────────────────────────────
// // //    public void NextChallenge()
// // // {
// // //     if (winPanel != null)
// // //         winPanel.SetActive(false);

    

// // // winChallengeNumber++;

// // // SaveLevel();

// // // UpdateChallengeTexts();

// // //     if (currentChallenge >= circleSkins.Length)
// // //         currentChallenge = 0;

// // //     sessionScore = 0;
// // //     sessionStage = currentChallenge + 1;

   

// // //     LoadChallenge();

    
// // // }

// // //     // ── Win panel: REPEAT button ───────────────────────────────────────────────
// // //     public void RepeatChallenge()
// // //     {
// // //         if (winPanel != null)
// // //             winPanel.SetActive(false);

// // //         // currentChallenge ++ hua tha, ek peeche jao
// // //         currentChallenge = Mathf.Max(0, currentChallenge - 1);

// // //         sessionScore = 0;
// // //         sessionStage = currentChallenge + 1;

       

// // //         LoadChallenge();   // seed yahan reset hoga — isliye pehle seed restore karo

// // //         // Seed restore karke same pattern dobara shuru karo
       

        
// // //     }

// // //     // ── Legacy restart (pehle wala method, zaroorat pade to) ─────────────────
// // //     public void RestartFromChallenge1()
// // // {
// // //     if (winPanel != null)
// // //         winPanel.SetActive(false);

// // //     currentChallenge = 0;

// // //     winChallengeNumber = 1;

// // // SaveLevel();

// // // UpdateChallengeTexts();

// // //     NewSession();
// // //     UpdateProgressDots();
// // // }

// // //     public void ChallengeCompletedFromReload()
// // //     {
// // //         RestartFromChallenge1();
// // //     }

   

// // //     // ── Stage Panel ───────────────────────────────────────────────────────────
// // //     IEnumerator ShowStagePanel(int stageNumber, int challengeIndex)
// // //     {
// // //         if (stagePopups.Length == 0) yield break;

// // //         int index = Mathf.Clamp(challengeIndex, 0, stagePopups.Length - 1);
// // //         StagePopup popup = stagePopups[index];

// // //         if (popup.panel == null) yield break;

// // //         popup.panel.SetActive(true);

// // //         if (popup.stageText != null)
// // //             popup.stageText.text = "STAGE " + stageNumber;

// // //         if (audioSource != null && popup.popupSound != null)
// // //             audioSource.PlayOneShot(popup.popupSound);

// // //         yield return new WaitForSeconds(panelShowTime);

// // //         popup.panel.SetActive(false);
// // //     }

// // //     // ── Spawn Knives ──────────────────────────────────────────────────────────
// // //     void SpawnAttachedKnives()
// // //     {
// // //         if (knifePrefab == null) return;

// // //         int knifeCount = GetPreAttachedKnifeCount();
// // //         if (knifeCount <= 0) return;

// // //         float angleStep = 360f / knifeCount;

// // //         for (int i = 0; i < knifeCount; i++)
// // //         {
// // //             float angle = i * angleStep;
// // //             Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
// // //             Vector3 pos = circleRenderer.transform.position + dir * attachedKnifeRadius;
// // //             pos -= dir * knifeInsertDepth;

// // //             GameObject knife = Instantiate(knifePrefab, pos, Quaternion.identity);
// // //             knife.tag = "Knife";
// // //             knife.transform.rotation = Quaternion.Euler(0f, 0f, angle - 180f);
// // //             knife.transform.SetParent(circleRenderer.transform);

// // //             Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
// // //             if (rb != null)
// // //             {
// // //                 rb.linearVelocity = Vector2.zero;
// // //                 rb.angularVelocity = 0f;
// // //                 rb.bodyType = RigidbodyType2D.Kinematic;
// // //             }

// // //             Knife1 knifeScript = knife.GetComponent<Knife1>();
// // //             if (knifeScript != null)
// // //                 knifeScript.manager = gameManager;
// // //         }
// // //     }

// // //     int GetPreAttachedKnifeCount()
// // //     {
// // //         bool isLastStage = (currentChallenge == circleSkins.Length - 1);

// // //         if (isLastStage)
// // //         {
// // //             return Random.Range(1, 4);
// // //         }
// // //         else
// // //         {
// // //             return (Random.value <= attachedKnifeSpawnChance) ? Random.Range(1, 4) : 0;
// // //         }
// // //     }

// // //     public CircleHitEffect GetCurrentHitEffect()
// // //     {
// // //         if (circleHitEffects == null || circleHitEffects.Length == 0)
// // //             return null;

// // //         int index = Mathf.Clamp(currentChallenge, 0, circleHitEffects.Length - 1);
// // //         return circleHitEffects[index];
// // //     }

// // //     void UpdateChallengeTexts()
// // // {
// // //     foreach (TextMeshProUGUI txt in challengeTexts)
// // //     {
// // //         if (txt != null)
// // //             txt.text = "LEVEL " + winChallengeNumber;
// // //     }
// // // }

// // // void UpdateRotation()
// // // {
// // //     // Sab scripts disable karo
// // //     for (int i = 0; i < rotationScripts.Length; i++)
// // //     {
// // //         if (rotationScripts[i] != null)
// // //             rotationScripts[i].enabled = false;
// // //     }

// // //     // Boss bhi disable karo
// // //     if (bossRotationScript != null)
// // //         bossRotationScript.enabled = false;

// // //     // Last stage hai?
// // //     if (currentChallenge == circleSkins.Length - 1)
// // //     {
// // //         if (bossRotationScript != null)
// // //         {
// // //             bossRotationScript.enabled = true;

// // //             Debug.Log("Boss Rotation Enabled");
// // //         }

// // //         return;
// // //     }

// // //     // Normal random rotation
// // //     if (currentChallenge < rotationOrder.Length)
// // //     {
// // //         int randomIndex = rotationOrder[currentChallenge];

// // //         if (rotationScripts[randomIndex] != null)
// // //         {
// // //             rotationScripts[randomIndex].enabled = true;

// // //             Debug.Log("Enabled Random: " +
// // //                       rotationScripts[randomIndex].GetType().Name);
// // //         }
// // //     }
// // // }

// // // void CreateRandomRotationOrder()
// // // {
// // //     rotationOrder = new int[rotationScripts.Length];

// // //     for (int i = 0; i < rotationScripts.Length; i++)
// // //     {
// // //         rotationOrder[i] = i;
// // //     }

// // //     // Fisher-Yates shuffle
// // //     for (int i = 0; i < rotationOrder.Length; i++)
// // //     {
// // //         int rand = Random.Range(i, rotationOrder.Length);

// // //         int temp = rotationOrder[i];
// // //         rotationOrder[i] = rotationOrder[rand];
// // //         rotationOrder[rand] = temp;
// // //     }
// // // }

// // // void SaveLevel()
// // // {
// // //     PlayerPrefs.SetInt(LevelKey, winChallengeNumber);
// // //     PlayerPrefs.Save();
// // // }

// // // string LevelKey
// // // {
// // //     get { return saveID + "_Level"; }
// // // }

// // // string ChallengeKey
// // // {
// // //     get { return saveID + "_Challenge"; }
// // // }

// // // public void ResetProgress()
// // // {
// // //     currentChallenge = 0;
// // //     winChallengeNumber = 1;

// // //     PlayerPrefs.DeleteKey(LevelKey);
// // //     PlayerPrefs.DeleteKey(ChallengeKey);

// // //     UpdateChallengeTexts();
// // // }

// // // public void ReloadCurrentLevel()
// // // {
// // //     sessionScore = 0;

// // //     if (winPanel != null)
// // //         winPanel.SetActive(false);

// // //     LoadChallenge();
// // // }

// // // public void RestartLevel()
// // // {
// // //     sessionScore = 0;
// // //     sessionStage = 1;

// // //     currentChallenge = 0;

// // //     if (winPanel != null)
// // //         winPanel.SetActive(false);

// // //     LoadChallenge();
// // // }

// // // public int GetCurrentLevel()
// // // {
// // //     return winChallengeNumber;
   
// // // }


// // // }


// // using UnityEngine;
// // using UnityEngine.UI;
// // using TMPro;
// // using System.Collections;

// // public class OtherGameManager2 : MonoBehaviour
// // {
// //     [Header("===== GAME MANAGER =====")]
// //     public OtherGameManager gameManager;

// //     [Header("===== CIRCLE =====")]
// //     public SpriteRenderer circleRenderer;

// //     [Header("===== CIRCLE SKINS =====")]
// //     public Sprite[] circleSkins;

// //     [Header("===== KNIFE COUNTS =====")]
// //     public int[] knifeCounts;

// //     [System.Serializable]
// //     public class CircleHitEffect
// //     {
// //         public GameObject hitEffect;
// //         public AudioClip hitSound;
// //     }

// //     [Header("===== ROTATION SCRIPTS =====")]
// // public MonoBehaviour[] rotationScripts;
// // private int[] rotationOrder;

// // [Header("===== BOSS ROTATION =====")]
// // public MonoBehaviour bossRotationScript;

// //     [Header("===== CIRCLE HIT EFFECTS =====")]
// //     public CircleHitEffect[] circleHitEffects;

// //     [Header("===== AUDIO =====")]
// //     public AudioSource audioSource;

// //     [Header("===== CHALLENGE TEXTS =====")]
// // public TextMeshProUGUI[] challengeTexts;

// //     [Header("===== CURRENT CHALLENGE =====")]
// //     public int currentChallenge = 0;

   

// //     [System.Serializable]
// //     public class StagePopup
// //     {
// //         public GameObject panel;
// //         public TextMeshProUGUI stageText;

// //         [Header("Sound")]
// //         public AudioClip popupSound;
// //     }

// //     [Header("===== STAGE POPUPS =====")]
// //     public StagePopup[] stagePopups;

// //     [Header("===== PANEL ANIMATION =====")]
// //     public float panelShowTime  = 1f;
// //     public float animationTime  = 0.25f;

// //     // ── Session tracking ─────────────────────────────────────────────────────
// //     private int sessionScore = 0;
// //     private int sessionStage = 1;

// //     [Header("===== PRE ATTACHED KNIVES =====")]
// //     public GameObject knifePrefab;
// //     public float attachedKnifeRadius = 1.2f;
// //     public float knifeInsertDepth = 0.25f;

// //     [Tooltip("Last challenge ke alawa baaki mein knife attach hone ki chance (0=kabhi nahi, 1=hamesha)")]
// //     [Range(0f, 1f)]
// //     public float attachedKnifeSpawnChance = 0.5f;

// //     // =========================================================================
// //     // =================== PROGRESS DOTS =================================
// //     // =========================================================================

// //     [Header("===== PROGRESS DOTS =====")]
// //     [Tooltip("Har stage ke liye ek GameObject drag karo — circle, star, diamond kuch bhi")]
// //     public GameObject[] progressDots;

// //     [Tooltip("Inactive dot ka color (white)")]
// //     public Color dotInactiveColor = Color.white;

// //     [Tooltip("Completed dot ka color (yellow)")]
// //     public Color dotActiveColor   = new Color(1f, 0.85f, 0f, 1f);

// //     [Tooltip("Dot scale animation ka duration")]
// //     public float dotAnimDuration  = 0.3f;

// //     // Runtime pe cache hogi Images/SpriteRenderers
// //     private Image[]          dotImages;
// //     private SpriteRenderer[] dotSprites;

// //     // =========================================================================
// //     // =================== WIN PANEL =====================================
// //     // =========================================================================

// //     [Header("===== WIN PANEL =====")]
// //     [Tooltip("Win panel ka root GameObject")]
// //     public GameObject winPanel;

// //     [Tooltip("Challenge number dikhane wala TMP text  e.g. 'CHALLENGE 2'")]
// //     public TextMeshProUGUI winChallengeText;

// //     [Tooltip("Win panel pop-in animation duration")]
// //     public float winPanelAnimDuration = 0.5f;

// //     [Tooltip("Win ka sound")]
// //     public AudioClip winSound;

// //     [Tooltip("Win panel ke baad auto-hide ho (0 = nahi hoga)")]
// //     public float winPanelAutoHideTime = 0f;

// //    private int winChallengeNumber = 1;

// //    private int savedLevel;

// //    [Header("SAVE ID")]
// //     public string saveID = "Desert";

// //     // =========================================================================
// //     // =================== WIN SLIDER (NEW) =====================================
// //     // =========================================================================

// //     [Header("===== WIN SLIDER =====")]
// //     [Tooltip("UI > Slider object drag karo (Min Value = 0, Max Value = 1)")]
// //     public Slider winSlider;

// //     [Tooltip("Har win ke baad kitna fill badhega (0.25 = 4 wins mein 100%)")]
// //     [Range(0.01f, 1f)]
// //     public float winSliderFillStep = 0.25f;

// //     [Tooltip("Win panel show hone ke kitne second baad slider fill start hoga")]
// //     public float winSliderFillDelay = 2f;

// //     [Tooltip("Fill animation ka duration")]
// //     public float winSliderFillDuration = 0.4f;

// //     [Tooltip("Fill ke sath bajne wala sound (delay ke baad)")]
// //     public AudioClip winSliderSound;

// //     [Tooltip("Jab slider 100% ho jaye tab ye panel show hoga")]
// //     public GameObject sliderCompletePanel;

// //     [Tooltip("Sound jo slider-complete panel ke sath bajega (optional)")]
// //     public AudioClip sliderCompleteSound;

// //     [Tooltip("Slider-complete panel kitni der dikhega (0 = manual close, khud band nahi hoga)")]
// //     public float sliderCompletePanelAutoHideTime = 0f;

// //     private float currentSliderFill = 0f;

// //     // =========================================================================
// //     void Start()
// //     {
// //        UtilityButtonManager.RefreshSoundState();
// //       winChallengeNumber =
// //     PlayerPrefs.GetInt(LevelKey, 1);
     
// //      CreateRandomRotationOrder();
// //     UpdateChallengeTexts();
// //         NewSession();

// //         foreach (StagePopup popup in stagePopups)
// //             if (popup.panel != null)
// //                 popup.panel.SetActive(false);

// //         // Win panel band rakho shuru mein
// //         if (winPanel != null)
// //             winPanel.SetActive(false);

// //         // Slider-complete panel bhi band rakho shuru mein
// //         if (sliderCompletePanel != null)
// //             sliderCompletePanel.SetActive(false);

// //         // Saved slider progress load karo
// //         currentSliderFill = PlayerPrefs.GetFloat(SliderKey, 0f);
// //         if (winSlider != null)
// //             winSlider.value = currentSliderFill;

// //         // Progress dots spawn karo
// //         BuildProgressDots();

        
// //     }

// //     // ── Naya session ─────────────────────────────────────────────────────────
// //     void NewSession()
// //     {
// //         sessionScore = 0;
// //         sessionStage = 1;

// //         // Hamesha 0 se shuru — order wise: skin[0], skin[1], skin[2]...
// //         // (PlayerPrefs wala resume sirf NextChallenge/RepeatChallenge ke liye kaam karta hai)
// //         currentChallenge =
// //     PlayerPrefs.GetInt(ChallengeKey, 0);

// //         LoadChallenge();
// //     }

// //     // =========================================================================
// //     // =================== PROGRESS DOTS =================================
// //     // =========================================================================

// //     // Inspector mein assign kiye GameObjects se Image/SpriteRenderer cache karo
// //     void BuildProgressDots()
// //     {
// //         if (progressDots == null || progressDots.Length == 0) return;

// //         dotImages  = new Image[progressDots.Length];
// //         dotSprites = new SpriteRenderer[progressDots.Length];

// //         for (int i = 0; i < progressDots.Length; i++)
// //         {
// //             if (progressDots[i] == null) continue;

// //             // UI Image try karo pehle
// //             Image img = progressDots[i].GetComponent<Image>();
// //             if (img == null) img = progressDots[i].GetComponentInChildren<Image>();

// //             if (img != null)
// //             {
// //                 dotImages[i] = img;
// //                 img.color = dotInactiveColor;
// //                 continue;
// //             }

// //             // Warna SpriteRenderer (world-space objects ke liye)
// //             SpriteRenderer sr = progressDots[i].GetComponent<SpriteRenderer>();
// //             if (sr == null) sr = progressDots[i].GetComponentInChildren<SpriteRenderer>();

// //             if (sr != null)
// //             {
// //                 dotSprites[i] = sr;
// //                 sr.color = dotInactiveColor;
// //             }
// //         }
// //     }

// //     // Sab dots ka color current state ke hisaab se set karo (animation ke bina)
// //     void UpdateProgressDots()
// //     {
// //         if (progressDots == null) return;

// //         for (int i = 0; i < progressDots.Length; i++)
// //         {
// //             Color c = (i < currentChallenge) ? dotActiveColor : dotInactiveColor;
// //             SetDotColor(i, c);
// //         }
// //     }

// //     // Ek dot ka color set karo (Image ya SpriteRenderer dono handle)
// //     void SetDotColor(int i, Color c)
// //     {
// //         if (dotImages  != null && i < dotImages.Length  && dotImages[i]  != null)
// //             dotImages[i].color = c;

// //         if (dotSprites != null && i < dotSprites.Length && dotSprites[i] != null)
// //             dotSprites[i].color = c;
// //     }

// //     // Dot ko scale animation ke saath yellow karo
// //     IEnumerator AnimateDotComplete(int index)
// //     {
// //         if (progressDots == null || index >= progressDots.Length || progressDots[index] == null)
// //             yield break;

// //         Transform t = progressDots[index].transform;
// //         float elapsed = 0f;

// //         // Scale up
// //         while (elapsed < dotAnimDuration * 0.5f)
// //         {
// //             elapsed += Time.deltaTime;
// //             float s = Mathf.Lerp(1f, 1.5f, elapsed / (dotAnimDuration * 0.5f));
// //             t.localScale = Vector3.one * s;
// //             yield return null;
// //         }

// //         SetDotColor(index, dotActiveColor);
// //         elapsed = 0f;

// //         // Scale back
// //         while (elapsed < dotAnimDuration * 0.5f)
// //         {
// //             elapsed += Time.deltaTime;
// //             float s = Mathf.Lerp(1.5f, 1f, elapsed / (dotAnimDuration * 0.5f));
// //             t.localScale = Vector3.one * s;
// //             yield return null;
// //         }

// //         t.localScale = Vector3.one;
// //     }

// //     // =========================================================================
// //     // =================== WIN PANEL =====================================
// //     // =========================================================================

    
// //    void ShowWinPanel()
// // {
// //     if (winPanel == null)
// //         return;

// //     winPanel.SetActive(true);

// //     if (winChallengeText != null)
// //         winChallengeText.text = "Level "+ winChallengeNumber.ToString();

// //     if (audioSource != null && winSound != null)
// //         audioSource.PlayOneShot(winSound);

// //     StartCoroutine(AnimateWinPanel());

// //     // Win panel ke saath slider fill sequence bhi start karo
// //     StartCoroutine(HandleWinSliderFill());
// // }

// //     IEnumerator AnimateWinPanel()
// //     {
// //         if (winPanel == null) yield break;

// //         Transform t = winPanel.transform;
// //         float elapsed = 0f;

// //         // Overshoot pop-in (scale 0 -> 1.15 -> 1)
// //         while (elapsed < winPanelAnimDuration * 0.7f)
// //         {
// //             elapsed += Time.deltaTime;
// //             float p = elapsed / (winPanelAnimDuration * 0.7f);
// //             float s = Mathf.Lerp(0f, 1.15f, p);
// //             t.localScale = Vector3.one * s;
// //             yield return null;
// //         }

// //         elapsed = 0f;
// //         while (elapsed < winPanelAnimDuration * 0.3f)
// //         {
// //             elapsed += Time.deltaTime;
// //             float p = elapsed / (winPanelAnimDuration * 0.3f);
// //             float s = Mathf.Lerp(1.15f, 1f, p);
// //             t.localScale = Vector3.one * s;
// //             yield return null;
// //         }

// //         t.localScale = Vector3.one;

// //         // Auto-hide (agar set ho)
// //         if (winPanelAutoHideTime > 0f)
// //         {
// //             yield return new WaitForSeconds(winPanelAutoHideTime);
// //             winPanel.SetActive(false);
// //         }
// //     }

// //     // =========================================================================
// //     // =================== WIN SLIDER (NEW) =====================================
// //     // =========================================================================

// //     // Win panel show hone ke X sec baad, sound ke saath slider ko +step fill karta hai.
// //     // Agar fill 100% cross ho jaye to sliderCompletePanel dikhata hai aur slider reset karta hai.
// //     IEnumerator HandleWinSliderFill()
// //     {
// //         // Pehle delay (jab tak win panel dikh raha hai)
// //         yield return new WaitForSeconds(winSliderFillDelay);

// //         // Sound bajao
// //         if (audioSource != null && winSliderSound != null)
// //             audioSource.PlayOneShot(winSliderSound);

// //         float startFill  = currentSliderFill;
// //         float targetFill = currentSliderFill + winSliderFillStep;
// //         bool  willComplete = targetFill >= 1f;

// //         if (willComplete)
// //             targetFill = 1f;

// //         // Fill animate karo
// //         float elapsed = 0f;
// //         while (elapsed < winSliderFillDuration)
// //         {
// //             elapsed += Time.deltaTime;
// //             float p = elapsed / winSliderFillDuration;

// //             if (winSlider != null)
// //                 winSlider.value = Mathf.Lerp(startFill, targetFill, p);

// //             yield return null;
// //         }

// //         if (winSlider != null)
// //             winSlider.value = targetFill;

// //         currentSliderFill = targetFill;
// //         SaveSliderProgress();

// //         // Agar 100% ho gaya, panel dikhao aur reset karo
// //         if (willComplete)
// //         {
// //             yield return new WaitForSeconds(0.3f);
// //             ShowSliderCompletePanel();

// //             currentSliderFill = 0f;
// //             SaveSliderProgress();

// //             if (winSlider != null)
// //                 winSlider.value = 0f;
// //         }
// //     }

// //     void ShowSliderCompletePanel()
// //     {
// //         if (sliderCompletePanel == null) return;

// //         sliderCompletePanel.SetActive(true);

// //         if (audioSource != null && sliderCompleteSound != null)
// //             audioSource.PlayOneShot(sliderCompleteSound);

// //         if (sliderCompletePanelAutoHideTime > 0f)
// //             StartCoroutine(AutoHideSliderCompletePanel());
// //     }

// //     IEnumerator AutoHideSliderCompletePanel()
// //     {
// //         yield return new WaitForSeconds(sliderCompletePanelAutoHideTime);

// //         if (sliderCompletePanel != null)
// //             sliderCompletePanel.SetActive(false);
// //     }

// //     void SaveSliderProgress()
// //     {
// //         PlayerPrefs.SetFloat(SliderKey, currentSliderFill);
// //         PlayerPrefs.Save();
// //     }

// //     // Manually slider-complete panel band karne ke liye (button OnClick se call karo)
// //     public void CloseSliderCompletePanel()
// //     {
// //         if (sliderCompletePanel != null)
// //             sliderCompletePanel.SetActive(false);
// //     }

// //     // =========================================================================
// //     // =================== EXISTING METHODS ====================================
// //     // =========================================================================

// //     public void AddKnifeScore()
// //     {
// //         sessionScore += 2;
// //     }

// //     void LoadChallenge()
// //     {
// //         if (circleSkins.Length == 0) return;

// //         if (currentChallenge >= circleSkins.Length)
// //             currentChallenge = 0;

// //         savedLevel = currentChallenge;
// //         circleRenderer.sprite = circleSkins[currentChallenge];
// //         UpdateRotation();

// //         if (currentChallenge < knifeCounts.Length)
// //         {
// //             gameManager.totalKnives = knifeCounts[currentChallenge];
// //             gameManager.stageTotalKnives = knifeCounts[currentChallenge];
// //             gameManager.UpdateKnifeCountText();
// //         }

// //        UpdateChallengeTexts();

// //         gameManager.ResetChallenge();
// //         SpawnAttachedKnives();

// //         // Dots update karo current state ke hisaab se
// //         UpdateProgressDots();

        
        

// //         // PlayerPrefs mein current challenge save karo
// //        PlayerPrefs.SetInt(ChallengeKey, currentChallenge);
// //         PlayerPrefs.Save();
// //     }

// //     public void ChallengeCompleted()
// //     {
// //         int completedStageIndex = currentChallenge; // dot index jo complete hua

// //         int completedStage = currentChallenge + 1;
// //         currentChallenge++;
// //         sessionStage = currentChallenge + 1;

// //         // Completed dot ko animate karo
// //         StartCoroutine(AnimateDotComplete(completedStageIndex));

// //         // Saare stages complete?
// //         if (currentChallenge >= circleSkins.Length)
// //         {
// //             // WIN!
// //             StartCoroutine(HandleWin());
// //             return;
// //         }
                    
// //         LoadChallenge();
// //         StartCoroutine(ShowStagePanel(completedStage, currentChallenge));
// //     }

// //     // Sab stages clear hone par Win sequence
// //     IEnumerator HandleWin()
// //     {
// //         // Pehle last dot yellow ho jaye
// //         yield return new WaitForSeconds(dotAnimDuration + 0.1f);

// //         // Stage popup dikhao (last stage ka)
// //         if (stagePopups.Length > 0)
// //         {
// //             int idx = Mathf.Clamp(currentChallenge - 1, 0, stagePopups.Length - 1);
// //             yield return StartCoroutine(ShowStagePanel(currentChallenge, idx));
// //         }

// //         // Ab win panel
// //         ShowWinPanel();
// //     }

// //     public void TriggerGameOver()
// //     {
// //         gameManager.displayScore = sessionScore;
// //         gameManager.displayStage = sessionStage;

// //         UtilityButtonManager utility = FindFirstObjectByType<UtilityButtonManager>();
// //         if (utility != null)
// //             utility.UpdateBestScoreAndStage(sessionScore, sessionStage);

// //         gameManager.GameOver();
// //     }

// //     // ── Win panel: NEXT CHALLENGE button ──────────────────────────────────────
// //    public void NextChallenge()
// // {
// //     if (winPanel != null)
// //         winPanel.SetActive(false);

    

// // winChallengeNumber++;

// // SaveLevel();

// // UpdateChallengeTexts();

// //     if (currentChallenge >= circleSkins.Length)
// //         currentChallenge = 0;

// //     sessionScore = 0;
// //     sessionStage = currentChallenge + 1;

   

// //     LoadChallenge();

    
// // }

// //     // ── Win panel: REPEAT button ───────────────────────────────────────────────
// //     public void RepeatChallenge()
// //     {
// //         if (winPanel != null)
// //             winPanel.SetActive(false);

// //         // currentChallenge ++ hua tha, ek peeche jao
// //         currentChallenge = Mathf.Max(0, currentChallenge - 1);

// //         sessionScore = 0;
// //         sessionStage = currentChallenge + 1;

       

// //         LoadChallenge();   // seed yahan reset hoga — isliye pehle seed restore karo

// //         // Seed restore karke same pattern dobara shuru karo
       

        
// //     }

// //     // ── Legacy restart (pehle wala method, zaroorat pade to) ─────────────────
// //     public void RestartFromChallenge1()
// // {
// //     if (winPanel != null)
// //         winPanel.SetActive(false);

// //     currentChallenge = 0;

// //     winChallengeNumber = 1;

// // SaveLevel();

// // UpdateChallengeTexts();

// //     NewSession();
// //     UpdateProgressDots();
// // }

// //     public void ChallengeCompletedFromReload()
// //     {
// //         RestartFromChallenge1();
// //     }

   

// //     // ── Stage Panel ───────────────────────────────────────────────────────────
// //     IEnumerator ShowStagePanel(int stageNumber, int challengeIndex)
// //     {
// //         if (stagePopups.Length == 0) yield break;

// //         int index = Mathf.Clamp(challengeIndex, 0, stagePopups.Length - 1);
// //         StagePopup popup = stagePopups[index];

// //         if (popup.panel == null) yield break;

// //         popup.panel.SetActive(true);

// //         if (popup.stageText != null)
// //             popup.stageText.text = "STAGE " + stageNumber;

// //         if (audioSource != null && popup.popupSound != null)
// //             audioSource.PlayOneShot(popup.popupSound);

// //         yield return new WaitForSeconds(panelShowTime);

// //         popup.panel.SetActive(false);
// //     }

// //     // ── Spawn Knives ──────────────────────────────────────────────────────────
// //     void SpawnAttachedKnives()
// //     {
// //         if (knifePrefab == null) return;

// //         int knifeCount = GetPreAttachedKnifeCount();
// //         if (knifeCount <= 0) return;

// //         float angleStep = 360f / knifeCount;

// //         for (int i = 0; i < knifeCount; i++)
// //         {
// //             float angle = i * angleStep;
// //             Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
// //             Vector3 pos = circleRenderer.transform.position + dir * attachedKnifeRadius;
// //             pos -= dir * knifeInsertDepth;

// //             GameObject knife = Instantiate(knifePrefab, pos, Quaternion.identity);
// //             knife.tag = "Knife";
// //             knife.transform.rotation = Quaternion.Euler(0f, 0f, angle - 180f);
// //             knife.transform.SetParent(circleRenderer.transform);

// //             Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
// //             if (rb != null)
// //             {
// //                 rb.linearVelocity = Vector2.zero;
// //                 rb.angularVelocity = 0f;
// //                 rb.bodyType = RigidbodyType2D.Kinematic;
// //             }

// //             Knife1 knifeScript = knife.GetComponent<Knife1>();
// //             if (knifeScript != null)
// //                 knifeScript.manager = gameManager;
// //         }
// //     }

// //     int GetPreAttachedKnifeCount()
// //     {
// //         bool isLastStage = (currentChallenge == circleSkins.Length - 1);

// //         if (isLastStage)
// //         {
// //             return Random.Range(1, 4);
// //         }
// //         else
// //         {
// //             return (Random.value <= attachedKnifeSpawnChance) ? Random.Range(1, 4) : 0;
// //         }
// //     }

// //     public CircleHitEffect GetCurrentHitEffect()
// //     {
// //         if (circleHitEffects == null || circleHitEffects.Length == 0)
// //             return null;

// //         int index = Mathf.Clamp(currentChallenge, 0, circleHitEffects.Length - 1);
// //         return circleHitEffects[index];
// //     }

// //     void UpdateChallengeTexts()
// // {
// //     foreach (TextMeshProUGUI txt in challengeTexts)
// //     {
// //         if (txt != null)
// //             txt.text = "LEVEL " + winChallengeNumber;
// //     }
// // }

// // void UpdateRotation()
// // {
// //     // Sab scripts disable karo
// //     for (int i = 0; i < rotationScripts.Length; i++)
// //     {
// //         if (rotationScripts[i] != null)
// //             rotationScripts[i].enabled = false;
// //     }

// //     // Boss bhi disable karo
// //     if (bossRotationScript != null)
// //         bossRotationScript.enabled = false;

// //     // Last stage hai?
// //     if (currentChallenge == circleSkins.Length - 1)
// //     {
// //         if (bossRotationScript != null)
// //         {
// //             bossRotationScript.enabled = true;

// //             Debug.Log("Boss Rotation Enabled");
// //         }

// //         return;
// //     }

// //     // Normal random rotation
// //     if (currentChallenge < rotationOrder.Length)
// //     {
// //         int randomIndex = rotationOrder[currentChallenge];

// //         if (rotationScripts[randomIndex] != null)
// //         {
// //             rotationScripts[randomIndex].enabled = true;

// //             Debug.Log("Enabled Random: " +
// //                       rotationScripts[randomIndex].GetType().Name);
// //         }
// //     }
// // }

// // void CreateRandomRotationOrder()
// // {
// //     rotationOrder = new int[rotationScripts.Length];

// //     for (int i = 0; i < rotationScripts.Length; i++)
// //     {
// //         rotationOrder[i] = i;
// //     }

// //     // Fisher-Yates shuffle
// //     for (int i = 0; i < rotationOrder.Length; i++)
// //     {
// //         int rand = Random.Range(i, rotationOrder.Length);

// //         int temp = rotationOrder[i];
// //         rotationOrder[i] = rotationOrder[rand];
// //         rotationOrder[rand] = temp;
// //     }
// // }

// // void SaveLevel()
// // {
// //     PlayerPrefs.SetInt(LevelKey, winChallengeNumber);
// //     PlayerPrefs.Save();
// // }

// // string LevelKey
// // {
// //     get { return saveID + "_Level"; }
// // }

// // string ChallengeKey
// // {
// //     get { return saveID + "_Challenge"; }
// // }

// // string SliderKey
// // {
// //     get { return saveID + "_SliderFill"; }
// // }

// // public void ResetProgress()
// // {
// //     currentChallenge = 0;
// //     winChallengeNumber = 1;

// //     PlayerPrefs.DeleteKey(LevelKey);
// //     PlayerPrefs.DeleteKey(ChallengeKey);

// //     UpdateChallengeTexts();
// // }

// // public void ReloadCurrentLevel()
// // {
// //     sessionScore = 0;

// //     if (winPanel != null)
// //         winPanel.SetActive(false);

// //     LoadChallenge();
// // }

// // public void RestartLevel()
// // {
// //     sessionScore = 0;
// //     sessionStage = 1;

// //     currentChallenge = 0;

// //     if (winPanel != null)
// //         winPanel.SetActive(false);

// //     LoadChallenge();
// // }

// // public int GetCurrentLevel()
// // {
// //     return winChallengeNumber;
   
// // }


// // }



// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;

// public class OtherGameManager2 : MonoBehaviour
// {
//     [Header("===== GAME MANAGER =====")]
//     public OtherGameManager gameManager;

//     [Header("===== CIRCLE =====")]
//     public SpriteRenderer circleRenderer;

//     [Header("===== CIRCLE SKINS =====")]
//     public Sprite[] circleSkins;

//     [Header("===== KNIFE COUNTS =====")]
//     public int[] knifeCounts;

//     [System.Serializable]
//     public class CircleHitEffect
//     {
//         public GameObject hitEffect;
//         public AudioClip hitSound;
//     }

//     [Header("===== ROTATION SCRIPTS =====")]
// public MonoBehaviour[] rotationScripts;
// private int[] rotationOrder;

// [Header("===== BOSS ROTATION =====")]
// public MonoBehaviour bossRotationScript;

//     [Header("===== CIRCLE HIT EFFECTS =====")]
//     public CircleHitEffect[] circleHitEffects;

//     [Header("===== AUDIO =====")]
//     public AudioSource audioSource;

//     [Header("===== CHALLENGE TEXTS =====")]
// public TextMeshProUGUI[] challengeTexts;

//     [Header("===== CURRENT CHALLENGE =====")]
//     public int currentChallenge = 0;

   

//     [System.Serializable]
//     public class StagePopup
//     {
//         public GameObject panel;
//         public TextMeshProUGUI stageText;

//         [Header("Sound")]
//         public AudioClip popupSound;
//     }

//     [Header("===== STAGE POPUPS =====")]
//     public StagePopup[] stagePopups;

//     [Header("===== PANEL ANIMATION =====")]
//     public float panelShowTime  = 1f;
//     public float animationTime  = 0.25f;

//     // ── Session tracking ─────────────────────────────────────────────────────
//     private int sessionScore = 0;
//     private int sessionStage = 1;

//     [Header("===== PRE ATTACHED KNIVES =====")]
//     public GameObject knifePrefab;
//     public float attachedKnifeRadius = 1.2f;
//     public float knifeInsertDepth = 0.25f;

//     [Tooltip("Last challenge ke alawa baaki mein knife attach hone ki chance (0=kabhi nahi, 1=hamesha)")]
//     [Range(0f, 1f)]
//     public float attachedKnifeSpawnChance = 0.5f;

//     // =========================================================================
//     // =================== PROGRESS DOTS =================================
//     // =========================================================================

//     [Header("===== PROGRESS DOTS =====")]
//     [Tooltip("Har stage ke liye ek GameObject drag karo — circle, star, diamond kuch bhi")]
//     public GameObject[] progressDots;

//     [Tooltip("Inactive dot ka color (white)")]
//     public Color dotInactiveColor = Color.white;

//     [Tooltip("Completed dot ka color (yellow)")]
//     public Color dotActiveColor   = new Color(1f, 0.85f, 0f, 1f);

//     [Tooltip("Dot scale animation ka duration")]
//     public float dotAnimDuration  = 0.3f;

//     // Runtime pe cache hogi Images/SpriteRenderers
//     private Image[]          dotImages;
//     private SpriteRenderer[] dotSprites;

//     // =========================================================================
//     // =================== WIN PANEL =====================================
//     // =========================================================================

//     [Header("===== WIN PANEL =====")]
//     [Tooltip("Win panel ka root GameObject")]
//     public GameObject winPanel;

//     [Tooltip("Challenge number dikhane wala TMP text  e.g. 'CHALLENGE 2'")]
//     public TextMeshProUGUI winChallengeText;

//     [Tooltip("Win panel pop-in animation duration")]
//     public float winPanelAnimDuration = 0.5f;

//     [Tooltip("Win ka sound")]
//     public AudioClip winSound;

//     [Tooltip("Win panel ke baad auto-hide ho (0 = nahi hoga)")]
//     public float winPanelAutoHideTime = 0f;

//    private int winChallengeNumber = 1;

//    private int savedLevel;

//    [Header("SAVE ID")]
//     public string saveID = "Desert";

//     // =========================================================================
//     // =================== WIN SLIDER (NEW) =====================================
//     // =========================================================================

//     [Header("===== WIN SLIDER =====")]
//     [Tooltip("UI > Slider object drag karo (Min Value = 0, Max Value = 1)")]
//     public Slider winSlider;

//     [Tooltip("Har win ke baad kitna fill badhega (0.25 = 4 wins mein 100%)")]
//     [Range(0.01f, 1f)]
//     public float winSliderFillStep = 0.25f;

//     [Tooltip("Win panel show hone ke kitne second baad slider fill start hoga")]
//     public float winSliderFillDelay = 2f;

//     [Tooltip("Fill animation ka duration")]
//     public float winSliderFillDuration = 0.4f;

//     [Tooltip("Fill ke sath bajne wala sound (delay ke baad)")]
//     public AudioClip winSliderSound;

//     [Tooltip("Jab slider 100% ho jaye tab ye panel show hoga")]
//     public GameObject sliderCompletePanel;

//     [Tooltip("Sound jo slider-complete panel ke sath bajega (optional)")]
//     public AudioClip sliderCompleteSound;

//     [Tooltip("Slider-complete panel kitni der dikhega (0 = manual close, khud band nahi hoga)")]
//     public float sliderCompletePanelAutoHideTime = 0f;

//     private float currentSliderFill = 0f;

//     // =========================================================================
//     void Start()
//     {
//        UtilityButtonManager.RefreshSoundState();
//       winChallengeNumber =
//     PlayerPrefs.GetInt(LevelKey, 1);
     
//      CreateRandomRotationOrder();
//     UpdateChallengeTexts();
//         NewSession();

//         foreach (StagePopup popup in stagePopups)
//             if (popup.panel != null)
//                 popup.panel.SetActive(false);

//         // Win panel band rakho shuru mein
//         if (winPanel != null)
//             winPanel.SetActive(false);

//         // Slider-complete panel bhi band rakho shuru mein
//         if (sliderCompletePanel != null)
//             sliderCompletePanel.SetActive(false);

//         // Saved slider progress load karo
//         currentSliderFill = PlayerPrefs.GetFloat(SliderKey, 0f);
//         if (winSlider != null)
//             winSlider.value = currentSliderFill;

//         // Progress dots spawn karo
//         BuildProgressDots();

        
//     }

//     // ── Naya session ─────────────────────────────────────────────────────────
//     void NewSession()
//     {
//         sessionScore = 0;
//         sessionStage = 1;

//         // Hamesha 0 se shuru — order wise: skin[0], skin[1], skin[2]...
//         // (PlayerPrefs wala resume sirf NextChallenge/RepeatChallenge ke liye kaam karta hai)
//         currentChallenge =
//     PlayerPrefs.GetInt(ChallengeKey, 0);

//         LoadChallenge();
//     }

//     // =========================================================================
//     // =================== PROGRESS DOTS =================================
//     // =========================================================================

//     // Inspector mein assign kiye GameObjects se Image/SpriteRenderer cache karo
//     void BuildProgressDots()
//     {
//         if (progressDots == null || progressDots.Length == 0) return;

//         dotImages  = new Image[progressDots.Length];
//         dotSprites = new SpriteRenderer[progressDots.Length];

//         for (int i = 0; i < progressDots.Length; i++)
//         {
//             if (progressDots[i] == null) continue;

//             // UI Image try karo pehle
//             Image img = progressDots[i].GetComponent<Image>();
//             if (img == null) img = progressDots[i].GetComponentInChildren<Image>();

//             if (img != null)
//             {
//                 dotImages[i] = img;
//                 img.color = dotInactiveColor;
//                 continue;
//             }

//             // Warna SpriteRenderer (world-space objects ke liye)
//             SpriteRenderer sr = progressDots[i].GetComponent<SpriteRenderer>();
//             if (sr == null) sr = progressDots[i].GetComponentInChildren<SpriteRenderer>();

//             if (sr != null)
//             {
//                 dotSprites[i] = sr;
//                 sr.color = dotInactiveColor;
//             }
//         }
//     }

//     // Sab dots ka color current state ke hisaab se set karo (animation ke bina)
//     void UpdateProgressDots()
//     {
//         if (progressDots == null) return;

//         for (int i = 0; i < progressDots.Length; i++)
//         {
//             Color c = (i < currentChallenge) ? dotActiveColor : dotInactiveColor;
//             SetDotColor(i, c);
//         }
//     }

//     // Ek dot ka color set karo (Image ya SpriteRenderer dono handle)
//     void SetDotColor(int i, Color c)
//     {
//         if (dotImages  != null && i < dotImages.Length  && dotImages[i]  != null)
//             dotImages[i].color = c;

//         if (dotSprites != null && i < dotSprites.Length && dotSprites[i] != null)
//             dotSprites[i].color = c;
//     }

//     // Dot ko scale animation ke saath yellow karo
//     IEnumerator AnimateDotComplete(int index)
//     {
//         if (progressDots == null || index >= progressDots.Length || progressDots[index] == null)
//             yield break;

//         Transform t = progressDots[index].transform;
//         float elapsed = 0f;

//         // Scale up
//         while (elapsed < dotAnimDuration * 0.5f)
//         {
//             elapsed += Time.deltaTime;
//             float s = Mathf.Lerp(1f, 1.5f, elapsed / (dotAnimDuration * 0.5f));
//             t.localScale = Vector3.one * s;
//             yield return null;
//         }

//         SetDotColor(index, dotActiveColor);
//         elapsed = 0f;

//         // Scale back
//         while (elapsed < dotAnimDuration * 0.5f)
//         {
//             elapsed += Time.deltaTime;
//             float s = Mathf.Lerp(1.5f, 1f, elapsed / (dotAnimDuration * 0.5f));
//             t.localScale = Vector3.one * s;
//             yield return null;
//         }

//         t.localScale = Vector3.one;
//     }

//     // =========================================================================
//     // =================== WIN PANEL =====================================
//     // =========================================================================

    
//    void ShowWinPanel()
// {
//     if (winPanel == null)
//         return;

//     winPanel.SetActive(true);

//     if (winChallengeText != null)
//         winChallengeText.text = "Level "+ winChallengeNumber.ToString();

//     if (audioSource != null && winSound != null)
//         audioSource.PlayOneShot(winSound);

//     StartCoroutine(AnimateWinPanel());

//     // Win panel ke saath slider fill sequence bhi start karo
//     StartCoroutine(HandleWinSliderFill());
// }

//     IEnumerator AnimateWinPanel()
//     {
//         if (winPanel == null) yield break;

//         Transform t = winPanel.transform;
//         float elapsed = 0f;

//         // Overshoot pop-in (scale 0 -> 1.15 -> 1)
//         while (elapsed < winPanelAnimDuration * 0.7f)
//         {
//             elapsed += Time.deltaTime;
//             float p = elapsed / (winPanelAnimDuration * 0.7f);
//             float s = Mathf.Lerp(0f, 1.15f, p);
//             t.localScale = Vector3.one * s;
//             yield return null;
//         }

//         elapsed = 0f;
//         while (elapsed < winPanelAnimDuration * 0.3f)
//         {
//             elapsed += Time.deltaTime;
//             float p = elapsed / (winPanelAnimDuration * 0.3f);
//             float s = Mathf.Lerp(1.15f, 1f, p);
//             t.localScale = Vector3.one * s;
//             yield return null;
//         }

//         t.localScale = Vector3.one;

//         // Auto-hide (agar set ho)
//         if (winPanelAutoHideTime > 0f)
//         {
//             yield return new WaitForSeconds(winPanelAutoHideTime);
//             winPanel.SetActive(false);
//         }
//     }

//     // =========================================================================
//     // =================== WIN SLIDER (NEW) =====================================
//     // =========================================================================

//     // Win panel show hone ke X sec baad, sound ke saath slider ko +step fill karta hai.
//     // Agar fill 100% cross ho jaye to sliderCompletePanel dikhata hai aur slider reset karta hai.
//     IEnumerator HandleWinSliderFill()
//     {
//         // Pehle delay (jab tak win panel dikh raha hai)
//         yield return new WaitForSeconds(winSliderFillDelay);

//         // Sound bajao
//         if (audioSource != null && winSliderSound != null)
//             audioSource.PlayOneShot(winSliderSound);

//         float startFill  = currentSliderFill;
//         float targetFill = currentSliderFill + winSliderFillStep;
//         bool  willComplete = targetFill >= 1f;

//         if (willComplete)
//             targetFill = 1f;

//         // Fill animate karo
//         float elapsed = 0f;
//         while (elapsed < winSliderFillDuration)
//         {
//             elapsed += Time.deltaTime;
//             float p = elapsed / winSliderFillDuration;

//             if (winSlider != null)
//                 winSlider.value = Mathf.Lerp(startFill, targetFill, p);

//             yield return null;
//         }

//         if (winSlider != null)
//             winSlider.value = targetFill;

//         currentSliderFill = targetFill;
//         SaveSliderProgress();

//         // Agar 100% ho gaya, panel dikhao aur reset karo
//         if (willComplete)
//         {
//             yield return new WaitForSeconds(0.3f);
//             ShowSliderCompletePanel();

//             currentSliderFill = 0f;
//             SaveSliderProgress();

//             if (winSlider != null)
//                 winSlider.value = 0f;
//         }
//     }

//     void ShowSliderCompletePanel()
//     {
//         if (sliderCompletePanel == null) return;

//         sliderCompletePanel.SetActive(true);

//         if (audioSource != null && sliderCompleteSound != null)
//             audioSource.PlayOneShot(sliderCompleteSound);

//         if (sliderCompletePanelAutoHideTime > 0f)
//             StartCoroutine(AutoHideSliderCompletePanel());
//     }

//     IEnumerator AutoHideSliderCompletePanel()
//     {
//         yield return new WaitForSeconds(sliderCompletePanelAutoHideTime);

//         if (sliderCompletePanel != null)
//             sliderCompletePanel.SetActive(false);
//     }

//     void SaveSliderProgress()
//     {
//         PlayerPrefs.SetFloat(SliderKey, currentSliderFill);
//         PlayerPrefs.Save();
//     }

//     // Manually slider-complete panel band karne ke liye (button OnClick se call karo)
//     public void CloseSliderCompletePanel()
//     {
//         if (sliderCompletePanel != null)
//             sliderCompletePanel.SetActive(false);
//     }

//     // =========================================================================
//     // =================== EXISTING METHODS ====================================
//     // =========================================================================

//     public void AddKnifeScore()
//     {
//         sessionScore += 2;
//     }

//     void LoadChallenge()
//     {
//         if (circleSkins.Length == 0) return;

//         if (currentChallenge >= circleSkins.Length)
//             currentChallenge = 0;

//         savedLevel = currentChallenge;
//         circleRenderer.sprite = circleSkins[currentChallenge];
//         UpdateRotation();

//         if (currentChallenge < knifeCounts.Length)
//         {
//             gameManager.totalKnives = knifeCounts[currentChallenge];
//             gameManager.stageTotalKnives = knifeCounts[currentChallenge];
//             gameManager.UpdateKnifeCountText();
//         }

//        UpdateChallengeTexts();

//         gameManager.ResetChallenge();
//         SpawnAttachedKnives();

//         // Dots update karo current state ke hisaab se
//         UpdateProgressDots();

        
        

//         // PlayerPrefs mein current challenge save karo
//        PlayerPrefs.SetInt(ChallengeKey, currentChallenge);
//         PlayerPrefs.Save();
//     }

//     public void ChallengeCompleted()
//     {
//         int completedStageIndex = currentChallenge; // dot index jo complete hua

//         int completedStage = currentChallenge + 1;
//         currentChallenge++;
//         sessionStage = currentChallenge + 1;

//         // Completed dot ko animate karo
//         StartCoroutine(AnimateDotComplete(completedStageIndex));

//         // Saare stages complete?
//         if (currentChallenge >= circleSkins.Length)
//         {
//             // WIN!
//             StartCoroutine(HandleWin());
//             return;
//         }
                    
//         LoadChallenge();
//         StartCoroutine(ShowStagePanel(completedStage, currentChallenge));
//     }

//     // Sab stages clear hone par Win sequence
//     IEnumerator HandleWin()
//     {
//         // Pehle last dot yellow ho jaye
//         yield return new WaitForSeconds(dotAnimDuration + 0.1f);

//         // Stage popup dikhao (last stage ka)
//         if (stagePopups.Length > 0)
//         {
//             int idx = Mathf.Clamp(currentChallenge - 1, 0, stagePopups.Length - 1);
//             yield return StartCoroutine(ShowStagePanel(currentChallenge, idx));
//         }

//         // Ab win panel
//         ShowWinPanel();
//     }

//     public void TriggerGameOver()
//     {
//         gameManager.displayScore = sessionScore;
//         gameManager.displayStage = sessionStage;

//         UtilityButtonManager utility = FindFirstObjectByType<UtilityButtonManager>();
//         if (utility != null)
//             utility.UpdateBestScoreAndStage(sessionScore, sessionStage);

//         gameManager.GameOver();
//     }

//     // ── Win panel: NEXT CHALLENGE button ──────────────────────────────────────
//    public void NextChallenge()
// {
//     if (winPanel != null)
//         winPanel.SetActive(false);

    

// winChallengeNumber++;

// SaveLevel();

// UpdateChallengeTexts();

//     if (currentChallenge >= circleSkins.Length)
//         currentChallenge = 0;

//     sessionScore = 0;
//     sessionStage = currentChallenge + 1;

   

//     LoadChallenge();

    
// }

//     // ── Win panel: REPEAT button ───────────────────────────────────────────────
//     public void RepeatChallenge()
//     {
//         if (winPanel != null)
//             winPanel.SetActive(false);

//         // currentChallenge ++ hua tha, ek peeche jao
//         currentChallenge = Mathf.Max(0, currentChallenge - 1);

//         sessionScore = 0;
//         sessionStage = currentChallenge + 1;

       

//         LoadChallenge();   // seed yahan reset hoga — isliye pehle seed restore karo

//         // Seed restore karke same pattern dobara shuru karo
       

        
//     }

//     // ── Legacy restart (pehle wala method, zaroorat pade to) ─────────────────
//     public void RestartFromChallenge1()
// {
//     if (winPanel != null)
//         winPanel.SetActive(false);

//     currentChallenge = 0;

//     winChallengeNumber = 1;

// SaveLevel();

// UpdateChallengeTexts();

//     NewSession();
//     UpdateProgressDots();
// }

//     public void ChallengeCompletedFromReload()
//     {
//         RestartFromChallenge1();
//     }

   

//     // ── Stage Panel ───────────────────────────────────────────────────────────
//     IEnumerator ShowStagePanel(int stageNumber, int challengeIndex)
//     {
//         if (stagePopups.Length == 0) yield break;

//         int index = Mathf.Clamp(challengeIndex, 0, stagePopups.Length - 1);
//         StagePopup popup = stagePopups[index];

//         if (popup.panel == null) yield break;

//         // Panel dikhte hi knife throw block karo
//         if (gameManager != null)
//             gameManager.inputBlocked = true;

//         popup.panel.SetActive(true);

//         if (popup.stageText != null)
//             popup.stageText.text = "STAGE " + stageNumber;

//         if (audioSource != null && popup.popupSound != null)
//             audioSource.PlayOneShot(popup.popupSound);

//         yield return new WaitForSeconds(panelShowTime);

//         popup.panel.SetActive(false);

//         // Panel hide hote hi wapas throw allow karo
//         if (gameManager != null)
//             gameManager.inputBlocked = false;
//     }

//     // ── Spawn Knives ──────────────────────────────────────────────────────────
//     void SpawnAttachedKnives()
//     {
//         if (knifePrefab == null) return;

//         int knifeCount = GetPreAttachedKnifeCount();
//         if (knifeCount <= 0) return;

//         float angleStep = 360f / knifeCount;

//         for (int i = 0; i < knifeCount; i++)
//         {
//             float angle = i * angleStep;
//             Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
//             Vector3 pos = circleRenderer.transform.position + dir * attachedKnifeRadius;
//             pos -= dir * knifeInsertDepth;

//             GameObject knife = Instantiate(knifePrefab, pos, Quaternion.identity);
//             knife.tag = "Knife";
//             knife.transform.rotation = Quaternion.Euler(0f, 0f, angle - 180f);
//             knife.transform.SetParent(circleRenderer.transform);

//             Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
//             if (rb != null)
//             {
//                 rb.linearVelocity = Vector2.zero;
//                 rb.angularVelocity = 0f;
//                 rb.bodyType = RigidbodyType2D.Kinematic;
//             }

//             Knife1 knifeScript = knife.GetComponent<Knife1>();
//             if (knifeScript != null)
//                 knifeScript.manager = gameManager;
//         }
//     }

//     int GetPreAttachedKnifeCount()
//     {
//         bool isLastStage = (currentChallenge == circleSkins.Length - 1);

//         if (isLastStage)
//         {
//             return Random.Range(1, 4);
//         }
//         else
//         {
//             return (Random.value <= attachedKnifeSpawnChance) ? Random.Range(1, 4) : 0;
//         }
//     }

//     public CircleHitEffect GetCurrentHitEffect()
//     {
//         if (circleHitEffects == null || circleHitEffects.Length == 0)
//             return null;

//         int index = Mathf.Clamp(currentChallenge, 0, circleHitEffects.Length - 1);
//         return circleHitEffects[index];
//     }

//     void UpdateChallengeTexts()
// {
//     foreach (TextMeshProUGUI txt in challengeTexts)
//     {
//         if (txt != null)
//             txt.text = "LEVEL " + winChallengeNumber;
//     }
// }

// void UpdateRotation()
// {
//     // Sab scripts disable karo
//     for (int i = 0; i < rotationScripts.Length; i++)
//     {
//         if (rotationScripts[i] != null)
//             rotationScripts[i].enabled = false;
//     }

//     // Boss bhi disable karo
//     if (bossRotationScript != null)
//         bossRotationScript.enabled = false;

//     // Last stage hai?
//     if (currentChallenge == circleSkins.Length - 1)
//     {
//         if (bossRotationScript != null)
//         {
//             bossRotationScript.enabled = true;

//             Debug.Log("Boss Rotation Enabled");
//         }

//         return;
//     }

//     // Normal random rotation
//     if (currentChallenge < rotationOrder.Length)
//     {
//         int randomIndex = rotationOrder[currentChallenge];

//         if (rotationScripts[randomIndex] != null)
//         {
//             rotationScripts[randomIndex].enabled = true;

//             Debug.Log("Enabled Random: " +
//                       rotationScripts[randomIndex].GetType().Name);
//         }
//     }
// }

// void CreateRandomRotationOrder()
// {
//     rotationOrder = new int[rotationScripts.Length];

//     for (int i = 0; i < rotationScripts.Length; i++)
//     {
//         rotationOrder[i] = i;
//     }

//     // Fisher-Yates shuffle
//     for (int i = 0; i < rotationOrder.Length; i++)
//     {
//         int rand = Random.Range(i, rotationOrder.Length);

//         int temp = rotationOrder[i];
//         rotationOrder[i] = rotationOrder[rand];
//         rotationOrder[rand] = temp;
//     }
// }

// void SaveLevel()
// {
//     PlayerPrefs.SetInt(LevelKey, winChallengeNumber);
//     PlayerPrefs.Save();
// }

// string LevelKey
// {
//     get { return saveID + "_Level"; }
// }

// string ChallengeKey
// {
//     get { return saveID + "_Challenge"; }
// }

// string SliderKey
// {
//     get { return saveID + "_SliderFill"; }
// }

// public void ResetProgress()
// {
//     currentChallenge = 0;
//     winChallengeNumber = 1;

//     PlayerPrefs.DeleteKey(LevelKey);
//     PlayerPrefs.DeleteKey(ChallengeKey);

//     UpdateChallengeTexts();
// }

// public void ReloadCurrentLevel()
// {
//     sessionScore = 0;

//     if (winPanel != null)
//         winPanel.SetActive(false);

//     LoadChallenge();
// }

// public void RestartLevel()
// {
//     sessionScore = 0;
//     sessionStage = 1;

//     currentChallenge = 0;

//     if (winPanel != null)
//         winPanel.SetActive(false);

//     LoadChallenge();
// }

// public int GetCurrentLevel()
// {
//     return winChallengeNumber;
   
// }


// }




using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class OtherGameManager2 : MonoBehaviour
{
    [Header("===== GAME MANAGER =====")]
    public OtherGameManager gameManager;

    [Header("===== CIRCLE =====")]
    public SpriteRenderer circleRenderer;

    [Header("===== CIRCLE SKINS =====")]
    public Sprite[] circleSkins;

    [Header("===== KNIFE COUNTS =====")]
    public int[] knifeCounts;

    [System.Serializable]
    public class CircleHitEffect
    {
        public GameObject hitEffect;
        public AudioClip hitSound;
    }

    [Header("===== ROTATION SCRIPTS =====")]
public MonoBehaviour[] rotationScripts;
private int[] rotationOrder;

[Header("===== BOSS ROTATION =====")]
public MonoBehaviour bossRotationScript;

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

    // =========================================================================
    // =================== PROGRESS DOTS =================================
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

    // =========================================================================
    // =================== WIN PANEL =====================================
    // =========================================================================

    [Header("===== WIN PANEL =====")]
    [Tooltip("Win panel ka root GameObject")]
    public GameObject winPanel;

    [Tooltip("Challenge number dikhane wala TMP text  e.g. 'CHALLENGE 2'")]
    public TextMeshProUGUI winChallengeText;

    [Tooltip("Win panel pop-in animation duration")]
    public float winPanelAnimDuration = 0.5f;

    [Tooltip("Win ka sound")]
    public AudioClip winSound;

    [Tooltip("Win panel ke baad auto-hide ho (0 = nahi hoga)")]
    public float winPanelAutoHideTime = 0f;

   private int winChallengeNumber = 1;

   private int savedLevel;

   [Header("SAVE ID")]
    public string saveID = "Desert";

    // =========================================================================
    // =================== WIN SLIDER (NEW) =====================================
    // =========================================================================

    [Header("===== WIN SLIDER =====")]
    [Tooltip("UI > Slider object drag karo (Min Value = 0, Max Value = 1)")]
    public Slider winSlider;

    [Tooltip("Har win ke baad kitna fill badhega (0.25 = 4 wins mein 100%)")]
    [Range(0.01f, 1f)]
    public float winSliderFillStep = 0.25f;

    [Tooltip("Win panel show hone ke kitne second baad slider fill start hoga")]
    public float winSliderFillDelay = 2f;

    [Tooltip("Fill animation ka duration")]
    public float winSliderFillDuration = 0.4f;

    [Tooltip("Fill ke sath bajne wala sound (delay ke baad)")]
    public AudioClip winSliderSound;

    [Tooltip("Jab slider 100% ho jaye tab ye panel show hoga")]
    public GameObject sliderCompletePanel;

    [Tooltip("Sound jo slider-complete panel ke sath bajega (optional)")]
    public AudioClip sliderCompleteSound;

    [Tooltip("Slider-complete panel kitni der dikhega (0 = manual close, khud band nahi hoga)")]
    public float sliderCompletePanelAutoHideTime = 0f;

    private float currentSliderFill = 0f;

    // =========================================================================
    void Start()
    {
       UtilityButtonManager.RefreshSoundState();
      winChallengeNumber =
    PlayerPrefs.GetInt(LevelKey, 1);
     
     CreateRandomRotationOrder();
    UpdateChallengeTexts();
        NewSession();

        foreach (StagePopup popup in stagePopups)
            if (popup.panel != null)
                popup.panel.SetActive(false);

        // Win panel band rakho shuru mein
        if (winPanel != null)
            winPanel.SetActive(false);

        // Slider-complete panel bhi band rakho shuru mein
        if (sliderCompletePanel != null)
            sliderCompletePanel.SetActive(false);

        // Saved slider progress load karo
        currentSliderFill = PlayerPrefs.GetFloat(SliderKey, 0f);
        if (winSlider != null)
            winSlider.value = currentSliderFill;

        // Progress dots spawn karo
        BuildProgressDots();

        
    }

    // ── Naya session ─────────────────────────────────────────────────────────
    void NewSession()
    {
        sessionScore = 0;
        sessionStage = 1;

        // Hamesha 0 se shuru — har app open/session pe order wise: skin[0], skin[1], skin[2]...
        // PlayerPrefs se resume NAHI karna — warna kabhi last saved (last skin) load ho jata tha.
        currentChallenge = 0;

        LoadChallenge();
    }

    // =========================================================================
    // =================== PROGRESS DOTS =================================
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
    void UpdateProgressDots()
    {
        if (progressDots == null) return;

        for (int i = 0; i < progressDots.Length; i++)
        {
            Color c = (i < currentChallenge) ? dotActiveColor : dotInactiveColor;
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

    // =========================================================================
    // =================== WIN PANEL =====================================
    // =========================================================================

    
   void ShowWinPanel()
{
    if (winPanel == null)
        return;

    winPanel.SetActive(true);

    if (winChallengeText != null)
        winChallengeText.text = "Level "+ winChallengeNumber.ToString();

    if (audioSource != null && winSound != null)
        audioSource.PlayOneShot(winSound);

    StartCoroutine(AnimateWinPanel());

    // Win panel ke saath slider fill sequence bhi start karo
    StartCoroutine(HandleWinSliderFill());
}

    IEnumerator AnimateWinPanel()
    {
        if (winPanel == null) yield break;

        Transform t = winPanel.transform;
        float elapsed = 0f;

        // Overshoot pop-in (scale 0 -> 1.15 -> 1)
        while (elapsed < winPanelAnimDuration * 0.7f)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / (winPanelAnimDuration * 0.7f);
            float s = Mathf.Lerp(0f, 1.15f, p);
            t.localScale = Vector3.one * s;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < winPanelAnimDuration * 0.3f)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / (winPanelAnimDuration * 0.3f);
            float s = Mathf.Lerp(1.15f, 1f, p);
            t.localScale = Vector3.one * s;
            yield return null;
        }

        t.localScale = Vector3.one;

        // Auto-hide (agar set ho)
        if (winPanelAutoHideTime > 0f)
        {
            yield return new WaitForSeconds(winPanelAutoHideTime);
            winPanel.SetActive(false);
        }
    }

    // =========================================================================
    // =================== WIN SLIDER (NEW) =====================================
    // =========================================================================

    // Win panel show hone ke X sec baad, sound ke saath slider ko +step fill karta hai.
    // Agar fill 100% cross ho jaye to sliderCompletePanel dikhata hai aur slider reset karta hai.
    IEnumerator HandleWinSliderFill()
    {
        // Pehle delay (jab tak win panel dikh raha hai)
        yield return new WaitForSeconds(winSliderFillDelay);

        // Sound bajao
        if (audioSource != null && winSliderSound != null)
            audioSource.PlayOneShot(winSliderSound);

        float startFill  = currentSliderFill;
        float targetFill = currentSliderFill + winSliderFillStep;
        bool  willComplete = targetFill >= 1f;

        if (willComplete)
            targetFill = 1f;

        // Fill animate karo
        float elapsed = 0f;
        while (elapsed < winSliderFillDuration)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / winSliderFillDuration;

            if (winSlider != null)
                winSlider.value = Mathf.Lerp(startFill, targetFill, p);

            yield return null;
        }

        if (winSlider != null)
            winSlider.value = targetFill;

        currentSliderFill = targetFill;
        SaveSliderProgress();

        // Agar 100% ho gaya, panel dikhao aur reset karo
        if (willComplete)
        {
            yield return new WaitForSeconds(0.3f);
            ShowSliderCompletePanel();

            currentSliderFill = 0f;
            SaveSliderProgress();

            if (winSlider != null)
                winSlider.value = 0f;
        }
    }

    void ShowSliderCompletePanel()
    {
        if (sliderCompletePanel == null) return;

        sliderCompletePanel.SetActive(true);

        if (audioSource != null && sliderCompleteSound != null)
            audioSource.PlayOneShot(sliderCompleteSound);

        if (sliderCompletePanelAutoHideTime > 0f)
            StartCoroutine(AutoHideSliderCompletePanel());
    }

    IEnumerator AutoHideSliderCompletePanel()
    {
        yield return new WaitForSeconds(sliderCompletePanelAutoHideTime);

        if (sliderCompletePanel != null)
            sliderCompletePanel.SetActive(false);
    }

    void SaveSliderProgress()
    {
        PlayerPrefs.SetFloat(SliderKey, currentSliderFill);
        PlayerPrefs.Save();
    }

    // Manually slider-complete panel band karne ke liye (button OnClick se call karo)
    public void CloseSliderCompletePanel()
    {
        if (sliderCompletePanel != null)
            sliderCompletePanel.SetActive(false);
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
        if (circleSkins.Length == 0) return;

        if (currentChallenge >= circleSkins.Length)
            currentChallenge = 0;

        savedLevel = currentChallenge;
        circleRenderer.sprite = circleSkins[currentChallenge];
        UpdateRotation();

        if (currentChallenge < knifeCounts.Length)
        {
            gameManager.totalKnives = knifeCounts[currentChallenge];
            gameManager.stageTotalKnives = knifeCounts[currentChallenge];
            gameManager.UpdateKnifeCountText();
        }

       UpdateChallengeTexts();

        gameManager.ResetChallenge();
        SpawnAttachedKnives();

        // Dots update karo current state ke hisaab se
        UpdateProgressDots();

        
        

        // PlayerPrefs mein current challenge save karo
       PlayerPrefs.SetInt(ChallengeKey, currentChallenge);
        PlayerPrefs.Save();
    }

    public void ChallengeCompleted()
    {
        int completedStageIndex = currentChallenge; // dot index jo complete hua

        int completedStage = currentChallenge + 1;
        currentChallenge++;
        sessionStage = currentChallenge + 1;

        // Completed dot ko animate karo
        StartCoroutine(AnimateDotComplete(completedStageIndex));

        // Saare stages complete?
        if (currentChallenge >= circleSkins.Length)
        {
            // WIN! — turant ChallengeKey ko 0 save karo, taaki agli baar app
            // open karne pe (chahe "Next Challenge" dabaya ho ya nahi) hamesha
            // pehle skin (step 0) se hi shuru ho, last skin se nahi
            PlayerPrefs.SetInt(ChallengeKey, 0);
            PlayerPrefs.Save();

            StartCoroutine(HandleWin());
            return;
        }
                    
        LoadChallenge();
        StartCoroutine(ShowStagePanel(completedStage, currentChallenge));
    }

    // Sab stages clear hone par Win sequence
    IEnumerator HandleWin()
    {
        // Pehle last dot yellow ho jaye
        yield return new WaitForSeconds(dotAnimDuration + 0.1f);

        // Stage popup dikhao (last stage ka)
        if (stagePopups.Length > 0)
        {
            int idx = Mathf.Clamp(currentChallenge - 1, 0, stagePopups.Length - 1);
            yield return StartCoroutine(ShowStagePanel(currentChallenge, idx));
        }

        // Ab win panel
        ShowWinPanel();
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
    if (winPanel != null)
        winPanel.SetActive(false);

    

winChallengeNumber++;

SaveLevel();

UpdateChallengeTexts();

    if (currentChallenge >= circleSkins.Length)
        currentChallenge = 0;

    sessionScore = 0;
    sessionStage = currentChallenge + 1;

   

    LoadChallenge();

    
}

    // ── Win panel: REPEAT button ───────────────────────────────────────────────
    public void RepeatChallenge()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        // currentChallenge ++ hua tha, ek peeche jao
        currentChallenge = Mathf.Max(0, currentChallenge - 1);

        sessionScore = 0;
        sessionStage = currentChallenge + 1;

       

        LoadChallenge();   // seed yahan reset hoga — isliye pehle seed restore karo

        // Seed restore karke same pattern dobara shuru karo
       

        
    }

    // ── Legacy restart (pehle wala method, zaroorat pade to) ─────────────────
    public void RestartFromChallenge1()
{
    if (winPanel != null)
        winPanel.SetActive(false);

    currentChallenge = 0;

    winChallengeNumber = 1;

SaveLevel();

UpdateChallengeTexts();

    NewSession();
    UpdateProgressDots();
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

        // Panel dikhte hi knife throw block karo
        if (gameManager != null)
            gameManager.inputBlocked = true;

        popup.panel.SetActive(true);

        if (popup.stageText != null)
            popup.stageText.text = "STAGE " + stageNumber;

        if (audioSource != null && popup.popupSound != null)
            audioSource.PlayOneShot(popup.popupSound);

        yield return new WaitForSeconds(panelShowTime);

        popup.panel.SetActive(false);

        // Panel hide hote hi wapas throw allow karo
        if (gameManager != null)
            gameManager.inputBlocked = false;
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

            Knife1 knifeScript = knife.GetComponent<Knife1>();
            if (knifeScript != null)
                knifeScript.manager = gameManager;
        }
    }

    int GetPreAttachedKnifeCount()
    {
        bool isLastStage = (currentChallenge == circleSkins.Length - 1);

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

    // Boss bhi disable karo
    if (bossRotationScript != null)
        bossRotationScript.enabled = false;

    // Last stage hai?
    if (currentChallenge == circleSkins.Length - 1)
    {
        if (bossRotationScript != null)
        {
            bossRotationScript.enabled = true;

            Debug.Log("Boss Rotation Enabled");
        }

        return;
    }

    // Normal random rotation
    if (currentChallenge < rotationOrder.Length)
    {
        int randomIndex = rotationOrder[currentChallenge];

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

void SaveLevel()
{
    PlayerPrefs.SetInt(LevelKey, winChallengeNumber);
    PlayerPrefs.Save();
}

string LevelKey
{
    get { return saveID + "_Level"; }
}

string ChallengeKey
{
    get { return saveID + "_Challenge"; }
}

string SliderKey
{
    get { return saveID + "_SliderFill"; }
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

    if (winPanel != null)
        winPanel.SetActive(false);

    LoadChallenge();
}

public void RestartLevel()
{
    sessionScore = 0;
    sessionStage = 1;

    currentChallenge = 0;

    if (winPanel != null)
        winPanel.SetActive(false);

    LoadChallenge();
}

public int GetCurrentLevel()
{
    return winChallengeNumber;
   
}


}