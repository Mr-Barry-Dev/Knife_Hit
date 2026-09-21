// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections.Generic;

// public class KnifeShopManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class KnifePage
//     {
//         [Header("===== PAGE OBJECT =====")]
//         public GameObject pageObject;

//         [Header("===== KNIFE BUTTONS =====")]
//         public Button[] knifeButtons;

//         [Header("===== KNIFE OBJECTS =====")]
//         public GameObject[] knifeObjects;

//         [Header("===== SELECTED INDICATORS =====")]
//         public GameObject[] selectedIndicators;

//         [Header("===== LOCK INDICATORS =====")]
//         public GameObject[] lockIndicators;
//     }

//     [Header("===== SHOP PAGES =====")]
//     public KnifePage[] pages;

//     [Header("===== PAGE DOTS =====")]
//     public Image[] pageDots;
//     public Color activeDotColor = Color.yellow;
//     public Color inactiveDotColor = Color.white;

//     [Header("===== KNIFE PREVIEW =====")]
//     public Transform previewParent;

//     [Header("===== RANDOM KNIFE PICK =====")]
//     public Button randomPickButton;
//     public int randomPickCost = 250;

//     [Header("===== SHOP MESSAGE =====")]
//     public TMP_Text shopMessage;

//     [Header("===== SWIPE SETTINGS =====")]
//     public float swipeThreshold = 100f;

//     [Header("===== SELECTED KNIFE =====")]
//     public int selectedKnifeIndex = 0;

//     private int currentPage = 0;
//     private int totalKnives = 0;

//     private Vector2 swipeStartPosition;
//     private bool isSwiping = false;

//     private const string SELECTED_KEY = "SelectedKnife";


//     [Header("===== PAGE LEFT / RIGHT BUTTONS =====")]
// public Button leftPageButton;
// public Button rightPageButton;

//     private void Start()
//     {
//         CountKnives();

//         if (totalKnives == 0)
//         {
//             Debug.LogWarning("No knives assigned!");
//             return;
//         }

//         selectedKnifeIndex = PlayerPrefs.GetInt(
//             SELECTED_KEY, 0
//         );

//         selectedKnifeIndex = Mathf.Clamp(
//             selectedKnifeIndex, 0, totalKnives - 1
//         );

//         SetupButtons();

//         if (leftPageButton != null)
// {
//     leftPageButton.onClick.RemoveListener(PreviousPage);
//     leftPageButton.onClick.AddListener(PreviousPage);
// }

// if (rightPageButton != null)
// {
//     rightPageButton.onClick.RemoveListener(NextPage);
//     rightPageButton.onClick.AddListener(NextPage);
// }

//         // First knife is free for a new player
//         if (!IsKnifeOwned(selectedKnifeIndex))
//         {
//             UnlockKnife(selectedKnifeIndex);
//         }

//         ShowPage(0);
//         SelectKnife(selectedKnifeIndex);

//         if (randomPickButton != null)
//         {
//             randomPickButton.onClick.RemoveListener(
//                 PickRandomKnife
//             );

//             randomPickButton.onClick.AddListener(
//                 PickRandomKnife
//             );
//         }

//         UpdateShopUI();
//     }

//     // ================= COUNT KNIVES =================

//     void CountKnives()
//     {
//         totalKnives = 0;

//         foreach (KnifePage page in pages)
//         {
//             if (page.knifeObjects != null)
//                 totalKnives += page.knifeObjects.Length;
//         }
//     }

//     // ================= SETUP BUTTONS =================

//     void SetupButtons()
//     {
//         for (int p = 0; p < pages.Length; p++)
//         {
//             int pageIndex = p;

//             if (pages[p].knifeButtons == null)
//                 continue;

//             for (int k = 0;
//                  k < pages[p].knifeButtons.Length; k++)
//             {
//                 int knifeIndex = k;

//                 Button btn = pages[p].knifeButtons[k];

//                 if (btn == null)
//                     continue;

//                 btn.onClick.RemoveAllListeners();

//                 btn.onClick.AddListener(() =>
//                 {
//                     SelectKnifeFromPage(
//                         pageIndex, knifeIndex
//                     );
//                 });
//             }
//         }
//     }

//     // ================= GLOBAL INDEX =================

//     int GetGlobalIndex(int pageIndex, int knifeIndex)
//     {
//         int globalIndex = knifeIndex;

//         for (int i = 0; i < pageIndex; i++)
//         {
//             globalIndex += pages[i].knifeObjects.Length;
//         }

//         return globalIndex;
//     }

//     // ================= OWNERSHIP =================

//     string GetOwnershipKey(int index)
//     {
//         return "KnifeOwned_" + index;
//     }

//     public bool IsKnifeOwned(int index)
//     {
//         return PlayerPrefs.GetInt(
//             GetOwnershipKey(index), 0
//         ) == 1;
//     }

//     void UnlockKnife(int index)
//     {
//         PlayerPrefs.SetInt(
//             GetOwnershipKey(index), 1
//         );

//         PlayerPrefs.Save();
//     }

//     // ================= SELECT KNIFE =================

//     void SelectKnifeFromPage(
//         int pageIndex, int knifeIndex)
//     {
//         int globalIndex = GetGlobalIndex(
//             pageIndex, knifeIndex
//         );

//         if (!IsKnifeOwned(globalIndex))
//         {
//             ShowMessage("Knife locked! Pick a random knife.");
//             return;
//         }

//         SelectKnife(globalIndex);
//     }

//     public void SelectKnife(int index)
//     {
//         if (index < 0 || index >= totalKnives)
//             return;

//         if (!IsKnifeOwned(index))
//         {
//             ShowMessage("Knife is locked!");
//             return;
//         }

//         selectedKnifeIndex = index;

//         PlayerPrefs.SetInt(
//             SELECTED_KEY, selectedKnifeIndex
//         );

//         PlayerPrefs.Save();

//         UpdatePreview();
//         UpdateShopUI();
//     }

//     // ================= RANDOM PICK =================

//     public void PickRandomKnife()
//     {
//         if (GemManager.Instance == null)
//         {
//             ShowMessage("GemManager not found!");
//             return;
//         }

//         if (pages == null ||
//             currentPage < 0 ||
//             currentPage >= pages.Length)
//         {
//             return;
//         }

//         KnifePage page = pages[currentPage];

//         if (page.knifeObjects == null ||
//             page.knifeObjects.Length == 0)
//         {
//             ShowMessage("No knives on this page!");
//             return;
//         }

//         // Find all locked knives on current page
//         List<int> lockedKnives = new List<int>();

//         for (int k = 0; k < page.knifeObjects.Length; k++)
//         {
//             int globalIndex = GetGlobalIndex(
//                 currentPage, k
//             );

//             if (!IsKnifeOwned(globalIndex))
//             {
//                 lockedKnives.Add(globalIndex);
//             }
//         }

//         // All knives already unlocked
//         if (lockedKnives.Count == 0)
//         {
//             ShowMessage("All knives unlocked!");
//             UpdateShopUI();
//             return;
//         }

//         // Spend gems only if a locked knife exists
//         if (!GemManager.Instance.TrySpendBlueGems(
//             randomPickCost))
//         {
//             ShowMessage("Not enough blue gems!");
//             return;
//         }

//         // Pick only from locked knives
//         int randomPosition = Random.Range(
//             0, lockedKnives.Count
//         );

//         int pickedKnifeIndex =
//             lockedKnives[randomPosition];

//         // Permanent ownership
//         UnlockKnife(pickedKnifeIndex);

//         // Select newly unlocked knife
//         SelectKnife(pickedKnifeIndex);

//         ShowMessage("New knife unlocked!");

//         UpdateShopUI();

//         Debug.Log(
//             "Unlocked knife index: " +
//             pickedKnifeIndex
//         );
//     }

//     // ================= UPDATE SHOP UI =================

//     void UpdateShopUI()
//     {
//         int globalIndex = 0;

//         for (int p = 0; p < pages.Length; p++)
//         {
//             KnifePage page = pages[p];

//             for (int k = 0;
//                  k < page.knifeObjects.Length; k++)
//             {
//                 bool owned = IsKnifeOwned(globalIndex);
//                 bool selected =
//                     globalIndex == selectedKnifeIndex;

//                 // Selected indicator
//                 if (page.selectedIndicators != null &&
//                     k < page.selectedIndicators.Length &&
//                     page.selectedIndicators[k] != null)
//                 {
//                     page.selectedIndicators[k]
//                         .SetActive(selected);
//                 }

//                 // Lock indicator
//                 if (page.lockIndicators != null &&
//                     k < page.lockIndicators.Length &&
//                     page.lockIndicators[k] != null)
//                 {
//                     page.lockIndicators[k]
//                         .SetActive(!owned);
//                 }

//                 globalIndex++;
//             }
//         }

//         // Disable random pick when all current page knives owned
//         if (randomPickButton != null &&
//             pages.Length > 0 &&
//             currentPage < pages.Length)
//         {
//             bool hasLockedKnife = false;

//             for (int k = 0;
//                  k < pages[currentPage].knifeObjects.Length;
//                  k++)
//             {
//                 int index = GetGlobalIndex(
//                     currentPage, k
//                 );

//                 if (!IsKnifeOwned(index))
//                 {
//                     hasLockedKnife = true;
//                     break;
//                 }
//             }

//             randomPickButton.interactable =
//                 hasLockedKnife;
//         }
//     }

//     // ================= PREVIEW =================

//     void UpdatePreview()
//     {
//         if (previewParent == null)
//             return;

//         foreach (Transform child in previewParent)
//         {
//             child.gameObject.SetActive(false);
//         }

//         int globalIndex = 0;

//         for (int p = 0; p < pages.Length; p++)
//         {
//             if (pages[p].knifeObjects == null)
//                 continue;

//             for (int k = 0;
//                  k < pages[p].knifeObjects.Length; k++)
//             {
//                 if (globalIndex == selectedKnifeIndex)
//                 {
//                     GameObject knife =
//                         pages[p].knifeObjects[k];

//                     if (knife != null)
//                         knife.SetActive(true);

//                     return;
//                 }

//                 globalIndex++;
//             }
//         }
//     }

//     public GameObject GetSelectedKnife()
//     {
//         int globalIndex = 0;

//         foreach (KnifePage page in pages)
//         {
//             if (page.knifeObjects == null)
//                 continue;

//             foreach (GameObject knife in page.knifeObjects)
//             {
//                 if (globalIndex == selectedKnifeIndex)
//                     return knife;

//                 globalIndex++;
//             }
//         }

//         return null;
//     }

//     // ================= SWIPE =================

//     void Update()
//     {
//         if (Input.touchCount > 0)
//         {
//             Touch touch = Input.GetTouch(0);

//             if (touch.phase == TouchPhase.Began)
//             {
//                 swipeStartPosition = touch.position;
//                 isSwiping = true;
//             }

//             if (touch.phase == TouchPhase.Ended && isSwiping)
//             {
//                 CheckSwipe(touch.position);
//                 isSwiping = false;
//             }

//             if (touch.phase == TouchPhase.Canceled)
//                 isSwiping = false;
//         }
//         else
//         {
//             if (Input.GetMouseButtonDown(0))
//             {
//                 swipeStartPosition = Input.mousePosition;
//                 isSwiping = true;
//             }

//             if (Input.GetMouseButtonUp(0) && isSwiping)
//             {
//                 CheckSwipe(Input.mousePosition);
//                 isSwiping = false;
//             }
//         }
//     }

//     void CheckSwipe(Vector2 endPosition)
//     {
//         float distance =
//             endPosition.x - swipeStartPosition.x;

//         if (Mathf.Abs(distance) < swipeThreshold)
//             return;

//         if (distance < 0)
//             NextPage();
//         else
//             PreviousPage();
//     }

//     // ================= PAGE NAVIGATION =================

//     public void NextPage()
//     {
//         if (currentPage < pages.Length - 1)
//         {
//             ShowPage(currentPage + 1);
//         }
//     }

//     public void PreviousPage()
//     {
//         if (currentPage > 0)
//         {
//             ShowPage(currentPage - 1);
//         }
//     }

//     void ShowPage(int pageIndex)
// {
//     if (pages == null || pages.Length == 0)
//         return;

//     currentPage = Mathf.Clamp(
//         pageIndex, 0, pages.Length - 1
//     );

//     // Show only current page
//     for (int i = 0; i < pages.Length; i++)
//     {
//         if (pages[i].pageObject != null)
//         {
//             pages[i].pageObject.SetActive(
//                 i == currentPage
//             );
//         }
//     }

//     // Update page dots
//     if (pageDots != null)
//     {
//         for (int i = 0; i < pageDots.Length; i++)
//         {
//             if (pageDots[i] != null)
//             {
//                 pageDots[i].color =
//                     i == currentPage
//                     ? activeDotColor
//                     : inactiveDotColor;
//             }
//         }
//     }

//     // Left button: disabled on first page
//     if (leftPageButton != null)
//     {
//         leftPageButton.interactable =
//             currentPage > 0;
//     }

//     // Right button: disabled on last page
//     if (rightPageButton != null)
//     {
//         rightPageButton.interactable =
//             currentPage < pages.Length - 1;
//     }

//     UpdateShopUI();
// }

//     // ================= MESSAGE =================

//     void ShowMessage(string message)
//     {
//         if (shopMessage != null)
//             shopMessage.text = message;

//         Debug.Log(message);
//     }
// }



using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class KnifeShopManager : MonoBehaviour
{
    [System.Serializable]
    public class KnifePage
    {
        [Header("===== PAGE OBJECT =====")]
        public GameObject pageObject;

        [Header("===== KNIFE BUTTONS =====")]
        public Button[] knifeButtons;

        [Header("===== KNIFE OBJECTS =====")]
        public GameObject[] knifeObjects;

        [Header("===== SELECTED INDICATORS =====")]
        public GameObject[] selectedIndicators;

        [Header("===== LOCK INDICATORS =====")]
        public GameObject[] lockIndicators;
    }

    [Header("===== SHOP PAGES =====")]
    public KnifePage[] pages;

    [Header("===== PAGE DOTS =====")]
    public Image[] pageDots;
    public Color activeDotColor = Color.yellow;
    public Color inactiveDotColor = Color.white;

    [Header("===== KNIFE PREVIEW =====")]
    public Transform previewParent;

    [Header("===== RANDOM KNIFE PICK =====")]
    public Button randomPickButton;
    public int randomPickCost = 250;

    [Header("===== SHOP MESSAGE =====")]
    public TMP_Text shopMessage;

    [Header("===== SWIPE SETTINGS =====")]
    public float swipeThreshold = 100f;

    [Header("===== SELECTED KNIFE =====")]
    public int selectedKnifeIndex = 0;

    private int currentPage = 0;
    private int totalKnives = 0;

    private Vector2 swipeStartPosition;
    private bool isSwiping = false;

    [Header("===== PAGE SLIDE ANIMATION =====")]
public float pageSlideDuration = 0.3f;
public float pageSlideDistance = 800f;

private bool isPageAnimating = false;
private Coroutine pageAnimationCoroutine;

    private const string SELECTED_KEY = "SelectedKnife";


    [Header("===== PAGE LEFT / RIGHT BUTTONS =====")]
public Button leftPageButton;
public Button rightPageButton;

    private void Start()
    {
        CountKnives();

        if (totalKnives == 0)
        {
            Debug.LogWarning("No knives assigned!");
            return;
        }

        selectedKnifeIndex = PlayerPrefs.GetInt(
            SELECTED_KEY, 0
        );

        selectedKnifeIndex = Mathf.Clamp(
            selectedKnifeIndex, 0, totalKnives - 1
        );

        SetupButtons();

        if (leftPageButton != null)
{
    leftPageButton.onClick.RemoveListener(PreviousPage);
    leftPageButton.onClick.AddListener(PreviousPage);
}

if (rightPageButton != null)
{
    rightPageButton.onClick.RemoveListener(NextPage);
    rightPageButton.onClick.AddListener(NextPage);
}

        // First knife is free for a new player
        if (!IsKnifeOwned(selectedKnifeIndex))
        {
            UnlockKnife(selectedKnifeIndex);
        }

        ShowPage(0);
        SelectKnife(selectedKnifeIndex);

        if (randomPickButton != null)
        {
            randomPickButton.onClick.RemoveListener(
                PickRandomKnife
            );

            randomPickButton.onClick.AddListener(
                PickRandomKnife
            );
        }

        UpdateShopUI();
    }

    // ================= COUNT KNIVES =================

    void CountKnives()
    {
        totalKnives = 0;

        foreach (KnifePage page in pages)
        {
            if (page.knifeObjects != null)
                totalKnives += page.knifeObjects.Length;
        }
    }

    // ================= SETUP BUTTONS =================

    void SetupButtons()
    {
        for (int p = 0; p < pages.Length; p++)
        {
            int pageIndex = p;

            if (pages[p].knifeButtons == null)
                continue;

            for (int k = 0;
                 k < pages[p].knifeButtons.Length; k++)
            {
                int knifeIndex = k;

                Button btn = pages[p].knifeButtons[k];

                if (btn == null)
                    continue;

                btn.onClick.RemoveAllListeners();

                btn.onClick.AddListener(() =>
                {
                    SelectKnifeFromPage(
                        pageIndex, knifeIndex
                    );
                });
            }
        }
    }

    // ================= GLOBAL INDEX =================

    int GetGlobalIndex(int pageIndex, int knifeIndex)
    {
        int globalIndex = knifeIndex;

        for (int i = 0; i < pageIndex; i++)
        {
            globalIndex += pages[i].knifeObjects.Length;
        }

        return globalIndex;
    }

    // ================= OWNERSHIP =================

    string GetOwnershipKey(int index)
    {
        return "KnifeOwned_" + index;
    }

    public bool IsKnifeOwned(int index)
    {
        return PlayerPrefs.GetInt(
            GetOwnershipKey(index), 0
        ) == 1;
    }

    void UnlockKnife(int index)
    {
        PlayerPrefs.SetInt(
            GetOwnershipKey(index), 1
        );

        PlayerPrefs.Save();
    }

    // ================= SELECT KNIFE =================

    void SelectKnifeFromPage(
        int pageIndex, int knifeIndex)
    {
        int globalIndex = GetGlobalIndex(
            pageIndex, knifeIndex
        );

        if (!IsKnifeOwned(globalIndex))
        {
            ShowMessage("Knife locked! Pick a random knife.");
            return;
        }

        SelectKnife(globalIndex);
    }

    public void SelectKnife(int index)
    {
        if (index < 0 || index >= totalKnives)
            return;

        if (!IsKnifeOwned(index))
        {
            ShowMessage("Knife is locked!");
            return;
        }

        selectedKnifeIndex = index;

        PlayerPrefs.SetInt(
            SELECTED_KEY, selectedKnifeIndex
        );

        PlayerPrefs.Save();

        UpdatePreview();
        UpdateShopUI();
    }

    // ================= RANDOM PICK =================

    public void PickRandomKnife()
    {
        if (GemManager.Instance == null)
        {
            ShowMessage("GemManager not found!");
            return;
        }

        if (pages == null ||
            currentPage < 0 ||
            currentPage >= pages.Length)
        {
            return;
        }

        KnifePage page = pages[currentPage];

        if (page.knifeObjects == null ||
            page.knifeObjects.Length == 0)
        {
            ShowMessage("No knives on this page!");
            return;
        }

        // Find all locked knives on current page
        List<int> lockedKnives = new List<int>();

        for (int k = 0; k < page.knifeObjects.Length; k++)
        {
            int globalIndex = GetGlobalIndex(
                currentPage, k
            );

            if (!IsKnifeOwned(globalIndex))
            {
                lockedKnives.Add(globalIndex);
            }
        }

        // All knives already unlocked
        if (lockedKnives.Count == 0)
        {
            ShowMessage("All knives unlocked!");
            UpdateShopUI();
            return;
        }

        // Spend gems only if a locked knife exists
        if (!GemManager.Instance.TrySpendBlueGems(
            randomPickCost))
        {
            ShowMessage("Not enough blue gems!");
            return;
        }

        // Pick only from locked knives
        int randomPosition = Random.Range(
            0, lockedKnives.Count
        );

        int pickedKnifeIndex =
            lockedKnives[randomPosition];

        // Permanent ownership
        UnlockKnife(pickedKnifeIndex);

        // Select newly unlocked knife
        SelectKnife(pickedKnifeIndex);

        ShowMessage("New knife unlocked!");

        UpdateShopUI();

        Debug.Log(
            "Unlocked knife index: " +
            pickedKnifeIndex
        );
    }

    // ================= UPDATE SHOP UI =================

    void UpdateShopUI()
    {
        int globalIndex = 0;

        for (int p = 0; p < pages.Length; p++)
        {
            KnifePage page = pages[p];

            for (int k = 0;
                 k < page.knifeObjects.Length; k++)
            {
                bool owned = IsKnifeOwned(globalIndex);
                bool selected =
                    globalIndex == selectedKnifeIndex;

                // Selected indicator
                if (page.selectedIndicators != null &&
                    k < page.selectedIndicators.Length &&
                    page.selectedIndicators[k] != null)
                {
                    page.selectedIndicators[k]
                        .SetActive(selected);
                }

                // Lock indicator
                if (page.lockIndicators != null &&
                    k < page.lockIndicators.Length &&
                    page.lockIndicators[k] != null)
                {
                    page.lockIndicators[k]
                        .SetActive(!owned);
                }

                globalIndex++;
            }
        }

        // Disable random pick when all current page knives owned
        if (randomPickButton != null &&
            pages.Length > 0 &&
            currentPage < pages.Length)
        {
            bool hasLockedKnife = false;

            for (int k = 0;
                 k < pages[currentPage].knifeObjects.Length;
                 k++)
            {
                int index = GetGlobalIndex(
                    currentPage, k
                );

                if (!IsKnifeOwned(index))
                {
                    hasLockedKnife = true;
                    break;
                }
            }

            randomPickButton.interactable =
                hasLockedKnife;
        }
    }

    // ================= PREVIEW =================

    void UpdatePreview()
    {
        if (previewParent == null)
            return;

        foreach (Transform child in previewParent)
        {
            child.gameObject.SetActive(false);
        }

        int globalIndex = 0;

        for (int p = 0; p < pages.Length; p++)
        {
            if (pages[p].knifeObjects == null)
                continue;

            for (int k = 0;
                 k < pages[p].knifeObjects.Length; k++)
            {
                if (globalIndex == selectedKnifeIndex)
                {
                    GameObject knife =
                        pages[p].knifeObjects[k];

                    if (knife != null)
                        knife.SetActive(true);

                    return;
                }

                globalIndex++;
            }
        }
    }

    public GameObject GetSelectedKnife()
    {
        int globalIndex = 0;

        foreach (KnifePage page in pages)
        {
            if (page.knifeObjects == null)
                continue;

            foreach (GameObject knife in page.knifeObjects)
            {
                if (globalIndex == selectedKnifeIndex)
                    return knife;

                globalIndex++;
            }
        }

        return null;
    }

    // ================= SWIPE =================

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPosition = touch.position;
                isSwiping = true;
            }

            if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                CheckSwipe(touch.position);
                isSwiping = false;
            }

            if (touch.phase == TouchPhase.Canceled)
                isSwiping = false;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                swipeStartPosition = Input.mousePosition;
                isSwiping = true;
            }

            if (Input.GetMouseButtonUp(0) && isSwiping)
            {
                CheckSwipe(Input.mousePosition);
                isSwiping = false;
            }
        }
    }

    void CheckSwipe(Vector2 endPosition)
    {
        float distance =
            endPosition.x - swipeStartPosition.x;

        if (Mathf.Abs(distance) < swipeThreshold)
            return;

        if (distance < 0)
            NextPage();
        else
            PreviousPage();
    }

    // ================= PAGE NAVIGATION =================

    public void NextPage()
{
    if (isPageAnimating)
        return;

    if (currentPage < pages.Length - 1)
    {
        ShowPage(currentPage + 1);
    }
}

public void PreviousPage()
{
    if (isPageAnimating)
        return;

    if (currentPage > 0)
    {
        ShowPage(currentPage - 1);
    }
}

    void ShowPage(int pageIndex)
{
    if (pages == null || pages.Length == 0)
        return;

    pageIndex = Mathf.Clamp(
        pageIndex, 0, pages.Length - 1
    );

    if (pageIndex == currentPage && 
        pages[pageIndex].pageObject.activeSelf)
        return;

    if (pageAnimationCoroutine != null)
    {
        StopCoroutine(pageAnimationCoroutine);
    }

    pageAnimationCoroutine = StartCoroutine(
        AnimatePage(currentPage, pageIndex)
    );
}


IEnumerator AnimatePage(int oldPage, int newPage)
{
    isPageAnimating = true;

    RectTransform oldRect = null;
    RectTransform newRect = null;

    if (oldPage >= 0 && oldPage < pages.Length)
    {
        if (pages[oldPage].pageObject != null)
        {
            oldRect = pages[oldPage].pageObject
                .GetComponent<RectTransform>();
        }
    }

    if (pages[newPage].pageObject != null)
    {
        newRect = pages[newPage].pageObject
            .GetComponent<RectTransform>();
    }

    if (newRect == null)
    {
        isPageAnimating = false;
        yield break;
    }

    // Direction
    float direction = newPage > oldPage ? -1f : 1f;

    // Enable new page
    pages[newPage].pageObject.SetActive(true);

    Vector2 newStartPosition =
        newRect.anchoredPosition;

    Vector2 newEndPosition =
        newStartPosition;

    newStartPosition.x +=
        -direction * pageSlideDistance;

    newRect.anchoredPosition =
        newStartPosition;

    Vector2 oldStartPosition = Vector2.zero;
    Vector2 oldEndPosition = Vector2.zero;

    if (oldRect != null)
    {
        oldStartPosition = oldRect.anchoredPosition;

        oldEndPosition = oldStartPosition;

        oldEndPosition.x +=
            direction * pageSlideDistance;
    }

    float elapsed = 0f;

    while (elapsed < pageSlideDuration)
    {
        elapsed += Time.unscaledDeltaTime;

        float t = Mathf.Clamp01(
            elapsed / pageSlideDuration
        );

        // Smooth movement
        float smoothT = Mathf.SmoothStep(
            0f, 1f, t
        );

        newRect.anchoredPosition =
            Vector2.Lerp(
                newStartPosition,
                newEndPosition,
                smoothT
            );

        if (oldRect != null)
        {
            oldRect.anchoredPosition =
                Vector2.Lerp(
                    oldStartPosition,
                    oldEndPosition,
                    smoothT
                );
        }

        yield return null;
    }

    // Final positions
    newRect.anchoredPosition = newEndPosition;

    if (oldRect != null)
    {
        oldRect.anchoredPosition = oldStartPosition;

        pages[oldPage].pageObject.SetActive(false);
    }

    currentPage = newPage;

    UpdatePageDots();

    UpdateShopUI();

    isPageAnimating = false;

    pageAnimationCoroutine = null;
}


void UpdatePageDots()
{
    if (pageDots == null)
        return;

    for (int i = 0; i < pageDots.Length; i++)
    {
        if (pageDots[i] != null)
        {
            pageDots[i].color =
                i == currentPage
                ? activeDotColor
                : inactiveDotColor;
        }
    }

    if (leftPageButton != null)
    {
        leftPageButton.interactable =
            currentPage > 0;
    }

    if (rightPageButton != null)
    {
        rightPageButton.interactable =
            currentPage < pages.Length - 1;
    }
}

    // ================= MESSAGE =================

    void ShowMessage(string message)
    {
        if (shopMessage != null)
            shopMessage.text = message;

        Debug.Log(message);
    }
}