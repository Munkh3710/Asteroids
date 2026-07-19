using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AsteroidsClone
{
    public class UIManager : MonoBehaviour
    {
        [Header("Game Stats")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _livesText;

        [Header("Ship Stats")]
        [SerializeField] private TextMeshProUGUI _coordsText;
        [SerializeField] private TextMeshProUGUI _angleText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _laserChargesText;
        [SerializeField] private TextMeshProUGUI _laserCooldownText;

        [Header("Game Over")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private Button _restartButton;

        [Header("References")]
        [SerializeField] private Player _player;
        [SerializeField] private GameState _gameState;

        private void Start()
        {
            _restartButton.onClick.AddListener(RestartGame);
            _gameOverPanel.SetActive(false);

            _gameState.OnScoreChanged += UpdateScoreUI;
            _gameState.OnLivesChanged += UpdateLivesUI;
            _gameState.OnGameOver += ShowGameOver;

            UpdateScoreUI(_gameState.Score);
            UpdateLivesUI(_gameState.Lives);
        }

        private void OnDestroy()
        {
            if (_gameState != null)
            {
                _gameState.OnScoreChanged -= UpdateScoreUI;
                _gameState.OnLivesChanged -= UpdateLivesUI;
                _gameState.OnGameOver -= ShowGameOver;
            }

            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(RestartGame);
            }
        }

        private void Update()
        {
            if (_player == null || !_player.GetComponent<Collider2D>().enabled)
            {
                _coordsText.text = "Coords: X:-- Y:--";
                _angleText.text = "Angle: --";
                _speedText.text = "Speed: --";
                _laserChargesText.text = "Laser: --/--";
                _laserCooldownText.text = "Recharge: --s";
                return;
            }

            Vector3 pos = _player.transform.position;
            _coordsText.text = $"Coords: X:{pos.x:F1} Y:{pos.y:F1}";
            _angleText.text = $"Angle: {_player.transform.eulerAngles.z:F1}°";

            float speed = _player.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
            _speedText.text = $"Speed: {speed:F2}";
            _laserChargesText.text = $"Laser: {_player.CurrentLaserCharges}/{_player.MaxLaserCharges}";

            float timeToRecharge = 0f;
            if (_player.CurrentLaserCharges < _player.MaxLaserCharges)
            {
                timeToRecharge = Mathf.Max(0, _player.NextLaserRechargeTime - Time.time);
            }
            _laserCooldownText.text = $"Recharge: {timeToRecharge:F1}s";
        }

        private void UpdateScoreUI(int score)
        {
            _scoreText.text = $"Score: {score}";
        }

        private void UpdateLivesUI(int lives)
        {
            _livesText.text = $"Lives: {lives}";
        }

        private void ShowGameOver(int finalScore)
        {
            _gameOverPanel.SetActive(true);
            _finalScoreText.text = $"Final Score: {finalScore}";
        }

        private void RestartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}
