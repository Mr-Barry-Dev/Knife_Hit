using UnityEngine;
using System.Collections;

public class BossRotation : MonoBehaviour
{
    [Header("BOSS SPRITE")]
    public SpriteRenderer targetRenderer;
    public Sprite bossSprite;

    [Header("ROTATION")]
    public float minSpeed = 150f;
    public float maxSpeed = 400f;

    [Range(0f, 1f)]
    public float stopChance = 0.4f;

    public float stopDuration = 0.5f;
    public float minChangeTime = 0.5f;
    public float maxChangeTime = 2f;

    private float currentSpeed;
    private Coroutine rotationRoutine;

    void OnEnable()
    {
        if (targetRenderer != null && bossSprite != null)
            targetRenderer.sprite = bossSprite;

        rotationRoutine = StartCoroutine(RandomRotationRoutine());
    }

    void OnDisable()
    {
        if (rotationRoutine != null)
            StopCoroutine(rotationRoutine);

        currentSpeed = 0f;
    }

    void Update()
    {
        if (targetRenderer != null)
        {
            targetRenderer.transform.Rotate(
                0f,
                0f,
                currentSpeed * Time.deltaTime);
        }
    }

    IEnumerator RandomRotationRoutine()
    {
        while (true)
        {
            int direction =
                Random.Range(0, 2) == 0 ? -1 : 1;

            currentSpeed =
                Random.Range(minSpeed, maxSpeed) * direction;

            yield return new WaitForSeconds(
                Random.Range(minChangeTime, maxChangeTime));

            if (Random.value <= stopChance)
            {
                currentSpeed = 0f;

                yield return new WaitForSeconds(stopDuration);
            }
        }
    }
}