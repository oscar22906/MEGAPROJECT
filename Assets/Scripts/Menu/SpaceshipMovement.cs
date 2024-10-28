using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    // Adjustable speed for forward movement
    public float forwardSpeed = 5f;

    // Adjustable intensity for jittery rotation effect
    public float rotationShakeIntensity = 1f;

    // Adjustable frequency for rotation shake effect
    public float rotationShakeFrequency = 20f;

    // Store original rotation for shake calculations
    private Quaternion originalRotation;

    void Start()
    {
        // Save the original rotation to add jitter around it
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Move the spaceship forward
        transform.position += transform.right * forwardSpeed * Time.deltaTime;

        // Calculate jittery rotation shake
        float shakeX = Mathf.PerlinNoise(Time.time * rotationShakeFrequency, 0f) * rotationShakeIntensity - rotationShakeIntensity / 2;
        float shakeY = Mathf.PerlinNoise(0f, Time.time * rotationShakeFrequency) * rotationShakeIntensity - rotationShakeIntensity / 2;
        float shakeZ = Mathf.PerlinNoise(Time.time * rotationShakeFrequency, Time.time * rotationShakeFrequency) * rotationShakeIntensity - rotationShakeIntensity / 2;

        // Apply shake to the spaceship's rotation
        transform.rotation = originalRotation * Quaternion.Euler(shakeX, shakeY, shakeZ);
    }
}
