using UnityEngine;

namespace AsteroidsClone
{
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 0.2f;

        private GameFlow _gameFlow;

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }

        public void Initialize(GameFlow gameFlow)
        {
            _gameFlow = gameFlow;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Asteroid>(out Asteroid asteroid))
            {
                asteroid.TakeDamage();
            }
            else if (collision.TryGetComponent<UFO>(out UFO ufo))
            {
                if (_gameFlow != null)
                {
                    _gameFlow.RegisterUfoKill(ufo.transform.position);
                }
                
                Destroy(collision.gameObject);
            }
        }
    }
}
