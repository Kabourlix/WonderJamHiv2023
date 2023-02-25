using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.UI;
using UnityEngine;
using Game.Scripts.Utility;

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
            _completedQuests = new List<Quest>();
        }

        #endregion

        [SerializeField] private int activeQuestSample = 3;
        
        public event Action<Quest> OnQuestCompleted;
        private List<Quest> _completedQuests;
        private List<Quest> _activeQuests;
        private List<Quest> _unusedQuests; 
        //public Quest[] baseQuest;
        [SerializeField] private Quest[] allQuests;
        [SerializeField] private QuestStat[] allQuestStats;


        private void Start()
        {
            ResetAllStats(); // Reset at the beginning of the game
            
            InitUnusedQuests();
            SampleQuests(activeQuestSample);
            Debug.Log($"<color=orange>{_activeQuests.Count} quests active.</color>");
        }
        
        public void AddQuest(Quest quest)
        {
            _activeQuests.Add(quest);
            HUDManager.Instance.AddQuest(quest);
        }

        public void RemoveQuest(Quest quest)
        {
            _activeQuests.Remove(quest);
        }


        public void QuestCompleted(Quest quest)
        {
            Debug.Log($"<color=orange>Quest {quest.Title} completed!</color>");
            _completedQuests.Add(quest);
            _activeQuests.Remove(quest);
            OnQuestCompleted?.Invoke(quest);
        }




        /// <summary>
        /// HeavyFunction to reset all the stats at the beginning of the game only
        /// </summary>
        private void ResetAllStats()
        {
            Debug.LogWarning("All stats have been reset.");
            foreach (var stat in allQuestStats)
            {
                //Reset the stat
                stat.Reset();
                stat.Link();
            }
        }
        
        /// <summary>
        /// Set up all the quest created in the project.
        /// Heavy : Only use at the beginning of the game
        /// </summary>
        private void InitUnusedQuests()
        {
            if (_unusedQuests != null)
            {
                Debug.LogWarning("Unused quests already initialized.");
                return;
            }

            _unusedQuests = allQuests.ToList();
            Debug.Log($"<color=orange>{_unusedQuests.Count} quests found.</color>");
        }
    
        // /// <summary>
        // /// Get all the existing quests in the project
        // /// </summary>
        // /// <returns>An array of all existing quests</returns>
        // public static Quest[] GetAllQuests()
        // {
        //     var questsNameAsset = AssetDatabase.FindAssets("t:Quest");
        //     
        //     var quests = new Quest[questsNameAsset.Length];
        //     for (int i = 0; i < questsNameAsset.Length; i++)
        //     {
        //         var path = AssetDatabase.GUIDToAssetPath(questsNameAsset[i]);
        //         quests[i] = AssetDatabase.LoadAssetAtPath<Quest>(path);
        //     }
        //
        //     return quests;
        // }

        private void SampleQuests(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (_unusedQuests.Count == 0)
                    return;
                var index = KUtils.Rnd.Next(_unusedQuests.Count);
                var quest = _unusedQuests[index];
                AddQuest(quest);
                _unusedQuests.RemoveAt(index);
            }
            
        }
    }
}
