using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.InputSystem;

namespace Aurore.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        #region Singleton pattern
        public static MenuManager Instance;

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

        [Tooltip("All the views of the submenu, we will switch between them during navigation. Exemple: Main buttons, settings submenu, credits submenu etc.")]
        [SerializeField] private List<ViewScript> _views;
        [Tooltip("the first view the player should see")]
        [SerializeField] private ViewScript _defaultView;
        public enum ControlModes { mouse, other };
        [SerializeField] private ControlModes _defaultControlMode;

        public Action<ControlModes> ControlModeChanged;

        private ControlModes _controlMode;

        public ControlModes ControlMode
        {
            get => _controlMode;
            set
            {
                if (value == _controlMode)
                {
                    return;
                }
                _controlMode = value;
                ControlModeChanged?.Invoke(_controlMode);
            }
        }

        private void Start()
        {
            if (!_views.Contains(_defaultView))
            {
                Debug.LogError("The default view requested isn't in the views list. Execution will continue with another Default View");
                _defaultView = _views[0];
            }

            if (!_defaultView.gameObject.activeSelf)
            {
                Debug.LogWarning("The default view wasn't the one shown, so we shown it. If you wish for another view to be shown at first, you should change the default view.");
            }

            ShowView(_defaultView);

            _controlMode = _defaultControlMode;

            //If any input is pressed we check its device to switch between mouse mode/other mode
            InputSystem.onActionChange += OnActionChange;
        }

        public void OnActionChange(object obj, InputActionChange change)
        {
            if (this == null) return;
            if (change != InputActionChange.ActionPerformed) return;
            if (((InputAction)obj).activeControl.device.displayName == "Mouse")
            {
                ControlMode = ControlModes.mouse;
            }
            else
            {
                ControlMode = ControlModes.other;
            }
            //Debug.Log($"{((InputAction)obj).activeControl.device.displayName}");
        }

        private void OnDestroy()
        {
            InputSystem.onActionChange -= OnActionChange;
            Instance = null;
        }

        public void LoadScene(string SceneName)
        {
            SceneManager.LoadScene(SceneName);
        }

        //Return true if current view is default view, false otherwise. If false will set view to default view
        public bool EnsureDefaultView()
        {
            if (!_defaultView.gameObject.activeSelf)
            {
                ShowView(_defaultView);
                return false;
            }
            return true;
        }

        public void ShowView(ViewScript viewToShow)
        {
            if (!_views.Contains(viewToShow))
            {
                Debug.LogError("The game object requested isn't in the views list");
                return;
            }

            foreach (ViewScript view in _views)
            {
                view.gameObject.SetActive(view == viewToShow);
                if (view != viewToShow) continue;
                view.InitializeView();
            }
        }

        public void QuitGame()
        {
            Debug.Log("Quit the game");
            Application.Quit();
        }
    }
}

