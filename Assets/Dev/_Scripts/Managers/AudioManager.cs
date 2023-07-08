using UnityEngine;

namespace Game.Manager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : StaticInstance<AudioManager>
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}