using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
    public class StatTriggerComponent : QuestTrigger
    {
        [SerializeField] private QuestStat statTargeted;
        
        [ContextMenu("Trigger Stat")]
        public override void Trigger()
        {
            if (statTargeted == null) throw new NullReferenceException("StatTargeted is null");
                
            statTargeted.Increment();
        }
    }
}