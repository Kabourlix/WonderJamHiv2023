using UnityEngine;

namespace Game.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        public Transform SpawnPosition => spawnPosition;
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
            return _currentHealth <= 0 ? 0 : Mathf.InverseLerp(0, maxHealth, _currentHealth);
        }
    }
}