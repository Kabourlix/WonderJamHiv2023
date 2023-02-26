using UnityEngine;

namespace Game.Scripts.Quests
{
    public abstract class QuestTrigger : MonoBehaviour
    {
        public abstract bool Trigger();
    }
}