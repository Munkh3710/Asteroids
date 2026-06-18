using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    public UFO ufoPrefab;
    public float spawnRate = 10.0f;
    public float spawnDistance = 12.0f;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        // Спавним в случайной точке на окружности вокруг центра экрана
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPoint = spawnDirection * spawnDistance;

        UFO ufo = Instantiate(ufoPrefab, spawnPoint, Quaternion.identity);
    }
}
