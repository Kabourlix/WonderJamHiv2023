using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aurore.MainMenu
{
    public class InputManager : MonoBehaviour
    {
        #region Singleton pattern
        public static InputManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        #endregion

        public static event Action OnPauseEvent;

        private void OnPause()
        {
            OnPauseEvent.Invoke();
        }
    }
}

