using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The tags of the objects to spawn
    public string[] objectTags;
    // The minimum spawn rate (in seconds)
    public float minSpawnRate;
    // The maximum spawn rate (in seconds)
    public float maxSpawnRate;
    // The rate at which the spawn rate increases
    public float spawnRateAcceleration;

    // The amount of time until the next spawn
    private float _spawnTimer;
    // The current spawn rate
    private float _currentSpawnRate;
    // A flag to check if the spawning is active or not
    private bool _isSpawning = false;

    void Start() {
        _currentSpawnRate = maxSpawnRate;
        _spawnTimer = Random.Range(minSpawnRate, maxSpawnRate); ;
    }

    void Update() {
        if (!_isSpawning) return;
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0) {
            // Pick a random object tag from the array
            string tag = objectTags[Random.Range(0, objectTags.Length)];
            // Spawn an object from the pool using the spawner's position and rotation
            ObjectPooler.Instance.SpawnFromPool(tag, transform.position, transform.rotation);
            // Increase the spawn rate
            _currentSpawnRate = Mathf.Clamp(_currentSpawnRate - spawnRateAcceleration * Time.deltaTime, minSpawnRate, maxSpawnRate);
            // Reset the spawn timer with the new spawn rate
            _spawnTimer = _currentSpawnRate;
        }
    }


    // Method to stop the spawning of objects
    public void StopSpawning() {
        _isSpawning = false;
    }

    // Method to start the spawning of objects
    public void StartSpawning() {
        _isSpawning = true;
    }

}

