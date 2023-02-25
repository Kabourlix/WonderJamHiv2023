using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aurore.MainMenu
{
    public class PauseManager : MonoBehaviour
    {
        #region Singleton pattern
        public static PauseManager Instance;

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

        [Tooltip("If true, hitting pause will only display menu without pausing the actual game, good for online games.")]
        [SerializeField] private bool _mustStopTime=true;
        [Tooltip("Slide the actual pause menu in this field.")]
        [SerializeField] private MenuManager _pauseMenu;
        private bool _isPaused;
        private float _cachedTimeScale; //Used to store time scale before the pause is on, to reset it to it afterwards. Useful if the game has a slow mo mode or something

        public void Start()
        {
            _isPaused = _pauseMenu.gameObject.activeSelf;
            Aurore.MainMenu.InputManager.OnPauseEvent += SwitchPauseState;
        }

        public void SwitchPauseState()
        {
            if (_isPaused)
            {
                BackAction();
                return;
            }
            EnablePause();
        }

        public void EnablePause()
        {
            if (_isPaused) return;
            if (_mustStopTime)
            {
                _cachedTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            _isPaused = true;
            _pauseMenu.gameObject.SetActive(true);
        }

        public void DisablePause()
        {
            if (!_isPaused) return;
            if (_mustStopTime)
            {
                Time.timeScale = _cachedTimeScale;
            }
            _isPaused = false;
            _pauseMenu.gameObject.SetActive(false);
        }

        //Should be called when the player press a "back" button, will go back to the main pause view if in settings or something, will exit pause if already on main view
        public void BackAction()
        {
            if (!_isPaused) return;
            if (_pauseMenu.EnsureDefaultView())
            {
                DisablePause();
            }
        }
    }
}

