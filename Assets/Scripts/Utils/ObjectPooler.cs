using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int amountToPool;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake() {
        // Make the object persist across scene changes
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Iterate over the pools 
        foreach (Pool pool in pools) {
            // Create a new queue
            Queue<GameObject> objectPool = new Queue<GameObject>();
            // Instantiate objects and add them to the queue
            for (int i = 0; i < pool.amountToPool; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            // Add the queue to the dictionary with the object's tag as the key
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) ScoreManager.Instance.AddPoints(10);
    }

    // Method to spawn an object from the pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        // check if the pool exists
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogError("Pool with tag: " + tag + " doesn't exist!");
            return null;
        }

        // Get an object from the queue
        GameObject spawnedObj = poolDictionary[tag].Dequeue();
        spawnedObj.SetActive(true);
        spawnedObj.transform.SetPositionAndRotation(position, rotation);

        // Add the object back to the queue
        poolDictionary[tag].Enqueue(spawnedObj);

        // Check if the object is not destroyed or if the component is not null before calling this method
        if (spawnedObj != null) {
            IPooledObject pooledObject = spawnedObj.GetComponent<IPooledObject>();
            if (pooledObject != null) pooledObject.OnObjectSpawned();
        }

        return spawnedObj;
    }

    public void ReturnToPool(string tag, GameObject obj) {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogError("Pool with tag: " + tag + " doesn't exist!");
            return;
        }
        // Deactivate the object
        obj.SetActive(false);
        // Add the object back to the queue
        poolDictionary[tag].Enqueue(obj);
    }
}
