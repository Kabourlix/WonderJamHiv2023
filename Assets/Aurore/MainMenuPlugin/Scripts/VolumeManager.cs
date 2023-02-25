using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Aurore.MainMenu
{
    [System.Serializable]
    struct VolumeKeyStringAssociation
    {
        public VolumeManager.VolumeKey key;
        public string name;
    }

    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        public enum VolumeKey { masterVolume, sfxVolume, musicVolume };
        [SerializeField] AudioSource _clickSource;

        [SerializeField] VolumeKeyStringAssociation[] _volumeKeyStringAssociations;
        private Dictionary<VolumeKey, string> _volumeKeyNames;

        #region Singleton pattern
        public static VolumeManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _volumeKeyNames = new Dictionary<VolumeKey, string>();
            foreach (VolumeKeyStringAssociation volumeKeyStringAssociation in _volumeKeyStringAssociations)
            {
                _volumeKeyNames.Add(volumeKeyStringAssociation.key, volumeKeyStringAssociation.name);
            }
        }
        #endregion

        private void Start()
        {
            ApplyVolumePrefs();
        }

        public void SetLevel(VolumeKey volumeKey, float volumeLevel)
        {
            PlayerPrefs.SetFloat(_volumeKeyNames[volumeKey], volumeLevel);
            ApplyVolumePrefs();
        }

        public float GetLevel(VolumeKey volumeKey)
        {
            return PlayerPrefs.GetFloat(_volumeKeyNames[volumeKey], 0);
        }


        public void ApplyVolumePrefs()
        {
            foreach (string volumeString in _volumeKeyNames.Values)
            {
                float volumeValue = PlayerPrefs.GetFloat(volumeString, 0);
                _audioMixer.SetFloat(volumeString, volumeValue);
            }
        }

        public void PlayClick()
        {
            _clickSource.Stop();
            _clickSource.Play();
        }
    }
}