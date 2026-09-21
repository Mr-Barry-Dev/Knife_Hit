// using UnityEngine;

// public class CircleEffectPiece : MonoBehaviour
// {
//     [HideInInspector]
//     public Vector2 direction;

//     [HideInInspector]
//     public float speed;

//     [HideInInspector]
//     public float lifeTime;

//     private float timer;

//     private SpriteRenderer sr;

//     private Color startColor;

//     private float rotateSpeed;


//     void Start()
//     {
//         sr = GetComponent<SpriteRenderer>();

//         if (sr != null)
//             startColor = sr.color;

//         // Har cone ki alag rotation speed
//         rotateSpeed =
//             Random.Range(500f, 900f);
//     }


//     void Update()
//     {
//         timer += Time.deltaTime;


//         // ==============================
//         // BLAST MOVEMENT
//         // ==============================

//         transform.position +=
//             (Vector3)(
//                 direction *
//                 speed *
//                 Time.deltaTime
//             );


//         // ==============================
//         // ROTATION
//         // ==============================

//         transform.Rotate(
//             0f,
//             0f,
//             rotateSpeed *
//             Time.deltaTime
//         );


//         // ==============================
//         // FADE
//         // ==============================

//         if (sr != null)
//         {
//             float alpha =
//                 Mathf.Lerp(
//                     1f,
//                     0f,
//                     timer / lifeTime
//                 );

//             sr.color = new Color(
//                 startColor.r,
//                 startColor.g,
//                 startColor.b,
//                 alpha
//             );
//         }


//         // ==============================
//         // DESTROY
//         // ==============================

//         if (timer >= lifeTime)
//         {
//             Destroy(gameObject);
//         }
//     }
// }



using UnityEngine;

public class CircleEffectPiece : MonoBehaviour
{
    [HideInInspector]
    public Vector2 direction;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public float lifeTime;

    private float timer;
    private SpriteRenderer sr;
    private Color startColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            startColor = sr.color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Move outward
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        // Rotate
        transform.Rotate(0f, 0f, 300f * Time.deltaTime);

        // Fade
        if (sr != null)
        {
            float alpha = Mathf.Lerp(
                1f,
                0f,
                timer / lifeTime
            );

            sr.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                alpha
            );
        }

        // Destroy
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}