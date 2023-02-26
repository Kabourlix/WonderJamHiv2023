using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class PoisonWell : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;
        private Collider _collider;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
            _collider = GetComponent<Collider>();
        }

        public void VFX()
        {
            Debug.Log("Interacting with" + gameObject.name);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }

        public void Interact()
        {
            if (!_statTriggerComponent.Trigger()) return;
            VFX();
            OnInteractionSuccess();
            
        }

        public void OnInteractionSuccess()
        {
            gameObject.layer = 0;
        }
    }
}