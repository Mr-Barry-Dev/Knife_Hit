using UnityEngine;

public class RotateFlipDirection_Array : MonoBehaviour
{
    [Header("Objects To Rotate")]
    public Transform[] targets;

    public float speed = 120f;
    public float changeTime = 3f;

    float timer;

    void Update()
    {
        if (targets == null) return;

        timer += Time.deltaTime;

        if (timer >= changeTime)
        {
            speed = -speed;
            timer = 0f;
        }

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}