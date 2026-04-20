using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        Instance = this;

        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objectPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true, 
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public GameObject GetObject()
    {
        return pool.Get();
    }

    public void ReturnObject(GameObject obj)
    {
        pool.Release(obj);
    }
}