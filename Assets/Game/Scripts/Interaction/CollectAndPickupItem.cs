using System;
using System.Collections.Generic;
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
        [SerializeField] private Quest _usedOn;
        [Tooltip("The colliders that are not used for the physic and that should be disables")]
        [SerializeField] List<Collider> _additionalColliders;

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
            QuestManager.Instance.OnQuestCompleted += OnQuestCompleted;
            foreach(Collider c in _additionalColliders)
            {
                c.enabled = false;
            }
        }

        private void OnQuestCompleted(Quest quest)
        {
            if (quest != _usedOn) return;
            _followScript.OnUse();
        }

        public void OnInteractionSuccess()
        {
            throw new NotImplementedException();
        }

    }
}