using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(fileName = "StateTransition", menuName = "K/StateTransition", order = 0)]
    public class StateTransitions : ScriptableObject
    {
        public StateTransitionsStruct[] Transitions;
    }
}