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

        public void Increment()
        {
            count++;
            Debug.Log($"{title} has been incremented to {count}");
        }
        
        public void Reset() => count = 0;

    }

    [Serializable]
    public struct Condition
    {
        [SerializeField] private QuestStat stat;
        public QuestStat Stat => stat;
        [SerializeField] private int target;
        
        public bool IsCompleted()
        {
            return stat.count >= target;
        }
    }
}