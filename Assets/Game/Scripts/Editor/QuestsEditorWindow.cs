using Game.Scripts.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.Editor
{
    public class QuestsEditorWindow : EditorWindow
    {
        [MenuItem("K/Quests Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<QuestsEditorWindow>();
            window.titleContent = new GUIContent("Quests Editor");
            window.minSize = new Vector2(800, 600);
        }

        private void OnEnable()
        {
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Game/Scripts/Editor/QuestEditorUIDocument.uxml");
            TemplateContainer container = original.CloneTree();
            rootVisualElement.Add(container);
            CreateCardView();
        }

        private void CreateCardView()
        {
            FindAllQuests(out var enemies);

            ListView enemiesList = rootVisualElement.Query<ListView>().First();
            enemiesList.makeItem = () => new Label();
            enemiesList.bindItem = (element, i) => { ((Label) element).text = enemies[i].name; };
            
            enemiesList.itemsSource = enemies;
            enemiesList.fixedItemHeight = 16;
            enemiesList.selectionType = SelectionType.Single;
            
            enemiesList.onSelectionChange += (enumerable) =>
            {
                foreach (var it in enumerable)
                {
                    var enemyInfoBox = rootVisualElement.Q("questData");
                    enemyInfoBox.Clear();
                    
                    var quest = it as Quest;
                    
                    var serializedEnemy = new SerializedObject(quest);
                    var enemyProperty = serializedEnemy.GetIterator();
                    enemyProperty.Next(true);
                    
                    while (enemyProperty.NextVisible(false))
                    {
                        var prop = new PropertyField(enemyProperty);
                        prop.SetEnabled(enemyProperty.name != "m_Script"); // disable script field
                        prop.Bind(serializedEnemy);
                        enemyInfoBox.Add(prop);
                        
                    }
                }
            };

        }

        private void FindAllQuests(out Quest[] quests)
        {
            var guids = AssetDatabase.FindAssets("t:Quest");
            quests = new Quest[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                quests[i] = AssetDatabase.LoadAssetAtPath<Quest>(path);
            }
        }
    }
}