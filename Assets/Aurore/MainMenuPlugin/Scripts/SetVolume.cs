using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Aurore.MainMenu
{
    /// <summary>
    /// To adjust the game's volume.
    /// </summary>
    public class SetVolume : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] VolumeManager.VolumeKey _volumeKey;

        public void Start()
        {
            _slider = gameObject.GetComponent<Slider>();
            float volume = VolumeManager.Instance.GetLevel(_volumeKey);
            float sliderValue = Mathf.Pow(10, (volume / 20.0f));
            _slider.value = sliderValue;
        }

        public void SetLevel(float sliderValue)
        {
            sliderValue = Mathf.Clamp(sliderValue, 0.00001f, 1);
            VolumeManager.Instance.SetLevel(_volumeKey, Mathf.Log10(sliderValue) * 20);
        }
    }
}