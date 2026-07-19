using UnityEngine;
using System;
using System.Collections;

namespace AsteroidsClone
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
    public class Player : MonoBehaviour
    {
        public event Action OnDied;

        [Header("Movement")]
        [SerializeField] private float _thrustSpeed = 1.0f;
        [SerializeField] private float _turnSpeed = 1.0f;

        [Header("Shooting")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _bulletCooldown = 0.5f;

        [Header("Laser")]
        [SerializeField] private LaserBeam _laserPrefab;
        [SerializeField] private Transform _laserSpawnPoint;
        [SerializeField] private float _laserLength = 20f;
        [field: SerializeField] public int MaxLaserCharges { get; private set; } = 3;
        [SerializeField] private float _laserRechargeTime = 5.0f;

        [Header("Dependencies")]
        [SerializeField] private GameFlow _gameFlow;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        private bool _isThrusting;
        private float _turnDirection;
        private float _nextFireTime;

        public int CurrentLaserCharges { get; private set; }
        public float NextLaserRechargeTime { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            ResetLaser();
        }

        private void Update()
        {
            _isThrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _turnDirection = 1.0f;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _turnDirection = -1.0f;
            }
            else
            {
                _turnDirection = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (Time.time >= _nextFireTime)
                {
                    Shoot();
                    _nextFireTime = Time.time + _bulletCooldown;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                ShootLaser();
            }

            if (CurrentLaserCharges < MaxLaserCharges && Time.time >= NextLaserRechargeTime)
            {
                CurrentLaserCharges++;
                NextLaserRechargeTime = Time.time + _laserRechargeTime;
            }
        }

        private void FixedUpdate()
        {
            if (_isThrusting)
            {
                _rigidbody.AddForce(transform.up * _thrustSpeed);
            }

            if (_turnDirection != 0.0f)
            {
                _rigidbody.AddTorque(_turnDirection * _turnSpeed);
            }
        }

        public void Hide()
        {
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
        }

        public void Respawn(Vector3 position, float invulnerabilityTime)
        {
            transform.position = position;
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");

            StartCoroutine(EnableCollisionsCoroutine(invulnerabilityTime));
        }

        public void ResetLaser()
        {
            CurrentLaserCharges = MaxLaserCharges;
            NextLaserRechargeTime = 0f;
        }

        private void Shoot()
        {
            if (_bulletPrefab != null)
            {
                Bullet bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
                bullet.Project(transform.up);
            }
        }

        private void ShootLaser()
        {
            if (CurrentLaserCharges > 0 && _laserPrefab != null && _laserSpawnPoint != null)
            {
                Vector3 spawnPos = _laserSpawnPoint.position + _laserSpawnPoint.up * (_laserLength / 2f);
                LaserBeam laser = Instantiate(_laserPrefab, spawnPos, transform.rotation);

                if (_gameFlow != null)
                {
                    laser.Initialize(_gameFlow);
                }

                CurrentLaserCharges--;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Asteroid>(out _) ||
                collision.gameObject.TryGetComponent<UFO>(out _))
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = 0.0f;
                Hide();

                if (OnDied != null)
                {
                    OnDied();
                }
            }
        }

        private IEnumerator EnableCollisionsCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
