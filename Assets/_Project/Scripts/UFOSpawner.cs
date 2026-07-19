using UnityEngine;

namespace AsteroidsClone
{
    public class UFOSpawner : MonoBehaviour
    {
        [SerializeField] private UFO _ufoPrefab;
        [SerializeField] private float _spawnRate = 10.0f;
        [SerializeField] private float _spawnDistance = 12.0f;
        [SerializeField] private Player _player;
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
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = spawnDirection * _spawnDistance;

            UFO ufo = Instantiate(_ufoPrefab, spawnPoint, Quaternion.identity);
            ufo.Initialize(_player, _gameFlow);
        }
    }
}
