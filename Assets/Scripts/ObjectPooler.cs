using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    class Pool
    {
        public string tag;
        public GameObject gameObject;
        public int size;
    }
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    [SerializeField] List<Pool> pools;

    #region Singleton
    public static ObjectPooler Instance;
    void Awake()
    {
        Instance = this;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectToSpawn = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.gameObject);
                obj.SetActive(false);
                objectToSpawn.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectToSpawn);
        }
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation, Vector2 direction)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;




        BulletMovement bulletMovement = objectToSpawn.GetComponent<BulletMovement>();
        if (bulletMovement != null)
        {
            bulletMovement.SetDirection(direction);
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;

    }

}
