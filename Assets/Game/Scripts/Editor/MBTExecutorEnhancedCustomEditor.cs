using Game.Scripts.Enemies;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor
{
    [CustomEditor(typeof(MBTExecutorEnhanced))]
    public class MBTExecutorEnhancedCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var mbtExecutorEnhanced = (MBTExecutorEnhanced) target;
            GUILayout.Space(20);
            if (GUILayout.Button("Start Logic"))
            {
                mbtExecutorEnhanced.StartLogic();
            }
            
            if (GUILayout.Button("Freeze"))
            {
                mbtExecutorEnhanced.Freeze();
            }
            if (GUILayout.Button("Unfreeze"))
            {
                mbtExecutorEnhanced.Unfreeze();
            }
        }
    }
}