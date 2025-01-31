using Oculus.Haptics;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public bool isVR = false;
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;

    private ParticleSystem juiceEffect;

    public AudioClip fruitIndicatorAudio;
    public AudioClip[] fruitFeedbackAudios;
    private AudioSource audioSource;

    public HapticClip fruitIndicatorHaptic;
    public HapticClip[] fruitFeedbackHaptics;

    private HapticClipPlayer player;

    public int points = 1;

    public bool isIndicatorHapticOn;
    public bool isIndicatorAudioOn;
    public bool isFeedbackVisualOn;
    public bool isFeedbackHapticOn;
    public bool isFeedbackAudioOn;

    private void Awake()
    {
        player = new HapticClipPlayer(fruitIndicatorHaptic);
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        if (isIndicatorHapticOn)
        {
            player.Play(Controller.Both);
        }
        if (isIndicatorAudioOn)
        {
            audioSource.PlayOneShot(fruitIndicatorAudio);
        }
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        if (isVR)
        {
            GameManagerVR.Instance.IncreaseScore(points);
        }
        else
        {
            GameManager.Instance.IncreaseScore(points);
        }

        // Disable the whole fruit
        fruitCollider.enabled = false;
        whole.SetActive(false);

        if (isFeedbackVisualOn)
        {
            // Enable the sliced fruit
            sliced.SetActive(true);

            // Rotate based on the slice angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

            // Add a force to each slice based on the blade direction
            foreach (Rigidbody slice in slices)
            {
                slice.velocity = fruitRigidbody.velocity;
                slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
            }
            fruitRigidbody.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.Direction, blade.transform.position, blade.sliceForce);
        }

        if (other.CompareTag("PlayerVR"))
        {
            Debug.Log(other.name);
            BladeVR blade = other.GetComponent<BladeVR>();
            int index = Random.Range(0, fruitFeedbackAudios.Length);
            if (isFeedbackVisualOn)
            {
                juiceEffect.Play();
            }
            if (isFeedbackAudioOn)
            {
                audioSource.PlayOneShot(fruitFeedbackAudios[index]);
            }

            if (isFeedbackHapticOn) {
                player.clip = fruitFeedbackHaptics[index];
                player.Play(other.GetComponent<BladeVR>().controller);
            }
            Slice(blade.Direction, blade.transform.position, blade.sliceForce);
        }
    }

}
