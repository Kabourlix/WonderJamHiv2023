using System;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Quests
{
    [UnityEngine.CreateAssetMenu(fileName = "QuestExample", menuName = "K/QuestSystem/Quest", order = 0)]
    public class Quest : ScriptableObject
    {

        private void OnEnable()
        {
            if (ID == default)
                ID = GUID.Generate();
        }

        public GUID ID { get; private set; }
        
        [SerializeField] private string title;
        public string Title => title;
        [SerializeField][TextArea] private string description;
        public string Description => description;
        
        public Condition[] conditions;
        
        public bool IsCompleted()
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsCompleted())
                    return false;
            }

            return true;
        }
        
        
    }
}