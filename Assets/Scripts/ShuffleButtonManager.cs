// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using TMPro;
// using System.Collections;
// using System.Collections.Generic;

// public class ShuffleButtonManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class ShuffleButtonEntry
//     {
//         [Header("── Button ──────────────────────────")]
//         public Button button;

//         [Header("── Type ───────────────────────────")]
//         public ButtonType buttonType = ButtonType.DirectAction;

//         [Header("(Direct Action) URL  [ instagram://  mailto:  market://  https://... ]")]
//         public string url = "";

//         [Header("(Panel) Panel RectTransform")]
//         public RectTransform panel;

//         [Header("(Panel) Close Button  (optional)")]
//         public Button closeButton;
//     }

//     public enum ButtonType
//     {
//         DirectAction,
//         OpenPanel
//     }

//     [Header("══ BUTTON ENTRIES (saare buttons daalo) ══")]
//     public List<ShuffleButtonEntry> entries = new List<ShuffleButtonEntry>();

//     [Header("══ KITNE BUTTONS DIKHANE HAIN ══")]
//     public int visibleButtonCount = 4;

//     // =====================================================
//     //  GIFT BUTTON SETTINGS
//     // =====================================================

//     [Header("══ GIFT BUTTON ══")]
//     [Tooltip("entries list me gift button ka index (0 se start)")]
//     public int giftButtonIndex = 0;

//     [Header("Timer Duration (change karo to TURANT naya timer lagega)")]
//     public int timerHours   = 0;
//     public int timerMinutes = 0;
//     public int timerSeconds = 30;

//     [Header("Timer + Ready — EK HI TMP Text")]
//     public TextMeshProUGUI giftTimerReadyText;

//     [Header("Gift Panel")]
//     public RectTransform giftPanel;
//     public Button        giftCloseButton;

//     [Header("Gift Panel Animation")]
//     public float          giftAnimDuration = 0.35f;
//     public AnimationCurve giftAnimCurve    = AnimationCurve.EaseInOut(0, 0, 1, 1);

//     // PlayerPrefs keys
//     private const string GIFT_TIMER_END_KEY      = "GIFT_TIMER_END";
//     private const string GIFT_TIMER_READY_KEY    = "GIFT_TIMER_READY";
//     private const string GIFT_TIMER_SAVED_H_KEY  = "GIFT_TIMER_SAVED_H";
//     private const string GIFT_TIMER_SAVED_M_KEY  = "GIFT_TIMER_SAVED_M";
//     private const string GIFT_TIMER_SAVED_S_KEY  = "GIFT_TIMER_SAVED_S";

//     private bool    _timerReady    = false;
//     private bool    _giftPanelOpen = false;
//     private Vector3 _giftPanelCenter;
//     private Vector3 _giftPanelScale;

//     // =====================================================
//     //  NO INTERNET TEXT
//     // =====================================================

//     [Header("══ NO INTERNET TEXT (TMP) ══")]
//     public TextMeshProUGUI noInternetText;
//     public float messageDuration = 2f;
//     public float fadeSpeed       = 0.3f;
//     public float popScale        = 1.25f;

//     [Header("══ OBJECTS TO DISABLE WHEN PANEL IS OPEN ══")]
//     public GameObject[] objectsToDisableOnOpen;

//     [Header("══ PANEL ANIMATION ══")]
//     public float          panelAnimDuration = 0.35f;
//     public AnimationCurve panelAnimCurve    = AnimationCurve.EaseInOut(0, 0, 1, 1);

//     private Vector3[] _panelWorldCenter;
//     private Vector3[] _panelOriginalScale;

//     private List<int> _activeIndices = new List<int>();

//     private bool _panelOpen         = false;
//     private int  _currentPanelIndex = -1;

//     private Coroutine _msgCoroutine;
//     private Coroutine _timerCoroutine;

//     [Header("Gift Ready Blink")]
// public Color blinkColor = Color.white;
// public float blinkDuration = 0.3f;
// public float blinkInterval = 5f;

// private Coroutine _giftBlinkCoroutine;
// private Color _giftOriginalColor;

//     // =====================================================
//     //  START
//     // =====================================================

//     void Start()
//     {
//         int total = entries.Count;

//         _panelWorldCenter   = new Vector3[total];
//         _panelOriginalScale = new Vector3[total];

//         if (noInternetText != null)
//         {
//             noInternetText.text  = "Please check your internet connection";
//             noInternetText.alpha = 0f;
//             noInternetText.transform.localScale = Vector3.one;
//         }

//         if (giftPanel != null)
//         {
//             _giftPanelCenter = giftPanel.position;
//             _giftPanelScale  = giftPanel.localScale;
//             giftPanel.gameObject.SetActive(false);
//         }

//         if (giftCloseButton != null)
//             giftCloseButton.onClick.AddListener(CloseGiftPanel);

//         if (giftTimerReadyText != null)
//             giftTimerReadyText.text = "00:00:00";

//         for (int i = 0; i < total; i++)
//         {
//             ShuffleButtonEntry e = entries[i];

//             if (e.button != null)
//                 e.button.gameObject.SetActive(false);

//             if (e.buttonType == ButtonType.OpenPanel && e.panel != null)
//             {
//                 _panelWorldCenter[i]   = e.panel.position;
//                 _panelOriginalScale[i] = e.panel.localScale;
//                 e.panel.gameObject.SetActive(false);
//             }
//         }

// if (giftButtonIndex < entries.Count &&
//     entries[giftButtonIndex].button != null)
// {
//     Image img = entries[giftButtonIndex].button.GetComponent<Image>();

//     if (img != null)
//         _giftOriginalColor = img.color;
// }

//         PickActiveButtons();
//         StartGiftTimer();
//     }

//     // =====================================================
//     //  RANDOM PICK
//     // =====================================================

//     void PickActiveButtons()
//     {
//         int total     = entries.Count;
//         int pickCount = Mathf.Min(visibleButtonCount, total);

//         List<int> allIndices = new List<int>();
//         for (int i = 0; i < total; i++) allIndices.Add(i);

//         for (int i = allIndices.Count - 1; i > 0; i--)
//         {
//             int j    = UnityEngine.Random.Range(0, i + 1);
//             int temp = allIndices[i];
//             allIndices[i] = allIndices[j];
//             allIndices[j] = temp;
//         }

//         _activeIndices.Clear();
//         for (int i = 0; i < pickCount; i++)
//             _activeIndices.Add(allIndices[i]);

//         for (int i = 0; i < total; i++)
//             if (entries[i].button != null)
//                 entries[i].button.transform.SetAsLastSibling();

//         for (int i = 0; i < pickCount; i++)
//         {
//             int entryIdx = _activeIndices[i];
//             ShuffleButtonEntry e = entries[entryIdx];

//             if (e.button == null) continue;

//             e.button.gameObject.SetActive(true);
//             e.button.onClick.RemoveAllListeners();

//             int capturedIdx = entryIdx;

//             if (entryIdx == giftButtonIndex)
//             {
//                 e.button.onClick.AddListener(OnGiftButtonPressed);
//             }
//             else if (e.buttonType == ButtonType.DirectAction)
//             {
//                 e.button.onClick.AddListener(() => OnDirectAction(capturedIdx));
//             }
//             else
//             {
//                 e.button.onClick.AddListener(() => OpenPanel(capturedIdx));

//                 if (e.closeButton != null)
//                 {
//                     e.closeButton.onClick.RemoveAllListeners();
//                     e.closeButton.onClick.AddListener(CloseCurrentPanel);
//                 }
//             }
//         }

//         LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
//     }

//     // =====================================================
//     //  GIFT TIMER — START LOGIC
//     // =====================================================

//     void StartGiftTimer()
//     {
//         int inspectorDuration = timerHours * 3600 + timerMinutes * 60 + timerSeconds;

//         // Pehle check karo kya Ready tha
//         bool wasReady = PlayerPrefs.GetInt(GIFT_TIMER_READY_KEY, 0) == 1;

//         // KEY FIX: Inspector me nayi value aayi hai ya nahi check karo
//         // Pichli baar jo H/M/S save kiya tha usse compare karo
//         int savedH = PlayerPrefs.GetInt(GIFT_TIMER_SAVED_H_KEY, -1);
//         int savedM = PlayerPrefs.GetInt(GIFT_TIMER_SAVED_M_KEY, -1);
//         int savedS = PlayerPrefs.GetInt(GIFT_TIMER_SAVED_S_KEY, -1);

//         bool durationChanged = (savedH != timerHours ||
//                                 savedM != timerMinutes ||
//                                 savedS != timerSeconds);

//         // Duration badli hai to purana sab clear karo aur naya timer lagao
//         if (durationChanged)
//         {
//             // Nayi duration save karo
//             PlayerPrefs.SetInt(GIFT_TIMER_SAVED_H_KEY, timerHours);
//             PlayerPrefs.SetInt(GIFT_TIMER_SAVED_M_KEY, timerMinutes);
//             PlayerPrefs.SetInt(GIFT_TIMER_SAVED_S_KEY, timerSeconds);
//             PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 0);
//             PlayerPrefs.DeleteKey(GIFT_TIMER_END_KEY);
//             PlayerPrefs.Save();

//             SetNewTimer(inspectorDuration);
//             return;
//         }

//         // Duration same hai — normal flow
//         if (wasReady)
// {
//     _timerReady = true;

//     if (giftTimerReadyText != null)
//         giftTimerReadyText.text = "Ready!";

//     StartGiftBlink();
//     return;
// }

//         string savedEnd = PlayerPrefs.GetString(GIFT_TIMER_END_KEY, "");

//         if (string.IsNullOrEmpty(savedEnd))
//         {
//             // Pehli baar
//             SetNewTimer(inspectorDuration);
//         }
//         else
//         {
//             // Resume karo
//             System.DateTime endTime = System.DateTime.Parse(
//                 savedEnd, null,
//                 System.Globalization.DateTimeStyles.RoundtripKind
//             );

//             System.TimeSpan remaining = endTime - System.DateTime.UtcNow;

//             if (remaining.TotalSeconds <= 0)
//             {
//                 // Game band tha aur timer khatam ho gaya
//                 _timerReady = true;
//                 PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 1);
//                 PlayerPrefs.DeleteKey(GIFT_TIMER_END_KEY);
//                 PlayerPrefs.Save();

//                 if (giftTimerReadyText != null)
//                     giftTimerReadyText.text = "Ready!";
//                 return;
//             }

//             // Timer baaki hai — resume
//             if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
//             _timerCoroutine = StartCoroutine(RunGiftTimer(endTime));
//         }
//     }

//     void SetNewTimer(int durationSeconds)
//     {
//         System.DateTime endTime = System.DateTime.UtcNow.AddSeconds(durationSeconds);

//         PlayerPrefs.SetString(GIFT_TIMER_END_KEY, endTime.ToString("o"));
//         PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 0);
//         PlayerPrefs.Save();

//         if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
//         _timerCoroutine = StartCoroutine(RunGiftTimer(endTime));
//     }

//     // =====================================================
//     //  GIFT TIMER — COROUTINE
//     // =====================================================

//     IEnumerator RunGiftTimer(System.DateTime endTime)
//     {
//         _timerReady = false;

//         while (true)
//         {
//             System.TimeSpan remaining = endTime - System.DateTime.UtcNow;

//             if (remaining.TotalSeconds <= 0)
//             {
//                 _timerReady = true;

//                 PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 1);
//                 PlayerPrefs.DeleteKey(GIFT_TIMER_END_KEY);
//                 PlayerPrefs.Save();

//                 if (giftTimerReadyText != null)
//                     giftTimerReadyText.text = "Ready!";

//                     StartGiftBlink();

//                 yield break;
//             }

//             if (giftTimerReadyText != null)
//             {
//                 if (remaining.TotalHours >= 24)
//                 {
//                     giftTimerReadyText.text = string.Format(
//                         "{0}d {1:D2}:{2:D2}:{3:D2}",
//                         remaining.Days,
//                         remaining.Hours,
//                         remaining.Minutes,
//                         remaining.Seconds
//                     );
//                 }
//                 else
//                 {
//                     giftTimerReadyText.text = string.Format(
//                         "{0:D2}:{1:D2}:{2:D2}",
//                         (int)remaining.TotalHours,
//                         remaining.Minutes,
//                         remaining.Seconds
//                     );
//                 }
//             }

//             yield return new WaitForSecondsRealtime(1f);
//         }
//     }

//     // =====================================================
//     //  GIFT BUTTON PRESSED
//     // =====================================================

//     void OnGiftButtonPressed()
//     {
//         if (!_timerReady) return;
//         OpenGiftPanel();
//     }

//     // =====================================================
//     //  GIFT PANEL OPEN
//     // =====================================================

//     void OpenGiftPanel()
//     {
//         if (_giftPanelOpen || _panelOpen) return;
//         if (giftPanel == null) return;

//         _giftPanelOpen = true;

//         SetActiveButtonsVisible(false);
//         SetExtraObjects(false);

//         Vector3 startFrom = _giftPanelCenter;
//         if (giftButtonIndex < entries.Count && entries[giftButtonIndex].button != null)
//             startFrom = entries[giftButtonIndex].button
//                             .GetComponent<RectTransform>().position;

//         giftPanel.gameObject.SetActive(true);
//         giftPanel.position   = startFrom;
//         giftPanel.localScale = Vector3.zero;

//         StartCoroutine(AnimateGiftPanel(_giftPanelCenter, _giftPanelScale, opening: true));
//     }

//     // =====================================================
//     //  GIFT PANEL CLOSE
//     // =====================================================

//     public void CloseGiftPanel()
//     {
//         if (!_giftPanelOpen) return;
        

//         StopGiftBlink();
//         Vector3 closeTo = _giftPanelCenter;
//         if (giftButtonIndex < entries.Count && entries[giftButtonIndex].button != null)
//             closeTo = entries[giftButtonIndex].button
//                           .GetComponent<RectTransform>().position;

//         StartCoroutine(AnimateGiftPanel(closeTo, Vector3.zero, opening: false));

//         // Ready clear karo aur naya timer shuru karo
//         _timerReady = false;

//         if (giftTimerReadyText != null)
//             giftTimerReadyText.text = "00:00:00";

//         PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 0);
//         PlayerPrefs.Save();

//         int newDuration = timerHours * 3600 + timerMinutes * 60 + timerSeconds;
//         SetNewTimer(newDuration);

        
//     }

//     // =====================================================
//     //  GIFT PANEL ANIMATION
//     // =====================================================

//     IEnumerator AnimateGiftPanel(Vector3 targetPos, Vector3 targetScale, bool opening)
//     {
//         Vector3 startPos   = giftPanel.position;
//         Vector3 startScale = giftPanel.localScale;

//         float t = 0f;
//         while (t < 1f)
//         {
//             t += Time.unscaledDeltaTime / giftAnimDuration;
//             float eval = giftAnimCurve.Evaluate(Mathf.Clamp01(t));

//             giftPanel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
//             giftPanel.localScale = Vector3.Lerp(startScale, targetScale, eval);

//             yield return null;
//         }

//         giftPanel.position   = targetPos;
//         giftPanel.localScale = targetScale;

//         if (!opening)
//         {
//             giftPanel.gameObject.SetActive(false);
//             SetActiveButtonsVisible(true);
//             SetExtraObjects(true);
//             _giftPanelOpen = false;
//         }
//     }

//     // =====================================================
//     //  DIRECT ACTION
//     // =====================================================

//     void OnDirectAction(int entryIndex)
//     {
//         if (Application.internetReachability == NetworkReachability.NotReachable)
//         {
//             ShowNoInternetText();
//             return;
//         }

//         string url = entries[entryIndex].url;
//         if (!string.IsNullOrEmpty(url))
//             Application.OpenURL(url);
//     }

//     // =====================================================
//     //  NO INTERNET TEXT ANIMATION
//     // =====================================================

//     void ShowNoInternetText()
//     {
//         if (noInternetText == null) return;

//         if (_msgCoroutine != null)
//             StopCoroutine(_msgCoroutine);

//         _msgCoroutine = StartCoroutine(NoInternetAnim());
//     }

//     IEnumerator NoInternetAnim()
//     {
//         Transform tf        = noInternetText.transform;
//         Vector3   origScale = Vector3.one;

//         noInternetText.alpha = 0f;
//         tf.localScale        = origScale * 0.6f;

//         float t = 0f;
//         while (t < 1f)
//         {
//             t += Time.unscaledDeltaTime / fadeSpeed;
//             float ev = Mathf.Clamp01(t);

//             noInternetText.alpha = Mathf.Lerp(0f, 1f, ev);

//             float scaleVal = ev < 0.5f
//                 ? Mathf.Lerp(0.6f, popScale, ev * 2f)
//                 : Mathf.Lerp(popScale, 1f, (ev - 0.5f) * 2f);

//             tf.localScale = origScale * scaleVal;
//             yield return null;
//         }

//         noInternetText.alpha = 1f;
//         tf.localScale        = origScale;

//         yield return new WaitForSecondsRealtime(messageDuration);

//         t = 0f;
//         while (t < 1f)
//         {
//             t += Time.unscaledDeltaTime / fadeSpeed;
//             noInternetText.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01(t));
//             yield return null;
//         }

//         noInternetText.alpha = 0f;
//         tf.localScale        = origScale;
//         _msgCoroutine        = null;
//     }

//     // =====================================================
//     //  NORMAL PANEL OPEN / CLOSE
//     // =====================================================

//     void OpenPanel(int entryIndex)
//     {
//         if (_panelOpen || _giftPanelOpen) return;

//         ShuffleButtonEntry e = entries[entryIndex];
//         if (e.panel == null) return;

//         _panelOpen         = true;
//         _currentPanelIndex = entryIndex;

//         SetActiveButtonsVisible(false);
//         SetExtraObjects(false);

//         RectTransform panel   = e.panel;
//         RectTransform btnRect = e.button.GetComponent<RectTransform>();

//         panel.gameObject.SetActive(true);
//         panel.position   = btnRect.position;
//         panel.localScale = Vector3.zero;

//         StartCoroutine(AnimatePanel(
//             panel,
//             _panelWorldCenter[entryIndex],
//             _panelOriginalScale[entryIndex],
//             opening: true
//         ));
//     }

//     public void CloseCurrentPanel()
//     {
//         if (_currentPanelIndex == -1) return;

//         ShuffleButtonEntry e       = entries[_currentPanelIndex];
//         RectTransform      panel   = e.panel;
//         RectTransform      btnRect = e.button.GetComponent<RectTransform>();

//         StartCoroutine(AnimatePanel(panel, btnRect.position, Vector3.zero, opening: false));
//     }

//     IEnumerator AnimatePanel(
//         RectTransform panel,
//         Vector3       targetPos,
//         Vector3       targetScale,
//         bool          opening)
//     {
//         Vector3 startPos   = panel.position;
//         Vector3 startScale = panel.localScale;

//         float t = 0f;
//         while (t < 1f)
//         {
//             t += Time.unscaledDeltaTime / panelAnimDuration;
//             float eval = panelAnimCurve.Evaluate(Mathf.Clamp01(t));

//             panel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
//             panel.localScale = Vector3.Lerp(startScale, targetScale, eval);

//             yield return null;
//         }

//         panel.position   = targetPos;
//         panel.localScale = targetScale;

//         if (!opening)
//         {
//             panel.gameObject.SetActive(false);
//             SetActiveButtonsVisible(true);
//             SetExtraObjects(true);
//             _panelOpen         = false;
//             _currentPanelIndex = -1;
//         }
//     }

//     // =====================================================
//     //  UPDATE
//     // =====================================================

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (_panelOpen && !IsPointerOverPanel(entries[_currentPanelIndex].panel))
//                 CloseCurrentPanel();

//             if (_giftPanelOpen && !IsPointerOverPanel(giftPanel))
//                 CloseGiftPanel();
//         }
//     }

//     // =====================================================
//     //  HELPERS
//     // =====================================================

//     bool IsPointerOverPanel(RectTransform panelRect)
//     {
//         if (panelRect == null) return false;

//         PointerEventData ed = new PointerEventData(EventSystem.current)
//         {
//             position = Input.mousePosition
//         };

//         var results = new List<RaycastResult>();
//         EventSystem.current.RaycastAll(ed, results);

//         foreach (var r in results)
//             if (r.gameObject.transform.IsChildOf(panelRect))
//                 return true;

//         return false;
//     }

//     void SetActiveButtonsVisible(bool visible)
//     {
//         foreach (int idx in _activeIndices)
//             if (entries[idx].button != null)
//                 entries[idx].button.gameObject.SetActive(visible);
//     }

//     void SetExtraObjects(bool enable)
//     {
//         if (objectsToDisableOnOpen == null) return;
//         foreach (GameObject obj in objectsToDisableOnOpen)
//             if (obj != null)
//                 obj.SetActive(enable);
//     }


//     void StartGiftBlink()
// {
//     StopGiftBlink();
//     _giftBlinkCoroutine = StartCoroutine(GiftBlinkRoutine());
// }

// void StopGiftBlink()
// {
//     if (_giftBlinkCoroutine != null)
//     {
//         StopCoroutine(_giftBlinkCoroutine);
//         _giftBlinkCoroutine = null;
//     }

//     if (giftButtonIndex < entries.Count &&
//         entries[giftButtonIndex].button != null)
//     {
//         Image img = entries[giftButtonIndex].button.GetComponent<Image>();

//         if (img != null)
//             img.color = _giftOriginalColor;
//     }
// }

// IEnumerator GiftBlinkRoutine()
// {
//     Image img = entries[giftButtonIndex].button.GetComponent<Image>();

//     if (img == null)
//         yield break;

//     while (true)
//     {
//         // 5 sec wait
//         yield return new WaitForSecondsRealtime(blinkInterval);

//         // Blink ON
//         img.color = blinkColor;

//         yield return new WaitForSecondsRealtime(blinkDuration);

//         // Blink OFF
//         img.color = _giftOriginalColor;
//     }
// }



// }



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ShuffleButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ShuffleButtonEntry
    {
        [Header("── Button ──────────────────────────")]
        public Button button;

        [Header("── Type ───────────────────────────")]
        public ButtonType buttonType = ButtonType.DirectAction;

        [Header("(Direct Action) URL  [ instagram://  mailto:  market://  https://... ]")]
        public string url = "";

        [Header("(Panel) Panel RectTransform")]
        public RectTransform panel;

        [Header("(Panel) Close Button  (optional)")]
        public Button closeButton;
    }

    public enum ButtonType
    {
        DirectAction,
        OpenPanel
    }

    [Header("══ BUTTON ENTRIES (saare buttons daalo) ══")]
    public List<ShuffleButtonEntry> entries = new List<ShuffleButtonEntry>();

    [Header("══ KITNE BUTTONS DIKHANE HAIN ══")]
    public int visibleButtonCount = 4;

    // =====================================================
    //  GIFT BUTTON SETTINGS
    // =====================================================

    [Header("══ GIFT BUTTON ══")]
    [Tooltip("entries list me gift button ka index (0 se start)")]
    public int giftButtonIndex = 0;

    [Header("Timer Duration (change karo to TURANT naya timer lagega)")]
    public int timerHours   = 0;
    public int timerMinutes = 0;
    public int timerSeconds = 30;

    [Header("Timer + Ready — EK HI TMP Text")]
    public TextMeshProUGUI giftTimerReadyText;

    [Header("Gift Panel")]
    public RectTransform giftPanel;
    public Button        giftCloseButton;

    [Header("Gift Panel Animation")]
    public float          giftAnimDuration = 0.35f;
    public AnimationCurve giftAnimCurve    = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // PlayerPrefs keys
    private const string GIFT_TIMER_END_KEY      = "GIFT_TIMER_END";
    private const string GIFT_TIMER_READY_KEY    = "GIFT_TIMER_READY";
    private const string GIFT_TIMER_SAVED_H_KEY  = "GIFT_TIMER_SAVED_H";
    private const string GIFT_TIMER_SAVED_M_KEY  = "GIFT_TIMER_SAVED_M";
    private const string GIFT_TIMER_SAVED_S_KEY  = "GIFT_TIMER_SAVED_S";

    private bool    _timerReady    = false;
    private bool    _giftPanelOpen = false;
    private Vector3 _giftPanelCenter;
    private Vector3 _giftPanelScale;

    // =====================================================
    //  NO INTERNET TEXT
    // =====================================================

    [Header("══ NO INTERNET TEXT (TMP) ══")]
    public TextMeshProUGUI noInternetText;
    public float messageDuration = 2f;
    public float fadeSpeed       = 0.3f;
    public float popScale        = 1.25f;

    [Header("══ OBJECTS TO DISABLE WHEN PANEL IS OPEN ══")]
    public GameObject[] objectsToDisableOnOpen;

    [Header("══ PANEL ANIMATION ══")]
    public float          panelAnimDuration = 0.35f;
    public AnimationCurve panelAnimCurve    = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3[] _panelWorldCenter;
    private Vector3[] _panelOriginalScale;

    private List<int> _activeIndices = new List<int>();

    private bool _panelOpen         = false;
    private int  _currentPanelIndex = -1;

    private Coroutine _msgCoroutine;
    private Coroutine _timerCoroutine;

    [Header("Gift Ready Blink")]
public Color blinkColor = Color.white;
public float blinkDuration = 0.3f;
public float blinkInterval = 5f;

private Coroutine _giftBlinkCoroutine;
private Color _giftOriginalColor;

[Header("══ EXIT BUTTON INDEX ══")]
public int exitButtonIndex = 6;

    // =====================================================
    //  START
    // =====================================================

    void Start()
    {
        int total = entries.Count;

        _panelWorldCenter   = new Vector3[total];
        _panelOriginalScale = new Vector3[total];

        if (noInternetText != null)
        {
            noInternetText.text  = "Please check your internet connection";
            noInternetText.alpha = 0f;
            noInternetText.transform.localScale = Vector3.one;
        }

        if (giftPanel != null)
        {
            _giftPanelCenter = giftPanel.position;
            _giftPanelScale  = giftPanel.localScale;
            giftPanel.gameObject.SetActive(false);
        }

        if (giftCloseButton != null)
            giftCloseButton.onClick.AddListener(CloseGiftPanel);

        if (giftTimerReadyText != null)
            giftTimerReadyText.text = "00:00:00";

        for (int i = 0; i < total; i++)
        {
            ShuffleButtonEntry e = entries[i];

            if (e.button != null)
                e.button.gameObject.SetActive(false);

            if (e.buttonType == ButtonType.OpenPanel && e.panel != null)
            {
                _panelWorldCenter[i]   = e.panel.position;
                _panelOriginalScale[i] = e.panel.localScale;
                e.panel.gameObject.SetActive(false);
            }
        }

if (giftButtonIndex < entries.Count &&
    entries[giftButtonIndex].button != null)
{
    Image img = entries[giftButtonIndex].button.GetComponent<Image>();

    if (img != null)
        _giftOriginalColor = img.color;
}

        PickActiveButtons();
        StartGiftTimer();
    }

    // =====================================================
    //  RANDOM PICK
    // =====================================================

    void PickActiveButtons()
    {
        int total     = entries.Count;
        int pickCount = Mathf.Min(visibleButtonCount, total);

        List<int> allIndices = new List<int>();
        for (int i = 0; i < total; i++) allIndices.Add(i);

        for (int i = allIndices.Count - 1; i > 0; i--)
        {
            int j    = UnityEngine.Random.Range(0, i + 1);
            int temp = allIndices[i];
            allIndices[i] = allIndices[j];
            allIndices[j] = temp;
        }

        _activeIndices.Clear();
        for (int i = 0; i < pickCount; i++)
            _activeIndices.Add(allIndices[i]);

        for (int i = 0; i < total; i++)
            if (entries[i].button != null)
                entries[i].button.transform.SetAsLastSibling();

        for (int i = 0; i < pickCount; i++)
        {
            int entryIdx = _activeIndices[i];
            ShuffleButtonEntry e = entries[entryIdx];

            if (e.button == null) continue;

            e.button.gameObject.SetActive(true);
            e.button.onClick.RemoveAllListeners();

            int capturedIdx = entryIdx;

            if (entryIdx == giftButtonIndex)
{
    e.button.onClick.AddListener(OnGiftButtonPressed);
}
else if (entryIdx == exitButtonIndex)
{
    e.button.onClick.AddListener(ExitGame);
}
else if (e.buttonType == ButtonType.DirectAction)
{
    e.button.onClick.AddListener(() => OnDirectAction(capturedIdx));
}
else
{
                e.button.onClick.AddListener(() => OpenPanel(capturedIdx));

                if (e.closeButton != null)
                {
                    e.closeButton.onClick.RemoveAllListeners();
                    e.closeButton.onClick.AddListener(CloseCurrentPanel);
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    // =====================================================
    //  GIFT TIMER — START LOGIC
    // =====================================================

    void StartGiftTimer()
    {
        int inspectorDuration = timerHours * 3600 + timerMinutes * 60 + timerSeconds;

        // Pehle check karo kya Ready tha
        bool wasReady = PlayerPrefs.GetInt(GIFT_TIMER_READY_KEY, 0) == 1;

        // KEY FIX: Inspector me nayi value aayi hai ya nahi check karo
        // Pichli baar jo H/M/S save kiya tha usse compare karo
        int savedH = PlayerPrefs.GetInt(GIFT_TIMER_SAVED_H_KEY, -1);
        int savedM = PlayerPrefs.GetInt(GIFT_TIMER_SAVED_M_KEY, -1);
        int savedS = PlayerPrefs.GetInt(GIFT_TIMER_SAVED_S_KEY, -1);

        bool durationChanged = (savedH != timerHours ||
                                savedM != timerMinutes ||
                                savedS != timerSeconds);

        // Duration badli hai to purana sab clear karo aur naya timer lagao
        if (durationChanged)
        {
            // Nayi duration save karo
            PlayerPrefs.SetInt(GIFT_TIMER_SAVED_H_KEY, timerHours);
            PlayerPrefs.SetInt(GIFT_TIMER_SAVED_M_KEY, timerMinutes);
            PlayerPrefs.SetInt(GIFT_TIMER_SAVED_S_KEY, timerSeconds);
            PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 0);
            PlayerPrefs.DeleteKey(GIFT_TIMER_END_KEY);
            PlayerPrefs.Save();

            SetNewTimer(inspectorDuration);
            return;
        }

        // Duration same hai — normal flow
        if (wasReady)
{
    _timerReady = true;

    if (giftTimerReadyText != null)
        giftTimerReadyText.text = "Ready!";

    StartGiftBlink();
    return;
}

        string savedEnd = PlayerPrefs.GetString(GIFT_TIMER_END_KEY, "");

        if (string.IsNullOrEmpty(savedEnd))
        {
            // Pehli baar
            SetNewTimer(inspectorDuration);
        }
        else
        {
            // Resume karo
            System.DateTime endTime = System.DateTime.Parse(
                savedEnd, null,
                System.Globalization.DateTimeStyles.RoundtripKind
            );

            System.TimeSpan remaining = endTime - System.DateTime.UtcNow;

            if (remaining.TotalSeconds <= 0)
            {
                // Game band tha aur timer khatam ho gaya
                _timerReady = true;
                PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 1);
                PlayerPrefs.DeleteKey(GIFT_TIMER_END_KEY);
                PlayerPrefs.Save();

                if (giftTimerReadyText != null)
                    giftTimerReadyText.text = "Ready!";
                return;
            }

            // Timer baaki hai — resume
            if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
            _timerCoroutine = StartCoroutine(RunGiftTimer(endTime));
        }
    }

    void SetNewTimer(int durationSeconds)
    {
        System.DateTime endTime = System.DateTime.UtcNow.AddSeconds(durationSeconds);

        PlayerPrefs.SetString(GIFT_TIMER_END_KEY, endTime.ToString("o"));
        PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 0);
        PlayerPrefs.Save();

        if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
        _timerCoroutine = StartCoroutine(RunGiftTimer(endTime));
    }

    // =====================================================
    //  GIFT TIMER — COROUTINE
    // =====================================================

    IEnumerator RunGiftTimer(System.DateTime endTime)
    {
        _timerReady = false;

        while (true)
        {
            System.TimeSpan remaining = endTime - System.DateTime.UtcNow;

            if (remaining.TotalSeconds <= 0)
            {
                _timerReady = true;

                PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 1);
                PlayerPrefs.DeleteKey(GIFT_TIMER_END_KEY);
                PlayerPrefs.Save();

                if (giftTimerReadyText != null)
                    giftTimerReadyText.text = "Ready!";

                    StartGiftBlink();

                yield break;
            }

            if (giftTimerReadyText != null)
            {
                if (remaining.TotalHours >= 24)
                {
                    giftTimerReadyText.text = string.Format(
                        "{0}d {1:D2}:{2:D2}:{3:D2}",
                        remaining.Days,
                        remaining.Hours,
                        remaining.Minutes,
                        remaining.Seconds
                    );
                }
                else
                {
                    giftTimerReadyText.text = string.Format(
                        "{0:D2}:{1:D2}:{2:D2}",
                        (int)remaining.TotalHours,
                        remaining.Minutes,
                        remaining.Seconds
                    );
                }
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    // =====================================================
    //  GIFT BUTTON PRESSED
    // =====================================================

    void OnGiftButtonPressed()
    {
        if (!_timerReady) return;
        OpenGiftPanel();
    }

    // =====================================================
    //  GIFT PANEL OPEN
    // =====================================================

    void OpenGiftPanel()
    {
        if (_giftPanelOpen || _panelOpen) return;
        if (giftPanel == null) return;

        _giftPanelOpen = true;

        SetActiveButtonsVisible(false);
        SetExtraObjects(false);

        Vector3 startFrom = _giftPanelCenter;
        if (giftButtonIndex < entries.Count && entries[giftButtonIndex].button != null)
            startFrom = entries[giftButtonIndex].button
                            .GetComponent<RectTransform>().position;

        giftPanel.gameObject.SetActive(true);
        giftPanel.position   = startFrom;
        giftPanel.localScale = Vector3.zero;

        StartCoroutine(AnimateGiftPanel(_giftPanelCenter, _giftPanelScale, opening: true));
    }

    // =====================================================
    //  GIFT PANEL CLOSE
    // =====================================================

    public void CloseGiftPanel()
    {
        if (!_giftPanelOpen) return;
        

        StopGiftBlink();
        Vector3 closeTo = _giftPanelCenter;
        if (giftButtonIndex < entries.Count && entries[giftButtonIndex].button != null)
            closeTo = entries[giftButtonIndex].button
                          .GetComponent<RectTransform>().position;

        StartCoroutine(AnimateGiftPanel(closeTo, Vector3.zero, opening: false));

        // Ready clear karo aur naya timer shuru karo
        _timerReady = false;

        if (giftTimerReadyText != null)
            giftTimerReadyText.text = "00:00:00";

        PlayerPrefs.SetInt(GIFT_TIMER_READY_KEY, 0);
        PlayerPrefs.Save();

        int newDuration = timerHours * 3600 + timerMinutes * 60 + timerSeconds;
        SetNewTimer(newDuration);

        
    }

    // =====================================================
    //  GIFT PANEL ANIMATION
    // =====================================================

    IEnumerator AnimateGiftPanel(Vector3 targetPos, Vector3 targetScale, bool opening)
    {
        Vector3 startPos   = giftPanel.position;
        Vector3 startScale = giftPanel.localScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / giftAnimDuration;
            float eval = giftAnimCurve.Evaluate(Mathf.Clamp01(t));

            giftPanel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
            giftPanel.localScale = Vector3.Lerp(startScale, targetScale, eval);

            yield return null;
        }

        giftPanel.position   = targetPos;
        giftPanel.localScale = targetScale;

        if (!opening)
        {
            giftPanel.gameObject.SetActive(false);
            SetActiveButtonsVisible(true);
            SetExtraObjects(true);
            _giftPanelOpen = false;
        }
    }

    // =====================================================
    //  DIRECT ACTION
    // =====================================================

    void OnDirectAction(int entryIndex)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShowNoInternetText();
            return;
        }

        string url = entries[entryIndex].url;
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }

    // =====================================================
    //  NO INTERNET TEXT ANIMATION
    // =====================================================

    void ShowNoInternetText()
    {
        if (noInternetText == null) return;

        if (_msgCoroutine != null)
            StopCoroutine(_msgCoroutine);

        _msgCoroutine = StartCoroutine(NoInternetAnim());
    }

    IEnumerator NoInternetAnim()
    {
        Transform tf        = noInternetText.transform;
        Vector3   origScale = Vector3.one;

        noInternetText.alpha = 0f;
        tf.localScale        = origScale * 0.6f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeSpeed;
            float ev = Mathf.Clamp01(t);

            noInternetText.alpha = Mathf.Lerp(0f, 1f, ev);

            float scaleVal = ev < 0.5f
                ? Mathf.Lerp(0.6f, popScale, ev * 2f)
                : Mathf.Lerp(popScale, 1f, (ev - 0.5f) * 2f);

            tf.localScale = origScale * scaleVal;
            yield return null;
        }

        noInternetText.alpha = 1f;
        tf.localScale        = origScale;

        yield return new WaitForSecondsRealtime(messageDuration);

        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeSpeed;
            noInternetText.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01(t));
            yield return null;
        }

        noInternetText.alpha = 0f;
        tf.localScale        = origScale;
        _msgCoroutine        = null;
    }

    // =====================================================
    //  NORMAL PANEL OPEN / CLOSE
    // =====================================================

    void OpenPanel(int entryIndex)
    {
        if (_panelOpen || _giftPanelOpen) return;

        ShuffleButtonEntry e = entries[entryIndex];
        if (e.panel == null) return;

        _panelOpen         = true;
        _currentPanelIndex = entryIndex;

        SetActiveButtonsVisible(false);
        SetExtraObjects(false);

        RectTransform panel   = e.panel;
        RectTransform btnRect = e.button.GetComponent<RectTransform>();

        panel.gameObject.SetActive(true);
        panel.position   = btnRect.position;
        panel.localScale = Vector3.zero;

        StartCoroutine(AnimatePanel(
            panel,
            _panelWorldCenter[entryIndex],
            _panelOriginalScale[entryIndex],
            opening: true
        ));
    }

    public void CloseCurrentPanel()
    {
        if (_currentPanelIndex == -1) return;

        ShuffleButtonEntry e       = entries[_currentPanelIndex];
        RectTransform      panel   = e.panel;
        RectTransform      btnRect = e.button.GetComponent<RectTransform>();

        StartCoroutine(AnimatePanel(panel, btnRect.position, Vector3.zero, opening: false));
    }

    IEnumerator AnimatePanel(
        RectTransform panel,
        Vector3       targetPos,
        Vector3       targetScale,
        bool          opening)
    {
        Vector3 startPos   = panel.position;
        Vector3 startScale = panel.localScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / panelAnimDuration;
            float eval = panelAnimCurve.Evaluate(Mathf.Clamp01(t));

            panel.position   = Vector3.Lerp(startPos,   targetPos,   eval);
            panel.localScale = Vector3.Lerp(startScale, targetScale, eval);

            yield return null;
        }

        panel.position   = targetPos;
        panel.localScale = targetScale;

        if (!opening)
        {
            panel.gameObject.SetActive(false);
            SetActiveButtonsVisible(true);
            SetExtraObjects(true);
            _panelOpen         = false;
            _currentPanelIndex = -1;
        }
    }

    // =====================================================
    //  UPDATE
    // =====================================================

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_panelOpen && !IsPointerOverPanel(entries[_currentPanelIndex].panel))
                CloseCurrentPanel();

            if (_giftPanelOpen && !IsPointerOverPanel(giftPanel))
                CloseGiftPanel();
        }
    }

    // =====================================================
    //  HELPERS
    // =====================================================

    bool IsPointerOverPanel(RectTransform panelRect)
    {
        if (panelRect == null) return false;

        PointerEventData ed = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ed, results);

        foreach (var r in results)
            if (r.gameObject.transform.IsChildOf(panelRect))
                return true;

        return false;
    }

    void SetActiveButtonsVisible(bool visible)
    {
        foreach (int idx in _activeIndices)
            if (entries[idx].button != null)
                entries[idx].button.gameObject.SetActive(visible);
    }

    void SetExtraObjects(bool enable)
    {
        if (objectsToDisableOnOpen == null) return;
        foreach (GameObject obj in objectsToDisableOnOpen)
            if (obj != null)
                obj.SetActive(enable);
    }


    void StartGiftBlink()
{
    StopGiftBlink();
    _giftBlinkCoroutine = StartCoroutine(GiftBlinkRoutine());
}

void StopGiftBlink()
{
    if (_giftBlinkCoroutine != null)
    {
        StopCoroutine(_giftBlinkCoroutine);
        _giftBlinkCoroutine = null;
    }

    if (giftButtonIndex < entries.Count &&
        entries[giftButtonIndex].button != null)
    {
        Image img = entries[giftButtonIndex].button.GetComponent<Image>();

        if (img != null)
            img.color = _giftOriginalColor;
    }
}

IEnumerator GiftBlinkRoutine()
{
    Image img = entries[giftButtonIndex].button.GetComponent<Image>();

    if (img == null)
        yield break;

    while (true)
    {
        // 5 sec wait
        yield return new WaitForSecondsRealtime(blinkInterval);

        // Blink ON
        img.color = blinkColor;

        yield return new WaitForSecondsRealtime(blinkDuration);

        // Blink OFF
        img.color = _giftOriginalColor;
    }
}

void ExitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
}

}