using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class CollectAndPickupItem : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;
        [HideInInspector]
        public bool PickedUp = false;
        private LeashFollowScript _followScript;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
        }

        public void Interact()
        {
            _statTriggerComponent.Trigger();
            Rigidbody rb=gameObject.GetComponent<Rigidbody>();

            if(rb==null) rb = gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.interpolation= RigidbodyInterpolation.Interpolate;
            LeashFollowScript leash= InteracterScript.Instance.SpawnNewLeash(rb);
            _followScript = leash;
            PickedUp = true;
        }

        public void OnInteractionSuccess()
        {
            throw new NotImplementedException();
        }

        public void Use(GameObject user)
        {
            if (!PickedUp) return;
            _followScript.OnUse(user);
        }
    }
}