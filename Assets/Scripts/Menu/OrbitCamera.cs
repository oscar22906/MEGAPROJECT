using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;                // The object to orbit around
    public float distance = 5.0f;           // Initial distance from the target
    public float minDistance = 2.0f;        // Minimum zoom distance
    public float maxDistance = 10.0f;       // Maximum zoom distance
    public float scrollSpeed = 2.0f;        // Speed of scroll zoom
    public float smoothSpeed = 2.0f;        // Speed of smooth transition
    public LayerMask collisionLayer;        // Layer mask for collision checks

    public float yMinLimit = -20f;           // Minimum vertical angle
    public float yMaxLimit = 80f;            // Maximum vertical angle
    public Vector2 randomRotationRange = new Vector2(0, 360);  // Range for random angles


    private float currentDistance;          // Current interpolated distance
    private Quaternion currentRotation;     // Current interpolated rotation
    private float targetX;                  // Target horizontal angle
    private float targetY;                  // Target vertical angle

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        targetX = angles.y;
        targetY = angles.x;
        currentRotation = Quaternion.Euler(targetY, targetX, 0);
        currentDistance = distance;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Adjust distance with scroll wheel input
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * scrollSpeed, minDistance, maxDistance);

            // Smoothly transition to the new distance
            currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * smoothSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                targetX = Random.Range(randomRotationRange.x, randomRotationRange.y);
                targetY = Random.Range(yMinLimit, yMaxLimit);
            }
            // Apply idle drift to target rotation
            targetX += 10f * Time.deltaTime;
            targetY = Mathf.Clamp(targetY, -20f, 80f);
            currentRotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(targetY, targetX, 0), Time.deltaTime * smoothSpeed);

            // Check for collisions and adjust distance
            Vector3 desiredPosition = target.position - currentRotation * Vector3.forward * currentDistance;
            if (Physics.Raycast(target.position, desiredPosition - target.position, out RaycastHit hit, currentDistance, collisionLayer))
            {
                currentDistance = Mathf.Clamp(hit.distance - 0.1f, minDistance, maxDistance);
            }

            // Set the camera's position and rotation
            transform.position = target.position - currentRotation * Vector3.forward * currentDistance;
            transform.rotation = currentRotation;
        }
    }
}
