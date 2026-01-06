using System;
using System.Collections;
using DG.Tweening;
using Southbyte.SaveSystem;
using UnityEngine;

namespace Southbyte.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        private const float FadeDuration = 1f;
        private const float DelayBetweenMusicClips = 3f;
        
        public event Action OnMusicEnded;
        
        [SerializeField] private AudioSource _soundAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;
        
        public float SoundVolume => _soundAudioSource.volume;
        public float MusicVolume => _musicAudioSource.volume;
        
        
        private void Awake()
        {
            _soundAudioSource.volume = PlayerPrefsManager.GetSoundVolume();
            _musicAudioSource.volume = PlayerPrefsManager.GetMusicVolume();
        }
        
        private void Reset()
        {
            if(_soundAudioSource == null)
            {
                _soundAudioSource = new GameObject("SoundAudioSource", typeof(AudioSource)).GetComponent<AudioSource>();
                _soundAudioSource.playOnAwake = false;
                _soundAudioSource.volume = PlayerPrefsManager.DefaultSoundVolume;
                _soundAudioSource.transform.SetParent(transform);
            }
            
            if(_musicAudioSource == null)
            {
                _musicAudioSource = new GameObject("MusicAudioSource", typeof(AudioSource)).GetComponent<AudioSource>();
                _musicAudioSource.playOnAwake = false;
                _musicAudioSource.volume = PlayerPrefsManager.DefaultMusicVolume;
                _musicAudioSource.transform.SetParent(transform);
            }
        }
        
        
        public void SetSoundVolume(float value)
        {
            _soundAudioSource.volume = value;
            PlayerPrefsManager.SaveSoundVolume(value);
        }

        public void SetMusicVolume(float value)
        {
            _musicAudioSource.volume = value;
            PlayerPrefsManager.SaveMusicVolume(value);
        }

        public void PlaySound(AudioClip clip)
        {
            _soundAudioSource.clip = clip;
            _soundAudioSource.Play();
        }
        
        public void PlayMusic(AudioClip clip)
        {
            StartCoroutine(PlayMusicRoutine(clip));
        }
        
        
        private IEnumerator PlayMusicRoutine(AudioClip clip)
        {
            var vol = PlayerPrefsManager.GetMusicVolume();
            _musicAudioSource.volume = 0;
            _musicAudioSource.DOFade(vol, FadeDuration);
            
            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
            yield return new WaitForSeconds(clip.length + DelayBetweenMusicClips);
            OnMusicEnded?.Invoke();
        }
    }
}