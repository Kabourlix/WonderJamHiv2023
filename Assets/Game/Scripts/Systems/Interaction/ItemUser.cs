using Game.Scripts.Interaction;
using Game.Scripts.Quests;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatTriggerComponent))]
public class ItemUser : MonoBehaviour, IInteractable
{
    private StatTriggerComponent _statTriggerComponent;

    [SerializeField] private string _messageOnFail;
    [Tooltip("What the npc will say if you have everything")]
    [SerializeField] private string _messageOnComplete = "Thank you";
    [SerializeField] private UnityEvent _onComplete;


    private void Awake()
    {
        _statTriggerComponent = GetComponent<StatTriggerComponent>();
    }

    public void Interact()
    {
        if (!_statTriggerComponent.Trigger())
        {
            DialogueSystem.AddMessage(_messageOnFail, 5f);
            return;
        };

        OnInteractionSuccess();
    }

    public void OnInteractionSuccess()
    {
        _onComplete?.Invoke();
        if (_messageOnComplete == null || _messageOnComplete == "") return;
        DialogueSystem.AddMessage(_messageOnComplete, 5f);
        gameObject.layer = 0;
    }
}
