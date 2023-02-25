using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
    [CreateAssetMenu(fileName = "Condition", menuName = "K/QuestSystem/Condition", order = 0)]
    public class QuestStat : ScriptableObject
    {
        [SerializeField] private string title;
        public string Title => title;

        public int count { get; private set; }
        
        public event Action OnCountChanged; 

        public void Increment()
        {
            count++;
            OnCountChanged?.Invoke();
            Debug.Log($"{title} has been incremented to {count}");
        }
        
        public void Reset() => count = 0;

    }

    [Serializable]
    public class Condition
    {
        [SerializeField] private QuestStat stat;
        public QuestStat Stat => stat;
        [SerializeField] private int target;
        
        public Condition() => _isCompleted = false;
        public event Action OnConditionCompleted; 
        public void LinkEvent()
        {
            IsCompleted = false;
            stat.OnCountChanged += () =>
            {
                IsCompleted = stat.count >= target || IsCompleted;
                Debug.Log($"Condition {stat.Title} has been updated and completed is now {IsCompleted}");
            };
        }

        private bool _isCompleted = false;
        public bool IsCompleted
        {
            get => _isCompleted;
            private set
            {
                if (!_isCompleted && value)
                {
                    _isCompleted = value;
                    OnConditionCompleted?.Invoke();
                }
                else
                    _isCompleted = value;
            }
        }
        
        
    }
}