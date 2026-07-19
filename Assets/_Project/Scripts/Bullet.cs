using UnityEngine;

namespace AsteroidsClone
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 500.0f;
        [SerializeField] private float _maxLifetime = 10.0f;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Project(Vector2 direction)
        {
            _rigidbody.AddForce(direction * _speed);
            Destroy(gameObject, _maxLifetime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }
}
