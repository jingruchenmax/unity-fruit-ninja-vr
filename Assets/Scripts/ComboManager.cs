using UnityEngine;
using Oculus.Haptics;

public class ComboManager : MonoBehaviour
{
    public LevelConfiguration configuration;
    public static ComboManager Instance { get; private set; }

    [Header("Combo Configuration")]
    public float comboDuration = 3f; // Time window for combos
    public AudioClip[] comboAudioClips; // Audio clips for each combo level
    public HapticClip[] comboHapticClips; // Haptic clips for each combo level
    public ParticleSystem[] comboParticleSystems; // Particle effects for each combo level
    bool isVisualOn;
    bool isAudioOn;
    bool isHapticOn;

    private int comboCount; // Current combo streak
    private float comboTimer; // Timer for combo expiration
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
        isVisualOn = configuration.enableComboVisual;
        isAudioOn = configuration.enableComboAudio;
        isHapticOn = configuration.enableComboHaptic;
    }

    private void Update()
    {
        // Expire combo if no activity within comboDuration
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    public void RegisterStrike()
    {
        comboCount++;
        comboTimer = comboDuration;

        // Play effects based on the current combo count
        PlayComboEffects(comboCount);
    }

    public void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0;

        // Stop all active particle effects
        foreach (ParticleSystem ps in comboParticleSystems)
        {
            if (ps.isPlaying) ps.Stop();
        }
    }

    private void PlayComboEffects(int comboLevel)
    {
        if (comboLevel <= 3) return;
        comboLevel = comboLevel - 3;
        // Clamp comboLevel to avoid array out-of-bounds errors
        int effectIndex = Mathf.Clamp(comboLevel - 1, 0, Mathf.Min(comboAudioClips.Length, comboHapticClips.Length, comboParticleSystems.Length) - 1);

        // Play audio effect
        if (isAudioOn && comboAudioClips.Length > effectIndex && comboAudioClips[effectIndex] != null)
        {
            audioSource.PlayOneShot(comboAudioClips[effectIndex]);
        }

        // Play haptic feedback
        if (isHapticOn && comboHapticClips.Length > effectIndex && comboHapticClips[effectIndex] != null)
        {
            HapticClipPlayer hapticPlayer = new HapticClipPlayer(comboHapticClips[effectIndex]);
            hapticPlayer.Play(Controller.Both); // Play haptics on both controllers
        }

        // Enable particle effect
        if (isVisualOn && comboParticleSystems.Length > effectIndex && comboParticleSystems[effectIndex] != null)
        {
            ParticleSystem particles = comboParticleSystems[effectIndex];
            if (!particles.isPlaying) particles.Play();
        }
        GameLogger.Instance.LogComboGained();
    }
}
