using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float lifeTime = 0.2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            if (asteroid != null) 
            {
                asteroid.TakeDamage();
            }
        }
        else if (collision.CompareTag("UFO"))
        {
            UFO ufo = collision.GetComponent<UFO>();
            
            if (ufo != null)
            {
                FindAnyObjectByType<GameManager>().UFODestroyed(ufo);
            }
            
            Destroy(collision.gameObject);
        }
    }
}
