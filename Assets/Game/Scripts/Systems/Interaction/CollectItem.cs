using System;
using Game.Scripts.Quests;
using Game.Scripts.Systems.Interaction;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class CollectItem : Interactable
    {

        public override void Interact()
        {
            Debug.Log("Interacting with" + gameObject.name);
            _statTriggerComponent.Trigger();
        }

       
    }
}