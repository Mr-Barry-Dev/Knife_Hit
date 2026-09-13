using UnityEngine;
public class Knife1 : MonoBehaviour
{
    [HideInInspector]
    public OtherGameManager manager;
    private bool stuck = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (stuck)
            return;
        // ================= HIT CIRCLE =================
        if (collision.gameObject.CompareTag("Circle"))
        {
            stuck = true;
            PlayStageHitEffect();
            manager.KnifeHitCircle(gameObject);
        }
        // ================= HIT KNIFE ==================
        else if (collision.gameObject.CompareTag("Knife"))
        {
            OtherGameManager2 gm2 =
                FindFirstObjectByType<OtherGameManager2>();
            if (gm2 != null)
            {
                gm2.TriggerGameOver();
            }
            Destroy(gameObject);
        }
    }
    // =================================================
    void PlayStageHitEffect()
    {
        OtherGameManager2 gm2 =
            FindFirstObjectByType<OtherGameManager2>();
        if (gm2 == null)
            return;
        OtherGameManager2.CircleHitEffect effectData =
            gm2.GetCurrentHitEffect();
        if (effectData == null)
            return;
        // ===== EFFECT =====
        if (effectData.hitEffect != null)
        {
            Instantiate(
                effectData.hitEffect,
                transform.position,
                Quaternion.identity);
        }
        // ===== SOUND =====
        if (effectData.hitSound != null &&
            gm2.audioSource != null)
        {
            gm2.audioSource.PlayOneShot(
                effectData.hitSound);
        }
    }
}