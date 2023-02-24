using UnityEngine;

namespace Game.Scripts.Quests
{
    [CreateAssetMenu(fileName = "Condition", menuName = "K/Quest", order = 0)]
    public class QuestStat : ScriptableObject
    {
        [SerializeField] private int goal;
        public int Goal => goal;
        
        public int compteur { get; private set; }
         
    }
}