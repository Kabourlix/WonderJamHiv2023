using System;
using Game.Scripts.Quests;
using UnityEngine;

namespace Game.Scripts.Proto
{
    public class TestQuest : MonoBehaviour, IQuestTrigger
    {
        [SerializeField] private QuestStat statTargeted;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TriggerStat(statTargeted);
            }
        }

        public void TriggerStat(QuestStat stat)
        {
            stat.Increment();
            QuestManager.Instance.CheckActiveQuest();
        }
    }
}