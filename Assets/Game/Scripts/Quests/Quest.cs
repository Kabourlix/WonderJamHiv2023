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

            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    condition.LinkEvent();
                    condition.OnConditionCompleted += () => IsCompleted();
                }
            }
        }

        #region Properties

        public GUID ID { get; private set; }
        
        [SerializeField] private string title;
        public string Title => title;
        [SerializeField][TextArea] private string description;
        public string Description => description;
        
        public Condition[] conditions;

        [SerializeField] private QuestType questType;
        public QuestType QuestType => questType;

        #endregion
        
        
        public void IsCompleted()
        {
            Debug.Log($"Checking if quest {title} is completed");
            foreach (var condition in conditions)
            {
                if (!condition.IsCompleted)
                    return;
            }
            QuestCompleted();
        }
        
        public void QuestCompleted() => QuestManager.Instance.QuestCompleted(this);
        
        
    }
}