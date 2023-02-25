using System;
using CartoonFX;
using Game.Inputs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Player
{
    public class EvolveViewSwitch : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera evolveCamera;
        [SerializeField] private ParticleSystem evolveEffect;
        [SerializeField] private PlayerController playerController;
        private float _effectDuration;
        private void Start()
        {
            PlayerController.OnEvolve += OnSwitchView;
            PlayerController.OnEvolveEnd += OnSwitchView;
            _effectDuration = evolveEffect.main.duration;
        }

        private void OnDestroy()
        {
            PlayerController.OnEvolve -= OnSwitchView;
            PlayerController.OnEvolveEnd -= OnSwitchView;
            
        }

        private void OnSwitchView()
        {
            if (!mainCamera.enabled) //If switch back to main cam
            {
                LeanTween.value(gameObject, 0, 1, 1.5f)
                    .setOnComplete(() =>
                    {
                        mainCamera.enabled = true;
                        evolveCamera.enabled = false;
                        InputManager.Instance.EnableControls(true);
                    });
            }
            else //If switch to evolve cam
            {
                mainCamera.enabled = !mainCamera.enabled;
                evolveCamera.enabled = !evolveCamera.enabled;
                LeanTween.value(gameObject, 0, 1, 1.5f)
                    .setOnComplete(() =>
                    {
                        Debug.Log("Evolve Anim start");
                        evolveEffect.Play();
                        LeanTween.delayedCall(_effectDuration * 0.5f, () => { playerController.EvolveEnd(); });
                    });
            }

        }
    }
}
