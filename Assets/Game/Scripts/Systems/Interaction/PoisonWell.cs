using System;
using Game.Scripts.Quests;
using Game.Scripts.Systems.Interaction;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class PoisonWell : Interactable
    {
        public void VFX()
        {
            Debug.Log("Interacting with" + gameObject.name);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }

        public override void Interact()
        {
            if (!_statTriggerComponent.Trigger()) return;
            VFX();
            OnInteractionSuccess();
            
        }
    }
}