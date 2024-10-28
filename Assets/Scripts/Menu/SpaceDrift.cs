using UnityEngine;

public class SpaceDrift : MonoBehaviour
{
    // Adjustable rotation speed
    public Vector3 rotationSpeed = new Vector3(1f, 1f, 1f);

    // Randomized drift speeds
    private Vector3 driftSpeed;

    void Start()
    {
        // Initialize drift speed with a small, random value for each axis
        driftSpeed = new Vector3(
            Random.Range(-rotationSpeed.x, rotationSpeed.x),
            Random.Range(-rotationSpeed.y, rotationSpeed.y),
            Random.Range(-rotationSpeed.z, rotationSpeed.z)
        );
    }

    void Update()
    {
        // Apply rotation to simulate drifting
        transform.Rotate(driftSpeed * Time.deltaTime);
    }
}
