using Game.Scripts.Interaction;
using Game.Scripts.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(StatTriggerComponent))]
public class ItemUser : MonoBehaviour, IInteractable
{
    private StatTriggerComponent _statTriggerComponent;

    [System.Serializable]
    struct RequiredItem
    {
        [Tooltip("The item in scene that is required")]
        public GameObject itemGo;
        [Tooltip("What the npc will say if you don't have it")]
        public string messageIfNotPickedUp;
    }
    [SerializeField] RequiredItem[] _requiredItems;
    [Tooltip("What the npc will say if you have everything")]
    [SerializeField] private string _messageOnComplete="Thank you";

    private void Awake()
    {
        _statTriggerComponent = GetComponent<StatTriggerComponent>();
    }

    public void Interact()
    {
        foreach (var item in _requiredItems) 
        {
            CollectAndPickupItem itemScript=item.itemGo.GetComponent<CollectAndPickupItem>();
            if (!itemScript.PickedUp)
            {
                DialogueSystem.AddMessage(item.messageIfNotPickedUp, 5f);
                return;
            }
        }

        foreach (var item in _requiredItems)
        {
            CollectAndPickupItem itemScript = item.itemGo.GetComponent<CollectAndPickupItem>();
            itemScript.Use(gameObject);
        }

        if (_messageOnComplete == null || _messageOnComplete == "") return;
        DialogueSystem.AddMessage(_messageOnComplete, 5f);
    }
}
