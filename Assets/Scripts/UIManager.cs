using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Игровые показатели")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    
    [Header("Показатели корабля")]
    public TextMeshProUGUI coordsText;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI laserChargesText;
    public TextMeshProUGUI laserCooldownText;
    
    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    
    [Header("Ссылки")]
    public Player player;
    public GameManager gameManager;

    private void Start()
    {
        // Подписываем кнопку Restart на перезапуск игры
        restartButton.onClick.AddListener(RestartGame);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // Обновляем счет и жизни
        scoreText.text = $"Score: {gameManager.score}";
        livesText.text = $"Lives: {gameManager.lives}";

        // Если игрок мертв — не обновляем показатели корабля
        if (player == null || !player.GetComponent<Collider2D>().enabled)
        {
            coordsText.text = "Coords: X:-- Y:--";
            angleText.text = "Angle: --";
            speedText.text = "Speed: --";
            laserChargesText.text = "Laser: --/--";
            laserCooldownText.text = "Recharge: --s";
            return;
        }

        // 1. Координаты
        Vector3 pos = player.transform.position;
        coordsText.text = $"Coords: X:{pos.x:F1} Y:{pos.y:F1}";

        // 2. Угол поворота
        float angle = player.transform.eulerAngles.z;
        angleText.text = $"Angle: {angle:F1}°";

        // 3. Мгновенная скорость
        float speed = player.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        speedText.text = $"Speed: {speed:F2}";

        // 4. Заряды лазера
        laserChargesText.text = $"Laser: {player.currentLaserCharges}/{player.maxLaserCharges}";

        // 5. Время отката лазера
        float timeToRecharge = 0f;
        if (player.currentLaserCharges < player.maxLaserCharges)
        {
            timeToRecharge = Mathf.Max(0, player.nextLaserRechargeTime - Time.time);
        }
        laserCooldownText.text = $"Recharge: {timeToRecharge:F1}s";
    }

    // Метод вызывается из GameManager при Game Over
    public void ShowGameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";
    }

    private void RestartGame()
    {
        // Перезапускаем сцену
        UnityEngine.SceneManagement.SceneManager.LoadScene( UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
