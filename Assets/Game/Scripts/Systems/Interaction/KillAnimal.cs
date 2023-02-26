using System;
using Game.Scripts.Enemies;
using Game.Scripts.Quests;
using Game.Scripts.Systems.Interaction;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Interaction
{
    [RequireComponent(typeof(StatTriggerComponent))]
    public class KillAnimal : Interactable
    {
        [SerializeField] private GameObject[] vfx;


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

        public override void Interact()
        {
            if (!_statTriggerComponent.Trigger()) return;
            VFX();
            OnInteractionSuccess();

        }
    }
}