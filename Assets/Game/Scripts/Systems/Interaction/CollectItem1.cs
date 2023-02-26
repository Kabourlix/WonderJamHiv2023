using System;
using Game.Scripts.Quests;
using Game.Scripts.Systems.Interaction;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class CollectItem1 : Interactable
    {
        
        public override void Interact()
        {
            _statTriggerComponent.Trigger();
            Debug.Log("Interacting with" + gameObject.name);
            gameObject.SetActive(false);

        }
        
    }
}