using UnityEngine;
using System;

namespace AsteroidsClone
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour
    {
        public event Action<Asteroid> OnDestroyed;

        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _size = 1.0f;
        [SerializeField] private float _minSize = 0.5f;
        [SerializeField] private float _maxSize = 1.5f;
        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private float _maxLifetime = 30.0f;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;

        public float Size => _size;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _spriteRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Length)];
            transform.eulerAngles = new Vector3(0.0f, 0.0f, UnityEngine.Random.value * 360.0f);
            transform.localScale = Vector3.one * _size;
            _rigidbody.mass = _size * 2.0f;
        }

        public void SetSize(float size)
        {
            _size = size;
        }

        public void SetTrajectory(Vector2 direction)
        {
            _rigidbody.AddForce(direction * _speed);
            Destroy(gameObject, _maxLifetime);
        }

        public void TakeDamage()
        {
            if ((_size * 0.5f) >= _minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            if (OnDestroyed != null)
            {
                OnDestroyed(this);
            }
            
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Bullet>(out _))
            {
                TakeDamage();
            }
        }

        private void CreateSplit()
        {
            Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Vector3 position = transform.position + randomOffset;
            
            Asteroid half = Instantiate(this, position, transform.rotation);
            half.SetSize(_size * 0.5f);
            
            Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;
            half.SetTrajectory(direction * _speed);
        }
    }
}
