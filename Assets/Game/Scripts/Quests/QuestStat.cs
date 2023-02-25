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
            //Debug.Log($"{title} has been incremented to {count}");
        }
        
        public void Reset() => count = 0;

    }
}