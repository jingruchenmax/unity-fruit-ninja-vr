using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "Custom/Configuration")]
public class LevelConfiguration : ScriptableObject
{
    [Header("General Settings")]
    public int timer;
    public bool isTimerOn;

    [Header("Combo Settings")]
    public bool enableComboVisual;
    public bool enableComboAudio;
    public bool enableComboHaptic;

    [Header("Sword Settings")]
    public bool enableSwordVisual;
    public bool enableSwordAudio;
    public bool enableSwordHaptic;

    [Header("Spawner Indicator Settings")]
    public bool enableSpawnerIndicatorVisual;
    public bool enableSpawnerIndicatorAudio;
    public bool enableSpawnerIndicatorHaptic;

    [Header("Spawner Feedback Settings")]
    public bool enableSpawnerFeedbackVisual;
    public bool enableSpawnerFeedbackAudio;
    public bool enableSpawnerFeedbackHaptic;
}
