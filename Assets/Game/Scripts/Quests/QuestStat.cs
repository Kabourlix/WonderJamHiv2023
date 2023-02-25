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
                priorCondition.LinkEvent();
        }

        public int count { get; private set; }
        
        public event Action OnCountChanged; 

        public void Increment()
        {
            if(priorCondition.Stat != null && !priorCondition.IsCompleted) return;
            count++;
            OnCountChanged?.Invoke();
        }
        
        public void Reset() => count = 0;

    }
}