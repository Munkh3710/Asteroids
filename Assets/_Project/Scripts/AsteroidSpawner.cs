using UnityEngine;

namespace AsteroidsClone
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private Asteroid _asteroidPrefab;
        [SerializeField] private float _trajectoryVariance = 15.0f;
        [SerializeField] private float _spawnRate = 2.0f;
        [SerializeField] private float _spawnDistance = 15.0f;
        [SerializeField] private int _spawnAmount = 1;
        [SerializeField] private float _minSpawnSize = 0.5f;
        [SerializeField] private float _maxSpawnSize = 1.5f;
        [SerializeField] private GameFlow _gameFlow;

        private float _spawnTimer;

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                Spawn();
                _spawnTimer = _spawnRate;
            }
        }

        private void Spawn()
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                Vector3 spawnDirection = Random.insideUnitCircle.normalized * _spawnDistance;
                Vector3 spawnPoint = transform.position + spawnDirection;

                float variance = Random.Range(-_trajectoryVariance, _trajectoryVariance);
                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

                Asteroid asteroid = Instantiate(_asteroidPrefab, spawnPoint, rotation);
                asteroid.SetSize(Random.Range(_minSpawnSize, _maxSpawnSize));
                asteroid.SetTrajectory(rotation * -spawnDirection);

                asteroid.OnDestroyed += HandleAsteroidDestroyed;
            }
        }

        private void HandleAsteroidDestroyed(Asteroid asteroid)
        {
            asteroid.OnDestroyed -= HandleAsteroidDestroyed;

            if (_gameFlow != null)
            {
                _gameFlow.RegisterAsteroidKill(asteroid.Size, asteroid.transform.position);
            }
        }
    }
}
