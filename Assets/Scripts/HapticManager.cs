using Oculus.Haptics;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
    // Assign both clips in the Unity editor
    public HapticClip defaultClip;
    private HapticClipPlayer player;
    public Controller controller;
    void Awake()
    {
        player = new HapticClipPlayer(defaultClip);
    }

    public void PlayHaptic(HapticClip clip)
    {
        player.clip = clip;
        player.Play(controller);
    }

    public void PlayHaptic(Controller setController, HapticClip clip)
    {
        player.clip = clip;
        player.Play(setController);
    }

    public void StopHaptics()
    {
        player.Stop();
    }
}