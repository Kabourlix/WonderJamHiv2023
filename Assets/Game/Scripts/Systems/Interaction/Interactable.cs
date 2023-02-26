using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Systems.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public abstract class Interactable : MonoBehaviour
    {

        private LayerMask _interactLayer;
        protected StatTriggerComponent _statTriggerComponent;
        [SerializeField] private Quest relatedQuest;
        protected virtual void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
            _interactLayer = LayerMask.NameToLayer("Interactable");
            gameObject.layer= 0;
        }

        protected virtual void Start()
        {
            if (relatedQuest != null)
            {
                QuestManager.Instance.OnQuestSetActive += ActivateQuest;
            }
        }
        
        private void ActivateQuest(Quest q)
        {
            Debug.Log($"Actrive quest {q.name} with rq {relatedQuest.name}, layer is {gameObject.layer}");
            if (!q.name.Equals(relatedQuest.name)) return;
            gameObject.layer = _interactLayer;
        }

        public abstract void Interact();

        public virtual void OnInteractionSuccess()
        {
            Debug.Log("Interact Success with " + gameObject.name);
            gameObject.layer = 0;
            if(relatedQuest !=null) QuestManager.Instance.OnQuestSetActive -= ActivateQuest;
            PlayerController.Instance.IsSuspect = true;
        }
    }
}