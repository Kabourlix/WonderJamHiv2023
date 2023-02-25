using System;
using UnityEngine;

namespace Game.Scripts.Quests
{
    public class SpecificQuestTrigger : QuestTrigger
    {
        [SerializeField] private Quest questTargeted;
        
        public override void Trigger()
        {
           questTargeted.QuestCompleted();
        }
    }
    
}