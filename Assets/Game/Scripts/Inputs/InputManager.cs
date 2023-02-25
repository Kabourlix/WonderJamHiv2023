using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Inputs
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
        public static event Action<Vector2> OnMoveEvent;
        public static event Action OnInteractEvent;
        public static event Action OnGrabEvent;

        public static event Action OnShowHideQuestEvent;
        public static event Action OnTest1Event;
        public static event Action OnTest2Event;
        public static event Action OnTest3Event;

        private void OnPause()
        {
            OnPauseEvent.Invoke();
        }

        private void OnMove(InputValue value)
        {
            OnMoveEvent?.Invoke(value.Get<Vector2>());
        }

        private void OnInteract()
        {
            OnInteractEvent?.Invoke();
        }

        private void OnTest1()
        {
            OnTest1Event?.Invoke();
        }

        private void OnTest2()
        {
            OnTest2Event?.Invoke();
        }

        private void OnTest3()
        {
            OnTest3Event?.Invoke();
        }

        private void OnGrab()
        {
            OnGrabEvent?.Invoke();
        }

        private void OnShowHideQuest()
        {
            OnShowHideQuestEvent?.Invoke();
        }
    }
}



