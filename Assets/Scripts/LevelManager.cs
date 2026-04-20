using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Game State")]
    public float timeSurvived { get; private set; }
    public int lives = 3;
    public int currentWave { get; private set; } = 1;
    public bool isGameOver { get; private set; }

    [SerializeField] private float timePerWave = 20f;
    private float waveTimer;

    [Header("Spawner Settings")]
    [SerializeField] private float initialSpawnRate = 1.5f;
    private float currentSpawnRate;
    private bool isSpawning = false;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] topSpawnPoints;
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;

    private Transform lastSpawnPoint;
    [SerializeField] private float topProbability = 0.7f; 

    void Awake() { Instance = this; }

    void OnEnable()
    {
        EventManager.OnPlayerHit += HandleHit;
        EventManager.OnWaveChanged += HandleWaveChanged;
    }

    void OnDisable()
    {
        EventManager.OnPlayerHit -= HandleHit;
        EventManager.OnWaveChanged -= HandleWaveChanged;
    }

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        EventManager.TriggerWaveChanged(currentWave);
    }

    void Update()
    {
        if (isGameOver) return;

        timeSurvived += Time.deltaTime;
        waveTimer += Time.deltaTime;

        if (waveTimer >= (timePerWave - 5f) && isSpawning)
        {
            isSpawning = false;
            CancelInvoke(nameof(SpawnLogic));
        }

        if (waveTimer >= timePerWave)
        {
            currentWave++;
            waveTimer = 0;
            EventManager.TriggerWaveChanged(currentWave);
        }
    }

    void HandleWaveChanged(int wave)
    {
        if (wave > 1)
        {
            currentSpawnRate = Mathf.Max(0.25f, currentSpawnRate * 0.85f);
            topProbability = Mathf.Max(0.4f, topProbability - 0.05f);
        }

        StartCoroutine(WaveTransitionRoutine());
    }

    IEnumerator WaveTransitionRoutine()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnLogic));

        yield return new WaitForSeconds(2.5f);

        if (!isGameOver)
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnLogic), 0f, currentSpawnRate);
        }
    }

    void SpawnLogic()
    {
        if (isGameOver || !isSpawning) return;

        Transform selectedPoint = null;
        Quaternion rotation = Quaternion.identity;

        float rand = Random.value;

        if (rand < topProbability)
        {
            selectedPoint = GetRandomPoint(topSpawnPoints);
            rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rand < topProbability + (1f - topProbability) / 2f) 
        {
            selectedPoint = GetRandomPoint(leftSpawnPoints);
            rotation = Quaternion.Euler(0, 0, -45);
        }
        else 
        {
            selectedPoint = GetRandomPoint(rightSpawnPoints);
            rotation = Quaternion.Euler(0, 0, 45);
        }

        if (selectedPoint != null)
        {
            GameObject obj = ObjectPool.Instance.GetObject();
            obj.transform.position = selectedPoint.position;
            obj.transform.rotation = rotation;
        }
    }

    Transform GetRandomPoint(Transform[] points)
    {
        if (points == null || points.Length == 0) return null;
        if (points.Length == 1) return points[0];

        Transform p;
        int attempts = 0;
        do
        {
            p = points[Random.Range(0, points.Length)];
            attempts++;
        } while (p == lastSpawnPoint && attempts < 10);

        lastSpawnPoint = p;
        return p;
    }

    void HandleHit()
    {
        lives--;
        if (lives <= 0 && !isGameOver)
        {
            isGameOver = true;
            isSpawning = false;
            Time.timeScale = 0;
            EventManager.TriggerGameOver();
            CancelInvoke(nameof(SpawnLogic));
        }
    }
}