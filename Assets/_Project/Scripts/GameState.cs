using UnityEngine;
using System;

namespace AsteroidsClone
{
    public class GameState : MonoBehaviour
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnLivesChanged;
        public event Action<int> OnGameOver;
        public event Action OnPlayerRespawned;

        [field: SerializeField] public int Score { get; private set; } = 0;
        [field: SerializeField] public int Lives { get; private set; } = 3;

        public void AddScore(int amount)
        {
            Score += amount;
            
            if (OnScoreChanged != null)
            {
                OnScoreChanged(Score);
            }
        }

        public void RegisterPlayerDeath()
        {
            Lives--;
            
            if (OnLivesChanged != null)
            {
                OnLivesChanged(Lives);
            }

            if (Lives <= 0)
            {
                if (OnGameOver != null)
                {
                    OnGameOver(Score);
                }
                ResetState();
            }
        }

        public void ResetState()
        {
            Score = 0;
            Lives = 3;
            
            if (OnScoreChanged != null)
            {
                OnScoreChanged(Score);
            }
            
            if (OnLivesChanged != null)
            {
                OnLivesChanged(Lives);
            }
        }

        public void NotifyPlayerRespawned()
        {
            if (OnPlayerRespawned != null)
            {
                OnPlayerRespawned();
            }
        }
    }
}

