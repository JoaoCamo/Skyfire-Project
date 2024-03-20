using UnityEngine;

namespace Game.Audio
{
    [CreateAssetMenu]
    public class SoundEffects : ScriptableObject
    {
        public SoundEffect[] soundEffects;
    }

    [System.Serializable]
    public struct SoundEffect
    {
        public AudioClip[] audioClips;
        public float volume;
    }
}