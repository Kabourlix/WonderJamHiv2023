using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
    [CreateAssetMenu(fileName = "Property", menuName = "K/QuestSystem/Property", order = 0)]
    public class QuestStat : ScriptableObject
    {
        [SerializeField] private string title;
        public string Title => title;

        [Tooltip("Add a condition to fulfill before incrementing the stat (if needed, can be null)")]
        [SerializeField] private Condition priorCondition;

        public void Link()
        {
            if(priorCondition.Stat is not null)
            {
                priorCondition.LinkEvent();
                priorCondition.OnConditionCompleted += () => OnPriorConditionCompleted?.Invoke(this);
            }
            else
            {
                OnPriorConditionCompleted?.Invoke(this);
            }
        }

        public int count { get; private set; }
        
        public event Action OnCountChanged;
        public event Action<QuestStat> OnPriorConditionCompleted;

        public bool Increment()
        {
            if(priorCondition.Stat != null && !priorCondition.IsCompleted) return false;
            count++;
            OnCountChanged?.Invoke();
            return true;
        }

        public bool IsPriorConditionCompleted => priorCondition.Stat is null || priorCondition.IsCompleted;
        
        public void Reset() => count = 0;

    }
}