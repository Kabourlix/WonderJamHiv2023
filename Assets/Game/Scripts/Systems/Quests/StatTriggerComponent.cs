using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
    public class StatTriggerComponent : QuestTrigger
    {
        [SerializeField] private QuestStat statTargeted;
        [Tooltip("Sound source will be played when Target is triggered ")]
        [SerializeField] private AudioSource _audioSource;


        [ContextMenu("Trigger Stat")]
        public override void Trigger()
        {
            if (statTargeted == null) throw new NullReferenceException("StatTargeted is null");
                
            statTargeted.Increment();

            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }
    }
}