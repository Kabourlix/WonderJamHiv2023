using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Systems.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class MakeMimic : Interactable
    {
        public void VFX()
        {
            Debug.Log("Interacting with" + gameObject.name);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
        }

        public override void Interact()
        {
            if (!_statTriggerComponent.Trigger()) return;
            VFX();
            OnInteractionSuccess();

        }
    }
}