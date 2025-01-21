using UnityEngine;
using System.IO; // For file operations
using System.Text; // For string formatting
using System;

public class ControllerMovementLogger : MonoBehaviour
{
    public LevelConfiguration configuration;
    string customFileName = "";

    private string leftLogFilePath;
    private string rightLogFilePath;
    bool isLogging = false;
    // Define the Unix epoch
    DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private void Awake()
    {
        customFileName = configuration.name;
    }
    public void StartLogging()
    {
        isLogging = true;
    }
    void Start()
    {

        // Calculate the Unix timestamp
        long unixTimestamp = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;

        // Generate file paths for left and right controllers
        leftLogFilePath = $"{Application.persistentDataPath}/{customFileName}_{unixTimestamp}_L.csv";
        rightLogFilePath = $"{Application.persistentDataPath}/{customFileName}_{unixTimestamp}_R.csv";

        // Create CSV files and write headers
        CreateLogFile(leftLogFilePath);
        CreateLogFile(rightLogFilePath);

        Debug.Log($"Left controller log file created at: {leftLogFilePath}");
        Debug.Log($"Right controller log file created at: {rightLogFilePath}");
    }

    void FixedUpdate()
    {
        // Check if logging is active
        if (isLogging)
        {
            LogControllerMovement(OVRInput.Controller.LTouch, leftLogFilePath);
            LogControllerMovement(OVRInput.Controller.RTouch, rightLogFilePath);
        }
    }

    private void CreateLogFile(string filePath)
    {
        // Create the CSV file and write the header
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            writer.WriteLine("Timestamp,PositionX,PositionY,PositionZ,RotationX,RotationY,RotationZ,RotationW");
        }
    }

    private void LogControllerMovement(OVRInput.Controller controller, string filePath)
    {
        // Get the position and rotation of the controller
        Vector3 position = OVRInput.GetLocalControllerPosition(controller);
        Quaternion rotation = OVRInput.GetLocalControllerRotation(controller);

        // Calculate the Unix timestamp
        long unixTimestamp = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;

        // Format the data as a CSV line
        string logEntry = $"{unixTimestamp},{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w}";

        // Write the log entry to the CSV file
        using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
        {
            writer.WriteLine(logEntry);
        }
    }
}
