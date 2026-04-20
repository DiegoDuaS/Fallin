using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    private float currentSpeed;

    void OnEnable()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 15f || Mathf.Abs(transform.position.y) > 10f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerPlayerHit(); 
            ReturnToPool();
        }
    }

    // Método auxiliar para centralizar el regreso al pool
    private void ReturnToPool()
    {
        // IMPORTANTE: Usamos el método que creamos en tu ObjectPooler
        ObjectPool.Instance.ReturnObject(this.gameObject);
    }
}