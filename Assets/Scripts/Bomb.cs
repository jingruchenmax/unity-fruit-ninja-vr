using Meta.WitAi;
using Oculus.Haptics;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool isIndicatorHapticOn;
    public bool isIndicatorAudioOn;
    public bool isIndicatorParticleOn;
    public bool isFeedbackHapticOn;
    public bool isFeedbackAudioOn;
    public bool isFeedbackVisualOn;

    public ParticleSystem explosion;
    public ParticleSystem idleExplosion;

    public AudioClip bombIndicatorAudio;
    public AudioClip bombFeedbackAudio;
    public AudioSource audioSource;
    public AudioSource fuseAudio;
    public HapticClip bombIndicatorHaptic;
    public HapticClip bombFeedbackHaptic;
    private HapticClipPlayer player;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = new HapticClipPlayer(bombIndicatorHaptic);
        if (isIndicatorParticleOn)
        {
            idleExplosion.Play();
        }
        if (isIndicatorHapticOn)
        {
            player.Play(Controller.Both);
        }
        if (isIndicatorAudioOn)
        {
            audioSource.PlayOneShot(bombIndicatorAudio);
        }
        StartCoroutine("StopFuse", 2f);
    }
    void StopFuse()
    {
        fuseAudio.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            GameManager.Instance.Explode();
        }

        else if (other.CompareTag("PlayerVR"))
        {
            GetComponent<Collider>().enabled = false;
            if (isFeedbackVisualOn)
            {
                explosion.Play();
            }
            if (isFeedbackAudioOn)
            {
                audioSource.PlayOneShot(bombFeedbackAudio);
            }
            if (isFeedbackHapticOn)
            {
                player.clip = bombIndicatorHaptic;
                player.Play(Controller.Both);
            }

            GameManagerVR.Instance.Explode();
        }
    }

}
