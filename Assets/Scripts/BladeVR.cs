using UnityEngine;
using Oculus.Haptics;
using TMPro;

public class BladeVR : MonoBehaviour
{
    public LevelConfiguration configuration;
    public float sliceForce = 5f; // Force threshold for slicing
    public float minSliceVelocity = 0.01f; // Minimum velocity for a valid strike
    public Controller controller;
    public MeleeWeaponTrail trail;
    bool isAudioOn;
    bool isVisualOn;
    bool isHapticOn;

    AudioSource audioSource;
    HapticClipPlayer player;

    public AudioClip[] clips;
    public HapticClip[] hapticClips;

    private Collider sliceCollider;

    private Vector3 direction;
    public Vector3 Direction => direction;

    private bool slicing;
    public bool Slicing => slicing;

    Vector3 oldPosition;

    public float strikeThreshold = 2f; // Threshold velocity to detect a strike
    private bool strikeDetected = false;

    private void Awake()
    {
        isVisualOn = configuration.enableSwordVisual;
        isHapticOn = configuration.enableSwordHaptic;
        isAudioOn = configuration.enableSwordAudio;
        audioSource = GetComponent<AudioSource>();
        player = new HapticClipPlayer(hapticClips[0]);
        sliceCollider = GetComponent<Collider>();
        StartSlice();
        if (isVisualOn)
        {
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
        }
    }

    private void Update()
    {
        ContinueSlice();

        // Check if a strike is detected
        if (CheckStrike())
        {
            OnStrikeDetected();
        }
    }

    private void StartSlice()
    {
        sliceCollider.enabled = true;
        oldPosition = transform.position;
    }

    private void ContinueSlice()
    {
        direction = transform.position - oldPosition;
        oldPosition = transform.position;
    }

    private bool CheckStrike()
    {
        // Calculate the velocity of the blade
        float velocity = direction.magnitude / Time.deltaTime;

        // Determine if the velocity exceeds the strike threshold
        if (velocity > strikeThreshold)
        {
            if (!strikeDetected)
            {
                strikeDetected = true;
                return true;
            }
        }
        else
        {
            strikeDetected = false;
        }

        return false;
    }

    private void OnStrikeDetected()
    {
        Debug.Log("Strike detected!");
        GameLogger.Instance.LogStrikeGained(controller == Controller.Left);
        int index = Random.Range(0, clips.Length);
        // Play audio if enabled
        if (isAudioOn && audioSource != null && clips.Length > 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clips[index]);
        }

        // Play haptics if enabled
        if (isHapticOn && player != null)
        {
            player.clip = hapticClips[index];
            player.Play(controller);
        }

        // Trigger visual effects (if any) here

    }
}
