using UnityEngine;

namespace Game.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 5;
        private int _currentHealth;
        public int CurrentHealth => _currentHealth;
        private GameManager _gameManager;
        
        private void Start()
        {
            _currentHealth = maxHealth;
            _gameManager = GameManager.Instance;
        }

        public float CaughtSuspicious()
        {
            _currentHealth--;
            if (_currentHealth <= 0)
            {
                _gameManager.ChangeState(GameState.GameOverState);
                return 0;
            }

            return Mathf.InverseLerp(0, (float) maxHealth, (float)_currentHealth);
        }
    }
}