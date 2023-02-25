using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Aurore.MainMenu
{
    public class ButtonScript : MonoBehaviour
    {
        public void Click()
        {
            VolumeManager.Instance.PlayClick();
            //Auto deselect button if we're on mouse control mode
            if (MenuManager.Instance.ControlMode==MenuManager.ControlModes.mouse)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}