using System;
using Game.Scripts.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class ConditionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _conditionName;
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponentInChildren<Toggle>();
            _toggle.isOn = false;
            _toggle.interactable = false;
        }


        public void Init(Condition c)
        {
            _conditionName.text = c.Stat.Title;
            c.OnConditionCompleted += () => _toggle.isOn = true;
        }
    }
}