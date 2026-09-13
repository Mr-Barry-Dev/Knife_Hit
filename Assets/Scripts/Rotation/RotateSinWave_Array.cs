using UnityEngine;

public class RotateSinWave_Array : MonoBehaviour
{
    [Header("Objects To Rotate")]
    public Transform[] targets;

    public float speed = 200f;

    void Update()
    {
        if (targets == null) return;

        float currentSpeed = Mathf.Sin(Time.time) * speed;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, currentSpeed * Time.deltaTime);
        }
    }
}