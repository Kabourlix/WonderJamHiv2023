using System;
using Game.Scripts.Enemies;
using Game.Scripts.Quests;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class KillAnimal : MonoBehaviour, IInteractable
    {
        private StatTriggerComponent _statTriggerComponent;
        [SerializeField] private GameObject[] vfx;

        private void Awake()
        {
            _statTriggerComponent = GetComponent<StatTriggerComponent>();
           
        }


        public void VFX()
        {
            GetComponentInChildren<MBTExecutorEnhanced>().Freeze();
            //Stop the navMeshAgent
            GetComponent<NavMeshAgent>().isStopped = true;
            
            for(var i = 0 ; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false); 
            }

            foreach (var go in vfx)
            {
                go.SetActive(true);
            }
            
            Debug.Log("Interacting with" + gameObject.name);
            LeanTween.value(gameObject,0,1, 1.5f)
                .setOnComplete(() => { gameObject.SetActive(false); });
            
            
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