using UnityEngine;

public class SmoothFollow2DCamera : MonoBehaviour
{
    [Header("Camera settings")]
    [Tooltip("Reference to the target GameObject")]
    public Transform target;
    [Tooltip("Current relative offset to the target")]
    public Vector3 offset;
    [Tooltip("Multiplier for the movement speed")]
    [Range(0f, 5f)]
    public float smoothSpeed = 0.125f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 position = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, position, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
