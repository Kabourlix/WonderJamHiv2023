using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
    public class StatTriggerComponent : MonoBehaviour
    {
        [SerializeField] private QuestStat statTargeted;
        
        [ContextMenu("Trigger Stat")]
        public void TriggerStat()
        {
            if (statTargeted == null) throw new NullReferenceException("StatTargeted is null");
                
            statTargeted.Increment();
        }
    }
}