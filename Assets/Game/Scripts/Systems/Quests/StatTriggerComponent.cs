using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Quests
{
    public class StatTriggerComponent : QuestTrigger
    {
        [FormerlySerializedAs("statTargeted")] [SerializeField] private QuestStat property;
        public QuestStat Property => property;
        [FormerlySerializedAs("_audioSource")]
        [Tooltip("Sound source will be played when Target is triggered ")]
        [SerializeField] private AudioSource audioSource;


        [ContextMenu("Trigger Stat")]
        public override bool Trigger()
        {
            if (property == null) throw new NullReferenceException("StatTargeted is null");
                
            var success = property.Increment();

            if (audioSource != null)
            {
                audioSource.Play();
            }

            return success;
        }
    }
}