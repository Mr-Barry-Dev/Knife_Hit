
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageScrollController : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private float scrollSpeed = 0.35f;

    [Header("Buttons")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [Header("Dot Indicator")]
    [SerializeField] private Transform dotParent;
    [SerializeField] private GameObject dotPrefab;

    [Header("Dot Colors")]
    [SerializeField] private Color normalDotColor = Color.white;
    [SerializeField] private Color selectedDotColor = Color.yellow;

    [Header("Dot Size")]
    [SerializeField] private float dotWidth = 18f;
    [SerializeField] private float dotHeight = 18f;
    [SerializeField] private float dotSpacing = 10f;

    [Header("Swipe Settings")]
    [SerializeField] private float swipeThreshold = 0.15f;

    private List<GameObject> dots = new List<GameObject>();

    private int currentPage = 0;
    private int totalPages = 0;

    private Coroutine scrollCoroutine;

    private float dragStartPosition;

    private bool isDragging = false;


    private void Start()
    {
        SetupPages();
        CreateDots();
        UpdateUI();
    }


    private void SetupPages()
    {
        if (content == null)
        {
            Debug.LogError("PageScrollController: Content is not assigned!");
            return;
        }

        totalPages = content.childCount;

        if (totalPages <= 0)
        {
            Debug.LogWarning("PageScrollController: Content ke andar koi Page nahi hai.");
            return;
        }

        // Horizontal page setup
        scrollRect.horizontal = true;
        scrollRect.vertical = false;

        // Buttons
        if (leftButton != null)
        {
            leftButton.onClick.RemoveAllListeners();
            leftButton.onClick.AddListener(PreviousPage);
        }

        if (rightButton != null)
        {
            rightButton.onClick.RemoveAllListeners();
            rightButton.onClick.AddListener(NextPage);
        }

        // ScrollRect events
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
    }


    private void CreateDots()
    {
        if (dotParent == null)
        {
            Debug.LogWarning("PageScrollController: Dot Parent assign nahi hai.");
            return;
        }

        if (dotPrefab == null)
        {
            Debug.LogWarning("PageScrollController: Dot Prefab assign nahi hai.");
            return;
        }

        // Purane dots delete karo
        for (int i = dotParent.childCount - 1; i >= 0; i--)
        {
            Destroy(dotParent.GetChild(i).gameObject);
        }

        dots.Clear();

        // New dots create karo
        for (int i = 0; i < totalPages; i++)
        {
            GameObject newDot = Instantiate(dotPrefab, dotParent);

            newDot.name = "Dot_" + (i + 1);

            RectTransform dotRect = newDot.GetComponent<RectTransform>();

            if (dotRect != null)
            {
                dotRect.sizeDelta = new Vector2(dotWidth, dotHeight);
            }

            // Image component
            Image dotImage = newDot.GetComponent<Image>();

            if (dotImage != null)
            {
                dotImage.color = normalDotColor;
            }

            dots.Add(newDot);
        }

        // Dot spacing
        HorizontalLayoutGroup layoutGroup =
            dotParent.GetComponent<HorizontalLayoutGroup>();

        if (layoutGroup != null)
        {
            layoutGroup.spacing = dotSpacing;
        }
    }


    public void NextPage()
    {
        if (isDragging)
            return;

        if (currentPage >= totalPages - 1)
            return;

        currentPage++;

        ScrollToPage(currentPage);
    }


    public void PreviousPage()
    {
        if (isDragging)
            return;

        if (currentPage <= 0)
            return;

        currentPage--;

        ScrollToPage(currentPage);
    }


    private void ScrollToPage(int page)
    {
        if (totalPages <= 1)
            return;

        page = Mathf.Clamp(page, 0, totalPages - 1);

        float targetPosition;

        // Page position calculate
        targetPosition = (float)page / (totalPages - 1);

        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
        }

        scrollCoroutine = StartCoroutine(SmoothScroll(targetPosition));
    }


    private IEnumerator SmoothScroll(float targetPosition)
    {
        float startPosition = scrollRect.horizontalNormalizedPosition;

        float time = 0f;

        while (time < scrollSpeed)
        {
            time += Time.deltaTime;

            float t = time / scrollSpeed;

            // Smooth movement
            t = Mathf.SmoothStep(0f, 1f, t);

            scrollRect.horizontalNormalizedPosition =
                Mathf.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;

        UpdateUI();
    }


    private void OnScrollChanged(Vector2 position)
    {
        if (totalPages <= 1)
            return;

        // Current page calculate
        float normalizedPosition = scrollRect.horizontalNormalizedPosition;

        int calculatedPage = Mathf.RoundToInt(
            normalizedPosition * (totalPages - 1)
        );

        calculatedPage = Mathf.Clamp(
            calculatedPage,
            0,
            totalPages - 1
        );

        if (calculatedPage != currentPage)
        {
            currentPage = calculatedPage;

            UpdateUI();
        }
    }


    private void UpdateUI()
    {
        UpdateDots();
        UpdateButtons();
    }


    private void UpdateDots()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            Image dotImage = dots[i].GetComponent<Image>();

            if (dotImage == null)
                continue;

            if (i == currentPage)
            {
                // Selected page
                dotImage.color = selectedDotColor;
            }
            else
            {
                // Other pages
                dotImage.color = normalDotColor;
            }
        }
    }


    private void UpdateButtons()
    {
        if (leftButton != null)
        {
            leftButton.interactable = currentPage > 0;
        }

        if (rightButton != null)
        {
            rightButton.interactable = currentPage < totalPages - 1;
        }
    }


    // -----------------------------
    // SWIPE / DRAG SUPPORT
    // -----------------------------

    public void OnBeginDrag()
    {
        isDragging = true;

        dragStartPosition = scrollRect.horizontalNormalizedPosition;

        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
        }
    }


    public void OnEndDrag()
    {
        isDragging = false;

        float currentPosition =
            scrollRect.horizontalNormalizedPosition;

        float difference =
            currentPosition - dragStartPosition;

        // Swipe Left
        if (difference > swipeThreshold)
        {
            currentPage--;
        }

        // Swipe Right
        else if (difference < -swipeThreshold)
        {
            currentPage++;
        }
        else
        {
            // Nearest page
            currentPage = Mathf.RoundToInt(
                currentPosition * (totalPages - 1)
            );
        }

        currentPage = Mathf.Clamp(
            currentPage,
            0,
            totalPages - 1
        );

        ScrollToPage(currentPage);
    }


    // -----------------------------
    // PUBLIC GETTERS
    // -----------------------------

    public int GetCurrentPage()
    {
        return currentPage;
    }


    public int GetTotalPages()
    {
        return totalPages;
    }
}

