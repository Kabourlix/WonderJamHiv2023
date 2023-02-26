using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
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
                if(IsCompleted) Debug.Log($"<color=purple>Condition {stat.Title} is complete.</color>");
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