using System;
using Game.Scripts.Enemies;
using Game.Scripts.Quests;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class MakeMimic : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
           
        }


        public void VFX()
        {
            
           

            Debug.Log("Interacting with" + gameObject.name);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);


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