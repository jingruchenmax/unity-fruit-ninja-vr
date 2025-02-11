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
    DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
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
        // Define the Unix epoch
        unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        // Create a timestamp for the log file
        // Get the current time
        DateTime now = DateTime.UtcNow;

        // Calculate the Unix timestamp
        long unixTimestamp = (long)(now - unixEpoch).TotalSeconds;
        // Generate the log file path with the custom name and timestamp
        logFilePath = $"{Application.persistentDataPath}/{customFileName}_{unixTimestamp}.csv";
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

    public void LogStrikeGained(bool isLeft)
    {
        if (isLeft)
        {
            LogEvent("Strike Left");
        }
        else
        {
            LogEvent("Strike Right");
        }
    }

    public void LogComboGained()
    {
        LogEvent("Combo");
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
