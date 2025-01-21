using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerVR : MonoBehaviour
{
    public static GameManagerVR Instance { get; private set; }
    public LevelConfiguration configuration;
    [SerializeField] private BladeVR bladeLeft;
    [SerializeField] private BladeVR bladeRight;
    [SerializeField] private Spawner spawner;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro deathText;
    [SerializeField] private TextMeshPro timerText;

    [SerializeField]
    private float timeRemaining = 120f; // 2 minutes in seconds
    [SerializeField] bool timerRunning = true;
    bool isGameStarted = false;
    private int score=0;
    private int death=0;
    bool isdying = false;

    public int Score => score;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        timeRemaining = configuration.timer;
        timerRunning = configuration.isTimerOn;
    }

    public void StartGame()
    {
        if(!isGameStarted)
        {
            isGameStarted = true;
            GameLogger.Instance.LogTimerStart();
            NewGame();
            if (timerRunning)
            {
                StartCoroutine(StartTimer());
            }
        }
    }

    private IEnumerator StartTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= 1f;
            UpdateTimerDisplay(timeRemaining);
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }

        TimerEnd();
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void NewGame()
    {
        isdying = false;
        GameLogger.Instance.LogGameStart();
        Time.timeScale = 1f;

        ClearScene();

        bladeLeft.enabled = true;
        bladeRight.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();

        // Reset combos
        ComboManager.Instance?.ResetCombo();
    }

    private void ClearScene()
    {
        foreach (Fruit fruit in FindObjectsOfType<Fruit>())
        {
            Destroy(fruit.gameObject);
        }

        foreach (Bomb bomb in FindObjectsOfType<Bomb>())
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        GameLogger.Instance.LogScoreGained();
        score += points;
        scoreText.text = score.ToString();

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            PlayerPrefs.SetFloat("hiscore", score);
        }

        // Register a successful score for combo tracking
        ComboManager.Instance?.RegisterStrike();
    }

    public void Explode()
    {
        if (!isdying)
        {
            isdying = true;
            bladeLeft.enabled = false;
            bladeRight.enabled = false;
            spawner.enabled = false;

            // Reset combo when player dies
            ComboManager.Instance?.ResetCombo();

            GameLogger.Instance.LogPlayerDeath();
            StartCoroutine(ExplodeSequence());
        }
    }

    void TimerEnd()
    {
        bladeLeft.enabled = false;
        bladeRight.enabled = false;
        spawner.enabled = false;
        GameLogger.Instance.LogTimerEnd();
        ClearScene();
    }

    private IEnumerator ExplodeSequence()
    {
        death++;
        deathText.text = death.ToString();
        yield return new WaitForSecondsRealtime(1f);
        NewGame();
    }
}
