using MBT;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Game.Scripts.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MonoBehaviourTree))]
    public class MBTExecutorEnhanced : MonoBehaviour
    {
        public MonoBehaviourTree monoBehaviourTree;
        [SerializeField] private Patroller patroller;
        public Patroller Patroller => patroller;
        [SerializeField] private NavMeshAgent agent;
        public NavMeshAgent Agent => agent;
        
        private bool _freeze = true;

        [ContextMenu("Start Logic")]
        public void StartLogic()
        {
           if (patroller is null || agent is null) throw new System.NullReferenceException("Patroller and/or Argent is not set.");
            monoBehaviourTree.Restart();
            Unfreeze();
            Debug.Log($"{gameObject.name} started logic");
        }
        public void Freeze() => _freeze = true;
        public void Unfreeze() => _freeze = false;
        public void Reset()
        {
            monoBehaviourTree = GetComponent<MonoBehaviourTree>();
            OnValidate();
        }

        void Update()
        {
            if (_freeze) return;

                monoBehaviourTree.Tick();
        }

        void OnValidate()
        {
            if (monoBehaviourTree != null && monoBehaviourTree.parent != null)
            {
                monoBehaviourTree = null;
                Debug.LogWarning("Subtree should not be target of update. Select parent tree instead.", this.gameObject);
            }
        }
    }
}
