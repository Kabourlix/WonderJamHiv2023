using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class KillAnimal : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;
        

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
           
        }


        public void VFX()
        {
            Debug.Log("Interacting with" + gameObject.name);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
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