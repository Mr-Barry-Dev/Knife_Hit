using UnityEngine;

public class RotateSpeedUp_Array : MonoBehaviour
{
    [Header("Objects To Rotate")]
    public Transform[] targets;

    public float speed = 50f;
    public float acceleration = 10f;

    void Update()
    {
        if (targets == null) return;

        speed += acceleration * Time.deltaTime;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}