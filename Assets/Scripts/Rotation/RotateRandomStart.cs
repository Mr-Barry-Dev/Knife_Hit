using UnityEngine;

public class RotateRandomStart : MonoBehaviour
{
     [Header("Objects To Rotate")]
    public Transform[] targets;

    public float speed = 120f;

    float[] directions;

    void Start()
    {
        if (targets == null) return;

        directions = new float[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            directions[i] = Random.value > 0.5f ? 1f : -1f;
        }
    }

    void Update()
    {
        if (targets == null) return;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, speed * directions[i] * Time.deltaTime);
        }
    }
}
