using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.AudioSystem
{
    public class SoundHandler : MonoBehaviour
    {
        [SerializeField] private SoundType _soundType = SoundType.Click;

        [Inject] private SoundManager _soundManager;
        [Inject] private AudioManager _audioManager;

        private Button _button;
        private Toggle _toggle;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _toggle = GetComponent<Toggle>();

            if(_button)
                _button.onClick.AddListener(PlaySound);
            
            if(_toggle)
                _toggle.onValueChanged.AddListener(_ => PlaySound());
        }


        private void PlaySound()
        {
            if(_audioManager.SoundVolume > 0f)
                _audioManager.PlaySound(_soundManager.GetAudioClip(_soundType));
        }
    }
}