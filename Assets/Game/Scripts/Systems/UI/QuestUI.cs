using System;
using System.Collections;
using Game.Scripts.Quests;
using Game.Scripts.Systems.UI;
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
        [FormerlySerializedAs("highlightTime")] [SerializeField] private float highlightExposedTime = 1.5f;
        [SerializeField] private float highlightSmoothTime = 0.3f;
        [SerializeField] private float highlightAlpha = 0.5f;
        private Image _highlight;

        private Quest _quest;

        private void Awake()
        {
            _highlight = GetComponent<Image>();
        }

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
        
        
        public void Highlight(Action callback)
        {
            var c = _highlight.color;
            LeanTween.value(gameObject, 0, highlightAlpha, highlightSmoothTime).setOnUpdate((float val) =>
            {
                _highlight.color = new Color(c.r,c.g,c.b,val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0, 1, highlightExposedTime)
                    .setOnComplete
                    (() =>
                        {
                            LeanTween.value(gameObject, highlightAlpha, 0, highlightSmoothTime)
                                .setOnUpdate((float val) => { _highlight.color = new Color(c.r, c.g, c.b, val); });
                        }
                    ).setOnComplete(callback);
            });
            //Highligh the quest 
        }
        
        

        public void QuestCompleted()
        {
            Destroy(gameObject);
        }
        

    }
}