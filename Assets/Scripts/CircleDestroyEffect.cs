// using UnityEngine;

// public class CircleDestroyEffect : MonoBehaviour
// {
//     [Header("===== CONE =====")]
//     public Sprite coneSprite;

//     [Header("===== EFFECT SETTINGS =====")]
//     public int pieceCount = 18;

//     public float explosionForce = 12f;

//     public float pieceLifeTime = 0.8f;

//     public float minSize = 0.35f;
//     public float maxSize = 0.7f;

//     [Header("===== COLOR =====")]
//     [Range(1f, 3f)]
//     public float colorBrightness = 1.2f;


//     public void Play(Sprite circleSprite, Vector3 position)
//     {
//         transform.position = position;

//         // Current circle sprite se color nikalo
//         Color circleColor = GetSpriteColor(circleSprite);

//         for (int i = 0; i < pieceCount; i++)
//         {
//             GameObject piece = new GameObject("ConePiece");

//             piece.transform.position = position;

//             SpriteRenderer sr =
//                 piece.AddComponent<SpriteRenderer>();

//             // White cone sprite
//             sr.sprite = coneSprite;

//             // Circle ka detected color
//             sr.color = circleColor;

//             // Circle ke upar render ho
//             sr.sortingLayerName = "Default";
//             sr.sortingOrder = 20;

//             // Random bada size
//             float size =
//                 Random.Range(minSize, maxSize);

//             piece.transform.localScale =
//                 Vector3.one * size;

//             // Random blast direction
//             Vector2 direction =
//                 Random.insideUnitCircle.normalized;

//             // Random blast speed
//             float speed =
//                 Random.Range(
//                     explosionForce * 0.7f,
//                     explosionForce
//                 );

//             CircleEffectPiece effectPiece =
//                 piece.AddComponent<CircleEffectPiece>();

//             effectPiece.direction = direction;
//             effectPiece.speed = speed;
//             effectPiece.lifeTime = pieceLifeTime;

//             // Random starting rotation
//             piece.transform.rotation =
//                 Quaternion.Euler(
//                     0f,
//                     0f,
//                     Random.Range(0f, 360f)
//                 );
//         }

//         Destroy(
//             gameObject,
//             pieceLifeTime + 0.2f
//         );
//     }


//     Color GetSpriteColor(Sprite sprite)
//     {
//         if (sprite == null)
//             return Color.white;

//         Texture2D texture = sprite.texture;

//         try
//         {
//             Color[] pixels =
//                 texture.GetPixels(
//                     Mathf.RoundToInt(
//                         sprite.textureRect.x
//                     ),
//                     Mathf.RoundToInt(
//                         sprite.textureRect.y
//                     ),
//                     Mathf.RoundToInt(
//                         sprite.textureRect.width
//                     ),
//                     Mathf.RoundToInt(
//                         sprite.textureRect.height
//                     )
//                 );

//             Color totalColor = Color.black;

//             int validPixels = 0;

//             foreach (Color pixel in pixels)
//             {
//                 // Transparent pixels ignore
//                 if (pixel.a < 0.2f)
//                     continue;

//                 // Bahut dark pixels ignore
//                 if (pixel.r < 0.05f &&
//                     pixel.g < 0.05f &&
//                     pixel.b < 0.05f)
//                     continue;

//                 totalColor += pixel;

//                 validPixels++;
//             }

//             if (validPixels == 0)
//                 return Color.white;

//             Color averageColor =
//                 totalColor / validPixels;

//             // Thoda brightness increase
//             averageColor *= colorBrightness;

//             averageColor.a = 1f;

//             return averageColor;
//         }
//         catch
//         {
//             Debug.LogWarning(
//                 "Circle sprite texture Read/Write Enabled nahi hai."
//             );

//             return Color.white;
//         }
//     }
// }



using UnityEngine;

public class CircleDestroyEffect : MonoBehaviour
{
    public int pieceCount = 18;
    public float explosionForce = 2.5f;
    public float pieceLifeTime = 0.5f;
    public float minSize = 0.08f;
    public float maxSize = 0.18f;

    public void Play(Sprite circleSprite, Vector3 position)
    {
        transform.position = position;

        for (int i = 0; i < pieceCount; i++)
        {
            GameObject piece = new GameObject("CirclePiece");

            piece.transform.position = position;

            SpriteRenderer sr = piece.AddComponent<SpriteRenderer>();

            // EXACT same circle sprite
            sr.sprite = circleSprite;

            // Same sorting layer/order as circle
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 20;

            // Random small size
            float size = Random.Range(minSize, maxSize);
            piece.transform.localScale = Vector3.one * size;

            // Random direction
            Vector2 direction = Random.insideUnitCircle.normalized;

            // Random speed
            float speed = Random.Range(1.5f, explosionForce);

            CircleEffectPiece effectPiece =
                piece.AddComponent<CircleEffectPiece>();

            effectPiece.direction = direction;
            effectPiece.speed = speed;
            effectPiece.lifeTime = pieceLifeTime;

            // Random rotation
            piece.transform.rotation =
                Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }

        Destroy(gameObject, pieceLifeTime + 0.2f);
    }
}