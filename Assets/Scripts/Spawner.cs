using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Spawner : MonoBehaviour
{
    public LevelConfiguration configuration;
    private Collider spawnArea;
    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;

    [Range(0f, 1f)] public float bombChance = 0.05f;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifetime = 5f;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        foreach (var fruitPrefab in fruitPrefabs)
        {
            fruitPrefab.GetComponent<Fruit>().isIndicatorHapticOn = configuration.enableSpawnerIndicatorHaptic;
            fruitPrefab.GetComponent<Fruit>().isIndicatorAudioOn = configuration.enableSpawnerIndicatorAudio;
            fruitPrefab.GetComponent<Fruit>().isFeedbackHapticOn = configuration.enableSpawnerFeedbackHaptic;
            fruitPrefab.GetComponent<Fruit>().isFeedbackAudioOn = configuration.enableSpawnerFeedbackAudio;
            fruitPrefab.GetComponent<Fruit>().isFeedbackVisualOn = configuration.enableSpawnerFeedbackVisual;
        }
        bombPrefab.GetComponent<Bomb>().isIndicatorAudioOn = configuration.enableSpawnerIndicatorAudio;
        bombPrefab.GetComponent<Bomb>().isIndicatorParticleOn = configuration.enableSpawnerIndicatorVisual;
        bombPrefab.GetComponent<Bomb>().isIndicatorHapticOn = configuration.enableSpawnerIndicatorHaptic;
        bombPrefab.GetComponent<Bomb>().isFeedbackVisualOn = configuration.enableSpawnerFeedbackVisual;
        bombPrefab.GetComponent<Bomb>().isFeedbackAudioOn = configuration.enableSpawnerFeedbackAudio;
        bombPrefab.GetComponent<Bomb>().isFeedbackHapticOn = configuration.enableSpawnerFeedbackHaptic;

    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);

        while (enabled)
        {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            GameLogger.Instance.LogGenerateFruit();

            if (Random.value < bombChance) {
                prefab = bombPrefab;
                GameLogger.Instance.LogGenerateBomb();
            }

            Vector3 position = new Vector3
            {
                x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            };

            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifetime);

            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

}
