using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NKLogger;
using Southbyte.DIConfiguration;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Southbyte.AudioSystem
{
    [EarlyInitialization]
    public class SoundManager : AsyncInitializationServiceBase
    {
        private const string SoundsResourcesAddressablesPath = "SoundsResources";
        
        [Inject] private AudioManager _audioManager;

        private SoundsResources _soundsResources;
        private AsyncOperationHandle<SoundsResources> _handle;
        private readonly Dictionary<SoundType, AudioClip> _cachedSounds = new ();

        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            await InitAsync();
            await InitializationTask;
        }
        

        public AudioClip GetAudioClip(SoundType soundType)
        {
            if(!_cachedSounds.TryGetValue(soundType, out var cachedAudioClip))
            {
                cachedAudioClip = _soundsResources.GetAudioClip(soundType);

                if(cachedAudioClip != null)
                    _cachedSounds.Add(soundType, cachedAudioClip);
            }

            return cachedAudioClip;
        }
        
        
        private async UniTask InitAsync()
        {
            _handle = Addressables.LoadAssetAsync<SoundsResources>(SoundsResourcesAddressablesPath);
            _handle.Completed += OnSoundsResourcesLoaded;
            await _handle.Task;
        }
        
        private void OnSoundsResourcesLoaded(AsyncOperationHandle<SoundsResources> result)
        {
            if(result.Status == AsyncOperationStatus.Succeeded)
            {
                _handle.Completed -= OnSoundsResourcesLoaded;
                _soundsResources = result.Result;
                TrySetInitializationResult(true);
                return;
            }
            
            _handle = Addressables.LoadAssetAsync<SoundsResources>(SoundsResourcesAddressablesPath);
            DebugPro.LogError($"{nameof(SoundsResources)} can't be loaded by path: {SoundsResourcesAddressablesPath}");
        }
    }
}