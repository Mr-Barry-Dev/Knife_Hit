using UnityEngine;
using System.Collections;

public class ClockwiseRotate : MonoBehaviour
{
    [Header("Object To Rotate")]
    public Transform target;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;
    public float rotateTime = 1f;
    public float stopTime = 0.5f;

    private bool canRotate = true;

    private void Start()
    {
        StartCoroutine(RotationRoutine());
    }

    private void Update()
    {
        if (canRotate && target != null)
        {
            target.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator RotationRoutine()
    {
        while (true)
        {
            canRotate = true;
            yield return new WaitForSeconds(rotateTime);

            canRotate = false;
            yield return new WaitForSeconds(stopTime);
        }
    }
}