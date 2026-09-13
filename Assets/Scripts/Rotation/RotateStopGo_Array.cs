using UnityEngine;

public class RotateStopGo_Array : MonoBehaviour
{
    [Header("Objects To Rotate")]
    public Transform[] targets;

    public float speed = 120f;

    float timer;

    void Update()
    {
        if (targets == null) return;

        timer += Time.deltaTime;

        if (timer < 2f)
        {
            Rotate();
        }
        else if (timer > 3f)
        {
            timer = 0f;
        }
    }

    void Rotate()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}