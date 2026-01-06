using System.Collections.Generic;
using NKLogger;
using Southbyte.AudioSystem;
using UnityEngine;

namespace Southbyte
{
    [CreateAssetMenu(fileName = "SoundsResources", menuName = EditorMenuNames.AudioRoot + "Sounds Resources")]
    public class SoundsResources : ScriptableObject
    {
        [SerializeField] private List<SerializablePair<SoundType, AudioClip>> _sounds;


        public AudioClip GetAudioClip(SoundType soundType)
        {
            foreach(var pair in _sounds)
            {
                if(pair.Key == soundType)
                    return pair.Value;
            }

            DebugPro.LogError($"Unable to find sound by type: [{soundType}]!", this);
            return null;
        }
    }
}