using UnityEngine;
using System.Collections;

namespace AsteroidsClone
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;
        [SerializeField] private Player _player;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private float _respawnDelay = 3.0f;
        [SerializeField] private float _invulnerabilityTime = 3.0f;

        private void OnEnable()
        {
            if (_player != null)
            {
                _player.OnDied += HandlePlayerDied;
            }
        }

        private void OnDisable()
        {
            if (_player != null)
            {
                _player.OnDied -= HandlePlayerDied;
            }
        }

        private void HandlePlayerDied()
        {
            if (_player != null && _explosion != null)
            {
                _explosion.transform.position = _player.transform.position;
                _explosion.Play();
            }
            
            if (_gameState != null)
            {
                _gameState.RegisterPlayerDeath();
                
                if (_gameState.Lives > 0)
                {
                    StartRespawnSequence(_player);
                }
            }
        }

        public void RegisterAsteroidKill(float size, Vector3 position)
        {
            if (_explosion != null)
            {
                _explosion.transform.position = position;
                _explosion.Play();
            }

            if (_gameState != null)
            {
                if (size < 0.75f) _gameState.AddScore(100);
                else if (size < 1.2f) _gameState.AddScore(50);
                else _gameState.AddScore(25);
            }
        }

        public void RegisterUfoKill(Vector3 position)
        {
            if (_explosion != null)
            {
                _explosion.transform.position = position;
                _explosion.Play();
            }

            if (_gameState != null)
            {
                _gameState.AddScore(200);
            }
        }

        public void StartRespawnSequence(Player player)
        {
            if (player != null)
            {
                StartCoroutine(RespawnCoroutine(player));
            }
        }

        private IEnumerator RespawnCoroutine(Player player)
        {
            yield return new WaitForSeconds(_respawnDelay);
            
            if (player != null)
            {
                player.Respawn(Vector3.zero, _invulnerabilityTime);
            }
            
            if (_gameState != null)
            {
                _gameState.NotifyPlayerRespawned();
            }
        }
    }
}
