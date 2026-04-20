using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Game Over Screen")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalStatsText;
    [SerializeField] private Button restartButton;

    void OnEnable()
    {
        EventManager.OnGameOver += ShowGameOver;
        EventManager.OnWaveChanged += UpdateWaveUI;
    }

    void OnDisable()
    {
        EventManager.OnGameOver -= ShowGameOver;
        EventManager.OnWaveChanged -= UpdateWaveUI;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (LevelManager.Instance != null && !LevelManager.Instance.isGameOver)
        {
            float t = LevelManager.Instance.timeSurvived;
            timeText.text = $"Time: {t:F1}s";
            livesText.text = $"Lives: {LevelManager.Instance.lives}";
        }
    }

    void UpdateWaveUI(int wave)
    {
        StopAllCoroutines(); 
        StartCoroutine(WaveMessageRoutine(wave));
    }

    IEnumerator WaveMessageRoutine(int wave)
    {
        waveText.text = $"WAVE {wave}";
        waveText.gameObject.SetActive(true); 
        
        yield return new WaitForSeconds(3f); 
        
        waveText.gameObject.SetActive(false); 
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        float t = LevelManager.Instance.timeSurvived;
        finalStatsText.text = $"You survived {t:F2}s and reached wave {LevelManager.Instance.currentWave}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}