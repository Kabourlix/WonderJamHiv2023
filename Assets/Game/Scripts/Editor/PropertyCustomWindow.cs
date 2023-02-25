using System;
using Game.Scripts.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.Editor
{
    public class PropertyCustomWindow : EditorWindow
    {
        [MenuItem("K/Property Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<PropertyCustomWindow>();
            window.titleContent = new GUIContent("Property Editor");
            window.minSize = new Vector2(800, 600);
        }

        private void OnEnable()
        {
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Game/Scripts/Editor/QuestPropertyWindow.uxml");
            TemplateContainer container = original.CloneTree();
            rootVisualElement.Add(container);
            CreateCardView();
            
            var createQuestButton = rootVisualElement.Q<Button>("propButton");
            createQuestButton.clicked += CreateProperty;
        }
        

        private void OnDisable()
        {
            var createQuestButton = rootVisualElement.Q<Button>("propButton");
            createQuestButton.clicked -= CreateProperty;
        }
        
        
        private void CreateCardView()
        {
            FindAllProperties(out var properties);

            ListView propList = rootVisualElement.Query<ListView>().First();
            propList.makeItem = () => new Label();
            propList.bindItem = (element, i) => { ((Label) element).text = properties[i].name; };
            
            propList.itemsSource = properties;
            propList.fixedItemHeight = 16;
            propList.selectionType = SelectionType.Single;
            
            propList.onSelectionChange += (enumerable) =>
            {
                foreach (var it in enumerable)
                {
                    var enemyInfoBox = rootVisualElement.Q("questData");
                    enemyInfoBox.Clear();
                    
                    var prop = it as QuestStat;
                    
                    var serializedEnemy = new SerializedObject(prop);
                    var enemyProperty = serializedEnemy.GetIterator();
                    enemyProperty.Next(true);
                    
                    while (enemyProperty.NextVisible(false))
                    {
                        var propField = new PropertyField(enemyProperty);
                        propField.SetEnabled(enemyProperty.name != "m_Script"); // disable script field
                        propField.Bind(serializedEnemy);
                        enemyInfoBox.Add(propField);
                        
                    }
                }
            };

        }
        
        private void FindAllProperties(out QuestStat[] quests)
        {
            var guids = AssetDatabase.FindAssets("t:QuestStat");
            quests = new QuestStat[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                quests[i] = AssetDatabase.LoadAssetAtPath<QuestStat>(path);
            }
            
        }
        
        private void CreateProperty()
        {
            var quest = ScriptableObject.CreateInstance<QuestStat>();
            
            var path = EditorUtility.SaveFilePanelInProject("Save Quest", "prop" , "asset", "Save Property",
                "Assets/Game/Data/QuestsStats/");
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Path is empty");
            }
            if(path.Split('.')[^1] != "asset")
                throw new Exception("File must be .asset");
            
            AssetDatabase.CreateAsset(quest, path);
            AssetDatabase.SaveAssets();
            CreateCardView();
        }
    }
}