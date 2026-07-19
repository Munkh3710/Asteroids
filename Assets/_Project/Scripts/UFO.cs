using UnityEngine;

namespace AsteroidsClone
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UFO : MonoBehaviour
    {
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private float _maxLifetime = 15.0f;
        [SerializeField] private float _freezeTimeAfterPlayerDeath = 4.0f;

        private Rigidbody2D _rigidbody;
        private Player _player;
        private GameFlow _gameFlow;
        private Collider2D _playerCollider;
        private bool _isFrozen;
        private float _freezeEndTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void Start()
        {
            Destroy(gameObject, _maxLifetime);
        }

        public void Initialize(Player player, GameFlow gameFlow)
        {
            _player = player;
            _gameFlow = gameFlow;

            if (_player != null)
            {
                _playerCollider = _player.GetComponent<Collider2D>();
            }
        }

        private void Update()
        {
            bool isPlayerAlive = _playerCollider != null && _playerCollider.enabled;

            if (!isPlayerAlive && !_isFrozen)
            {
                _isFrozen = true;
                _freezeEndTime = Time.time + _freezeTimeAfterPlayerDeath;
                _rigidbody.linearVelocity = Vector2.zero;
            }

            if (_isFrozen)
            {
                if (Time.time >= _freezeEndTime)
                {
                    _isFrozen = false;
                }
                else
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    return;
                }
            }

            if (isPlayerAlive && !_isFrozen)
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _rigidbody.linearVelocity = direction * _speed;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Bullet>(out _))
            {
                if (_gameFlow != null)
                {
                    _gameFlow.RegisterUfoKill(transform.position);
                }
                Destroy(gameObject);
            }
        }
    }
}
