using MBT;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MonoBehaviourTree))]
    public class MBTExecutorEnhanced : MonoBehaviour
    {
        public MonoBehaviourTree monoBehaviourTree;
        private bool _freeze = true;

        [ContextMenu("Start Logic")]
        public void StartLogic()
        {
           
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
