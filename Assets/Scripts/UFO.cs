using UnityEngine;

public class UFO : MonoBehaviour
{
    public float speed = 3.0f;
    public float maxLifetime = 15.0f;
    public float freezeTimeAfterPlayerDeath = 4.0f;
    
    private Rigidbody2D _rigidbody;
    private Transform _playerTransform;
    private Collider2D _playerCollider; 
    private bool _isFrozen = false;
    private float _freezeEndTime = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            _playerTransform = player.transform;
            _playerCollider = player.GetComponent<Collider2D>();
        }
    }

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        // Проверяем, жив ли игрок (включен ли его коллайдер)
        bool isPlayerAlive = _playerCollider != null && _playerCollider.enabled;

        if (!isPlayerAlive && !_isFrozen)
        {
            // Если игрок умер запускаем таймер замирания
            _isFrozen = true;
            _freezeEndTime = Time.time + freezeTimeAfterPlayerDeath;
            _rigidbody.linearVelocity = Vector2.zero;
        }

        if (_isFrozen)
        {
            // НЛО замерло - проверяем, прошло ли время замирания
            if (Time.time >= _freezeEndTime)
            {
                _isFrozen = false;
            }
            else
            {
                // Все еще замерло - стоим на месте
                _rigidbody.linearVelocity = Vector2.zero;
                CheckScreenWrap();
                return;
            }
        }

        if (isPlayerAlive && !_isFrozen)
        {
            Vector2 direction = (_playerTransform.position - transform.position).normalized;
            _rigidbody.linearVelocity = direction * speed;
        }

        CheckScreenWrap(); 
    }

    private void CheckScreenWrap()
    {
        float screenRight = 9.4f, screenLeft = -9.4f, screenTop = 5.5f, screenBottom = -5.5f;
        Vector3 pos = transform.position;

        if (pos.x > screenRight) pos.x = screenLeft;
        else if (pos.x < screenLeft) pos.x = screenRight;
        if (pos.y > screenTop) pos.y = screenBottom;
        else if (pos.y < screenBottom) pos.y = screenTop;

        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            FindAnyObjectByType<GameManager>().UFODestroyed(this);
            Destroy(gameObject);
        }
    }
    
}
