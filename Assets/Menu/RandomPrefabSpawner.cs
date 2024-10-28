using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;          // Array of prefabs to spawn
    public float spawnInterval = 2f;      // Time interval between spawns
    public float deleteDistance = 20f;    // Distance from the player to delete the prefab
    public float spawnRadius = 10f;       // Radius within which to spawn objects
    public float safeRadius = 2f;         // Minimum distance from the center (ship) to spawn
    public Transform ship;                // Reference to the player Transform

    private void Start()
    {
        // Start spawning prefabs at intervals
        InvokeRepeating(nameof(SpawnPrefab), 0f, spawnInterval);
    }

    private void Update()
    {
        // Delete prefabs that are too far from the player
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpawnedPrefab"))
        {
            if (Vector3.Distance(obj.transform.position, ship.position) > deleteDistance)
            {
                Destroy(obj);
            }
        }
    }

    private void SpawnPrefab()
    {
        if (prefabs.Length == 0) return;

        // Choose a random prefab from the array
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        // Generate a spawn position within the spherical radius but outside the safe zone
        Vector3 spawnPosition;
        do
        {
            spawnPosition = Random.insideUnitSphere * spawnRadius;
        }
        while (spawnPosition.magnitude < safeRadius);

        // Instantiate the prefab at the calculated position
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition + transform.position, Quaternion.identity);
        spawnedPrefab.tag = "SpawnedPrefab"; // Set tag for easy identification
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn radius as a sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Draw the safe radius as a smaller sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, safeRadius);
    }
}
