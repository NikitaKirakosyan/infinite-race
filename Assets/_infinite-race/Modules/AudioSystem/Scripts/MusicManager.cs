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
    public class MusicManager : AsyncInitializationServiceBase
    {
        private const string MusicResourcesAddressablesPath = "MusicResources";
        
        [Inject] private AudioManager _audioManager;
        
        private MusicResources _musicResources;
        private List<AudioClip> _randomizedMusics;
        private AsyncOperationHandle<MusicResources> _handle;
        
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            await InitAsync();
            _audioManager.OnMusicEnded += PlayMusic;
            PlayFirstRequiredMusic();
            await InitializationTask;
        }
        
        
        private void PlayFirstRequiredMusic()
        {
            if(_musicResources.FirstRequiredMusicClip == null)
                return;
            
            var musicClip = _musicResources.FirstRequiredMusicClip;
            _audioManager.PlayMusic(musicClip);
        }
        
        private void PlayMusic()
        {
            if(_randomizedMusics.IsNullOrEmpty())
            {
                _randomizedMusics = _musicResources.Musics.GetRandomElements(_musicResources.Musics.Count);
                
                if(_randomizedMusics.IsNullOrEmpty())
                {
                    DebugPro.LogError("No randomized Musics found!");
                    return;
                }
            }
            
            var musicClip = _randomizedMusics[0];
            _randomizedMusics.RemoveAt(0);
            _audioManager.PlayMusic(musicClip);
        }
        
        private async UniTask InitAsync()
        {
            _handle = Addressables.LoadAssetAsync<MusicResources>(MusicResourcesAddressablesPath);
            _handle.Completed += OnMusicResourcesLoaded;
            await _handle.Task;
        }
        
        private void OnMusicResourcesLoaded(AsyncOperationHandle<MusicResources> result)
        {
            _handle.Completed -= OnMusicResourcesLoaded;
            
            if(result.Status == AsyncOperationStatus.Succeeded)
            {
                _musicResources = result.Result;
                TrySetInitializationResult(true);
                return;
            }
            
            DebugPro.LogError($"{nameof(MusicResources)} can't be loaded by path: {MusicResourcesAddressablesPath}");
        }
    }
}