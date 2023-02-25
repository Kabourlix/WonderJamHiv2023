using Game.Scripts.Quests;

namespace Game.Scripts.Editor
{
    [UnityEditor.CustomEditor(typeof(Quest))]
    public class QuestSOCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
            //Space
            UnityEditor.EditorGUILayout.Space();
            var quest = (Quest) target;
            UnityEditor.EditorGUILayout.LabelField("ID", quest.ID.ToString());
        }
    }
}