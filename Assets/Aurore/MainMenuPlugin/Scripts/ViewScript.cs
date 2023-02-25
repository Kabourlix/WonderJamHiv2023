using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aurore.MainMenu
{
    public class ViewScript : MonoBehaviour
    {
        [SerializeField] private Selectable _firstSelected; //The first selected element of the ui, useful for controllers players

        public void Start()
        {
            MenuManager.Instance.ControlModeChanged += OnControlModeChanged;
        }

        public void InitializeView()
        {
            OnControlModeChanged(MenuManager.Instance.ControlMode);
        }

        private void OnControlModeChanged(MenuManager.ControlModes newControlMode)
        {
            if (!this.isActiveAndEnabled) return;
            //Debug.Log(newControlMode);
            if (newControlMode == MenuManager.ControlModes.mouse)
            {
                EventSystem.current.SetSelectedGameObject(null);
                return;
            }

            if (_firstSelected == null) return;
            //_firstSelected.gameObject.SetActive(false);
            _firstSelected.Select();
            _firstSelected.OnSelect(null);
        }
    }
}