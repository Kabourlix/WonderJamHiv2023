using Game.Scripts.Interaction;
using Game.Scripts.Quests;
using Game.Scripts.Systems.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(StatTriggerComponent))]
public class ItemUser : Interactable
{

    [FormerlySerializedAs("_messageOnFail")] [SerializeField] private string messageOnFail;
    [FormerlySerializedAs("_messageOnComplete")]
    [Tooltip("What the npc will say if you have everything")]
    [SerializeField] private string messageOnComplete = "Thank you";
    [FormerlySerializedAs("_onComplete")] [SerializeField] private UnityEvent onComplete;



    public override void Interact()
    {
        if (!_statTriggerComponent.Trigger())
        {
            DialogueSystem.AddMessage(messageOnFail, 5f);
            return;
        };

        OnInteractionSuccess();
    }

    public override void OnInteractionSuccess()
    {
        base.OnInteractionSuccess();
        onComplete?.Invoke();
        if (messageOnComplete == null || messageOnComplete == "") return;
        DialogueSystem.AddMessage(messageOnComplete, 5f);
    }
}
