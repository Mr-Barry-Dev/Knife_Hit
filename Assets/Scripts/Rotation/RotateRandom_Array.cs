using UnityEngine;

public class RotateRandom_Array : MonoBehaviour
{
    [Header("Objects To Rotate")]
    public Transform[] targets;

    float[] speeds;

    void Start()
    {
        if (targets == null) return;

        speeds = new float[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            float sp = Random.Range(70, 150);
            speeds[i] = Random.value > 0.5f ? sp : -sp;
        }
    }

    void Update()
    {
        if (targets == null) return;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, speeds[i] * Time.deltaTime);
        }
    }
}