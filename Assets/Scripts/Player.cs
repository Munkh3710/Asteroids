using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Движение игрока")]
    private Rigidbody2D _rigidbody;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private bool _thrusting;
    private float _turnDirection;

    [Header("Настройки снаряда")]
    public Bullet bulletPrefab;
    public float bulletCooldown = 0.5f;
    private float _nextFireTime = 0f;
    
    [Header("Настройки лазера")]
    public LaserBeam laserPrefab;
    public GameObject laserSpawn;
    public float laserLength = 20f; 
    public int maxLaserCharges = 3;
    public int currentLaserCharges = 3;
    public float laserRechargeTime = 5.0f;
    public float nextLaserRechargeTime = 0f;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _thrusting = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
        } else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            _turnDirection = -1.0f;
        } else
        {
            _turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + bulletCooldown;
            }
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            ShootLaser();
        }

        if (currentLaserCharges < maxLaserCharges && Time.time >= nextLaserRechargeTime)
        {
            currentLaserCharges++;
            nextLaserRechargeTime = Time.time + laserRechargeTime;
        }
    }

    private void FixedUpdate()
    {
        if(_thrusting)
        {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }

        if(_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * this.turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate (this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("UFO"))
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            FindAnyObjectByType<GameManager>().PlayerDied();
        }
    }

    private void ShootLaser()
    {
        if (currentLaserCharges > 0)
        {
            // Сдвигаем лазер вперед на половину длины
            Vector3 spawnPos = laserSpawn.transform.position + laserSpawn.transform.up * (laserLength / 2f);
            Instantiate(laserPrefab, spawnPos, transform.rotation);
            currentLaserCharges--;
        }
    }
}
