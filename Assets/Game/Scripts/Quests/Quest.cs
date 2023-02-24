using System;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Quests
{
    [UnityEngine.CreateAssetMenu(fileName = "QuestExemple", menuName = "K/Quest", order = 0)]
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
        
        
    }
}