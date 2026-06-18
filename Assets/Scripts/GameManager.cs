using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;
    public int score = 0;
    public UIManager uiManager;
    private int _lastScore; 

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if(asteroid.size < 0.75f)
        {
            this.score += 100;
        } else if (asteroid.size < 1.2f)
        {
            this.score += 50;
        } else
        {
            this.score += 25;
        }
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;
        _lastScore = this.score;

        if(this.lives <= 0)
            GameOver(); 
        else
            Invoke(nameof(Respawn), this.respawnTime);    
    }

    public void UFODestroyed(UFO ufo)
    {
        this.explosion.transform.position = ufo.transform.position;
        this.explosion.Play();
        this.score += 200;
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        
        // Включаем спрайт и коллайдер обратно
        this.player.GetComponent<SpriteRenderer>().enabled = true;
        this.player.GetComponent<Collider2D>().enabled = true;
        
        // Временно делаем невидимым для столкновений
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        // Показываем экран Game Over
        if (uiManager != null)
        {
            uiManager.ShowGameOver(this.score); // Передаем счет ДО сброса
        }

        this.lives = 3;
        this.score = 0;

        // Сбрасываем заряды лазера
        this.player.currentLaserCharges = this.player.maxLaserCharges;
        this.player.nextLaserRechargeTime = 0f;

    }
}
