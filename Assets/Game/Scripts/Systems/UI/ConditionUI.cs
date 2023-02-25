using Game.Scripts.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.Systems.UI
{
    public class ConditionUI : MonoBehaviour
    {
        [FormerlySerializedAs("_conditionName")] [SerializeField] private TMP_Text conditionName;
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponentInChildren<Toggle>();
            _toggle.isOn = false;
            _toggle.interactable = false;
        }


        public void Init(Condition c)
        {
            conditionName.text = c.Stat.Title;
            c.OnConditionCompleted += () => _toggle.isOn = true;
        }
    }
}