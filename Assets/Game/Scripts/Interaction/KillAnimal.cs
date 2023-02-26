using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class KillAnimal : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;
        [SerializeField] private GameObject sound;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
        }

        public void Interact()
        {
            Debug.Log("Interacting with" + gameObject.name);
            _statTriggerComponent.Trigger();
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetComponent<CapsuleCollider>().isTrigger = false;
            Instantiate(sound, transform);
        }
    }
}