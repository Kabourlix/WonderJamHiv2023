using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance;
        private void Awake()
        {
            if(Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
            //DontDestroyOnLoad(gameObject); // Keep it alive through levels
            
            _apparitionWait = new WaitForSeconds(apparitionTime);
        }

        private void Start()
        {
            InputManager.OnShowHideQuestEvent += HideQuestPanel;
            QuestManager.Instance.OnQuestCompleted += (q) =>
            {
                StopAllCoroutines();
                StartCoroutine(ShowQuestCompletedCoroutine(q));
            };
        }

        

        [SerializeField] private GameObject questPanel;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Slider experienceBar;
        [SerializeField] private Slider suspiciousBar;
        
        [Header("Timings")]
        [SerializeField] private float apparitionTime = 2f;
        private WaitForSeconds _apparitionWait;
        
        private Dictionary<Quest,QuestUI> _questUIs = new Dictionary<Quest, QuestUI>();

        public void Hide(ApparitionController ctrl, bool b)
        {
            if (b)
            {
                ctrl.Disapparition();
            }
            else
            {
                ctrl.Apparition();
            }
        }

        #region Quests

        private bool _questPanelActive = true;

        public void HideQuestPanel()
        {
            HideQuestPanel(_questPanelActive);
        }
        public void HideQuestPanel(bool b)
        {
            _questPanelActive = !b;
            Hide(questPanel.GetComponent<ApparitionController>(), b);
        }

        public void AddQuest(Quest quest)
        {
            var questGO = Instantiate(questPrefab, questPanel.transform);
            var ui = questGO.GetComponent<QuestUI>();
            ui.Init(quest);
            _questUIs.Add(quest,ui);
        }

        #endregion

        public void HideExperienceBar(bool b) => Hide(experienceBar.GetComponent<ApparitionController>(),b);
        public void UpdateExpBar(float value) => experienceBar.value = Mathf.Clamp01(value);
        public void UpdateSuspiciousBar(float value) => suspiciousBar.value = Mathf.Clamp01(value);
        
        #region Panel Auto Display

        public void ShowQuestTemp(Quest q)
        {
            if (_questPanelActive)
            {
                StartCoroutine(_questUIs[q].HighlightTempCoroutine());
                return;
            }
            StopAllCoroutines();
            StartCoroutine(ShowQuestTempCoroutine(q));
        }

        private IEnumerator ShowQuestTempCoroutine(Quest q)
        {
            HideQuestPanel(false);
            var ui = _questUIs[q];
            ui.Highlight(true);
            yield return _apparitionWait;
            ui.Highlight(false);
            HideQuestPanel(true);
        }

        private IEnumerator ShowQuestCompletedCoroutine(Quest quest)
        {
            var ui = _questUIs[quest];
            if(!_questPanelActive) HideQuestPanel(false);
            ui.Highlight(true);
            yield return _apparitionWait;
            ui.QuestCompleted();
            yield return _apparitionWait;
            HideQuestPanel(true);
        }

        #endregion
    }
}
