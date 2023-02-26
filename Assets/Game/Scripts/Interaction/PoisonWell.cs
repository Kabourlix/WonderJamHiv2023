using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class PoisonWell : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
        }

        public void Interact()
        {
            Debug.Log("Interacting with" + gameObject.name);
            _statTriggerComponent.Trigger();
            gameObject.SetActive(false);
        }
    }
}