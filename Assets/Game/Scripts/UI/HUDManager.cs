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
        }
        
        [SerializeField] private GameObject questPanel;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Slider experienceBar;
        [SerializeField] private Slider suspiciousBar;

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
        
        public void HideQuestPanel(bool b) => Hide(questPanel.GetComponent<ApparitionController>(),b);
        public void HideExperienceBar(bool b) => Hide(experienceBar.GetComponent<ApparitionController>(),b);
        
        public void AddQuest(Quest quest)
        {
            var questGO = Instantiate(questPrefab, questPanel.transform);
            questGO.GetComponent<QuestUI>().Init(quest);
        }
            
        
        
        
    }
}
