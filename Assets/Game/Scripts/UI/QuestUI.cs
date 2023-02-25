using System;
using System.Collections;
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
        [SerializeField] private float highlightTime = 1f;
        [SerializeField] private float highlightAlpha = 0.5f;
        private WaitForSeconds _highlightWait;
        private Image _highlight;

        private Quest _quest;

        private void Awake()
        {
            _highlightWait = new WaitForSeconds(highlightTime);
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
        
        private Coroutine _highlightCoroutine;
        public void Highlight(bool b)
        {
            if (_highlightCoroutine != null)
                StopCoroutine(_highlightCoroutine);
            _highlightCoroutine = StartCoroutine(HighlightCoroutine(b));
        }
        
        private IEnumerator HighlightCoroutine(bool b)
        {
            var target = b ? highlightAlpha : 0;
            while (Mathf.Abs(_highlight.color.a -target) > 0.01f)
            {
                var color = _highlight.color;
                var alpha = Mathf.Lerp(color.a,  target, Time.deltaTime);
                _highlight.color = new Color(color.r, color.g, color.b, alpha);
                yield return typeof(WaitForEndOfFrame);
            }
            _highlight.color = new Color(_highlight.color.r, _highlight.color.g, _highlight.color.b, target);
            _highlightCoroutine = null;
        }
        
        public IEnumerator HighlightTempCoroutine()
        {
            Highlight(true);
            yield return _highlightWait;
            Highlight(false);
        }

        public void QuestCompleted()
        {
            Destroy(gameObject);
        }
        

    }
}