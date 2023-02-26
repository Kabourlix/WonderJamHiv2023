using System;
using Game.Scripts.Quests;
using Game.Scripts.Systems.Interaction;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class Receive : Interactable
    {
        


        public void VFX()
        {
            Debug.Log("Interacting with" + gameObject.name);
          
        }

        public override void Interact()
        {
            if (!_statTriggerComponent.Trigger()) return;
            VFX();
            OnInteractionSuccess();

            
        }

        


    }
}