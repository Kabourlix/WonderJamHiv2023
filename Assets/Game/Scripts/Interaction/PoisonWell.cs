using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class PoisonWell : MonoBehaviour
    {
        private StatTriggerComponent _statTriggerComponent;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
        }

        public void OnInteract()
        {
            Debug.Log("Interacting with" + gameObject.name);
            _statTriggerComponent.Trigger();
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetComponent<CapsuleCollider>().isTrigger = false;
        }
    }
}