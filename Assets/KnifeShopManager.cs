
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    private const string SELECTED_KEY = "SelectedKnife";

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
        if (currentPage < pages.Length - 1)
        {
            ShowPage(currentPage + 1);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            ShowPage(currentPage - 1);
        }
    }

    void ShowPage(int pageIndex)
    {
        if (pages.Length == 0)
            return;

        currentPage = Mathf.Clamp(
            pageIndex, 0, pages.Length - 1
        );

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].pageObject != null)
            {
                pages[i].pageObject.SetActive(
                    i == currentPage
                );
            }
        }

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

        UpdateShopUI();
    }

    // ================= MESSAGE =================

    void ShowMessage(string message)
    {
        if (shopMessage != null)
            shopMessage.text = message;

        Debug.Log(message);
    }
}