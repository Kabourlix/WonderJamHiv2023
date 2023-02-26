using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Systems.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public abstract class Interactable : MonoBehaviour
    {

        private LayerMask _interactLayer;
        protected StatTriggerComponent _statTriggerComponent;
        protected virtual void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
            _interactLayer = LayerMask.NameToLayer("Interactable");
        }
        
        public abstract void Interact();

        public virtual void OnInteractionSuccess()
        {
            gameObject.layer = 0;
        }
    }
}