using UnityEngine;
using System.IO; // For file operations
using System.Text; // For string formatting
using System;

public class GameLogger : MonoBehaviour
{
    public LevelConfiguration configuration;
    private string logFilePath;
    string customFileName = "";
    public static GameLogger Instance { get; private set; }

    private void Awake()
    {
        customFileName = configuration.name;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Make persistent across scenes
    }
    void Start()
    {
        // Create a timestamp for the log file
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

        // Generate the log file path with the custom name and timestamp
        logFilePath = $"{Application.persistentDataPath}/{customFileName}_{timestamp}.csv";
        Debug.Log(logFilePath);
        // Create and write the CSV header
        using (StreamWriter writer = new StreamWriter(logFilePath, false, Encoding.UTF8))
        {
            writer.WriteLine("Timestamp,Event");
        }
    }
    public void LogTimerStart()
    {
        LogEvent("Timer Start");
    }
    public void LogGameStart()
    {
        LogEvent("Game Start");
    }

    public void LogGenerateFruit()
    {
        LogEvent("Fruit Generated");
    }

    public void LogGenerateBomb()
    {
        LogEvent("Bomb Generated");
    }

    public void LogScoreGained()
    {
        LogEvent("Score Gained");
    }

    public void LogPlayerDeath()
    {
        LogEvent("Player Dead");
    }

    public void LogTimerEnd()
    {
        LogEvent("Timer End");
    }

    private void LogEvent(string gameEvent)
    {
        // Get the current time
        DateTime now = DateTime.UtcNow;

        // Define the Unix epoch
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Calculate the Unix timestamp
        long unixTimestamp = (long)(now - unixEpoch).TotalSeconds;
        string logEntry = $"{unixTimestamp},{gameEvent}";

        // Write the log entry to the CSV file
        using (StreamWriter writer = new StreamWriter(logFilePath, true, Encoding.UTF8))
        {
            writer.WriteLine(logEntry);
        }
    }
}
