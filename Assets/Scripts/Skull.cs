using UnityEngine;
using System.Collections;

public class Skull : MonoBehaviour
{
    [Header("EFFECT")]
    public GameObject destroyEffect;

    [Header("SOUND")]
    public AudioClip destroySound;

    private bool collected = false;

    private Collider2D col;
    private SpriteRenderer sr;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (other.CompareTag("Knife"))
        {
            collected = true;

            GameManager2 gm =
                FindFirstObjectByType<GameManager2>();

            if (gm != null)
            {
                if (GemManager.Instance != null)
{
    GemManager.Instance.AddSkullReward();
}

                Debug.Log(
                    "Total Skulls : " +
                    gm.totalSkulls);
            }

            // EFFECT
            if (destroyEffect != null)
            {
                Instantiate(
                    destroyEffect,
                    transform.position,
                    Quaternion.identity);
            }

            // SOUND
            if (destroySound != null)
            {
                AudioSource.PlayClipAtPoint(
                    destroySound,
                    transform.position);
            }

            // HIDE
            if (sr != null)
                sr.enabled = false;

            // COLLIDER OFF
            if (col != null)
                col.enabled = false;

            StartCoroutine(
                DestroyAfterTime());
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}