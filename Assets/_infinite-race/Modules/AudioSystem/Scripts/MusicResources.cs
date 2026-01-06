using System.Collections.Generic;
using UnityEngine;

namespace Southbyte.AudioSystem
{
    [CreateAssetMenu(fileName = "MusicResources", menuName = EditorMenuNames.AudioRoot + "Music Resources")]
    public class MusicResources : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _musics;
        [SerializeField] private AudioClip _firstRequiredMusicClip;
        
        public List<AudioClip> Musics => _musics;
        public AudioClip FirstRequiredMusicClip => _firstRequiredMusicClip;
    }
}