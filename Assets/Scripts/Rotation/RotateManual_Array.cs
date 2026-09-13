using UnityEngine;

public class RotateManual_Array : MonoBehaviour
{
    [Header("Objects To Rotate")]
    public Transform[] targets;

    public float speed = 120f;

    void Update()
    {
        if (targets == null) return;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].Rotate(0, 0, speed * Time.deltaTime);
        }
    }

    public void ReverseAll()
    {
        speed = -speed;
    }
}