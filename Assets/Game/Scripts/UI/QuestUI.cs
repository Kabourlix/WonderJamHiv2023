using System;
using Game.Scripts.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class QuestUI : MonoBehaviour
    {
        
        [Header("Quest UI")]
        [SerializeField] private TMP_Text questName;
        [SerializeField] private Image typeImage;
        [SerializeField] private Transform conditionsContainer;
        
        
        [Header("Prefab")]
        [SerializeField] private GameObject conditionPrefab;

        private void Awake()
        {
            QuestManager.Instance.OnQuestCompleted += QuestCompleted;
        }

        private Quest _quest;

        public void Init(Quest quest)
        {
            questName.text = quest.Title;
            typeImage.color = quest.QuestType == QuestType.Evil ? Color.red : Color.green;
            _quest = quest;
            foreach (var c in quest.conditions)
            {
                AddCondition(c);
            }
        }

        private void AddCondition(Condition c)
        {
            var conditionGO = Instantiate(conditionPrefab, conditionsContainer);
            var conditionUI = conditionGO.GetComponent<ConditionUI>();
            conditionUI.Init(c);
            
        }

        private void QuestCompleted(Quest quest)
        {
            if (quest == _quest)
            {
                Destroy(gameObject);
            }
        }
        

    }
}