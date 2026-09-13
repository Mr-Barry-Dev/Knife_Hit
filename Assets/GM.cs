using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour
{
    [Header("===== CIRCLE =====")]
    public Transform rotatingCircle;
    public float rotateSpeed = 200f;

    [Header("===== KNIFE =====")]
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public float knifeSpeed = 30f;

    [Header("===== HIT EFFECT =====")]
    public float knifeInsertDistance = 0.15f;
    public float insertSpeed = 20f;

    private GameObject currentKnife;
    private bool gameOver = false;

    private OtherGameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<OtherGameManager>();
    }

    void Update()
    {
        if (gameManager == null)
            return;

        if (!gameManager.gameStarted || gameOver)
            return;

        rotatingCircle.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && currentKnife != null)
        {
            ThrowKnife();
        }
    }

    public void StartGame()
    {
        gameOver = false;
        SpawnKnife();
    }

    void ThrowKnife()
    {
        Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.up * knifeSpeed;

        currentKnife = null;
    }

    public void SpawnKnife()
    {
        currentKnife = Instantiate(
            knifePrefab,
            knifeSpawnPoint.position,
            Quaternion.identity);

        currentKnife.GetComponent<Knife1>().manager = gameManager;
    }

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
        Vector3 targetPos =
            startPos + knifeObj.transform.up * knifeInsertDistance;

        float t = 0f;

        while (t < 1f)
        {
            if (knifeObj == null)
                yield break;

            t += Time.deltaTime * insertSpeed;

            knifeObj.transform.position =
                Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        if (knifeObj == null || rotatingCircle == null)
            yield break;

        knifeObj.transform.SetParent(rotatingCircle);

        OtherGameManager2 gm2 =
            FindFirstObjectByType<OtherGameManager2>();

        if (gm2 != null)
            gm2.AddKnifeScore();

        SpawnKnife();
    }

    public void ResetChallenge()
    {
        StopAllCoroutines();

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

        if (gameManager.gameStarted)
            SpawnKnife();
    }

    public void GameOver()
    {
        gameOver = true;

        if (currentKnife != null)
        {
            Destroy(currentKnife);
            currentKnife = null;
        }
    }

    public void Reload()
    {
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

        SpawnKnife();
    }
}