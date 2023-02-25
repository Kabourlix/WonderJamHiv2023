using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Game.Scripts.Quests
{
    public class QuestManager : MonoBehaviour
    {

        #region Singleton Declaration

        public static QuestManager Instance;

        private void Awake()
        {
            if(Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it alive through levels
            _activeQuests = new List<Quest>();
        }

        #endregion
        
        public event Action<Quest> OnQuestCompleted;
        private List<Quest> _activeQuests;
        public Quest[] baseQuest;


        private void Start()
        {
            ResetAllStats(); // Reset at the beginning of the game
            foreach (var quest in baseQuest)
            {
                _activeQuests.Add(quest);
            }
        }


        public void CheckActiveQuest()
        {
            if (_activeQuests is null || _activeQuests.Count == 0) return;
            foreach (var quest in _activeQuests)
            {
                if(quest.IsCompleted())
                {
                    Debug.Log($"quest {quest.name} is completed!");
                    OnQuestCompleted?.Invoke(quest);
                }
            }
        }




        /// <summary>
        /// HeavyFunction to reset all the stats at the beginning of the game only
        /// </summary>
        private void ResetAllStats()
        {
            Debug.LogWarning("All stats have been reset.");
            //Look for every QuestStat in the project
            var stats = AssetDatabase.FindAssets("t:QuestStat");
            foreach (var stat in stats)
            {
                //Get the path of the asset
                var path = AssetDatabase.GUIDToAssetPath(stat);
                //Load the asset
                var questStat = AssetDatabase.LoadAssetAtPath<QuestStat>(path);
                //Reset the stat
                questStat.Reset();
            }
        }
    }
}
