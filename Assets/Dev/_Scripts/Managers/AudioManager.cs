using System.Collections;
using UnityEngine;

namespace Game.Manager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : StaticInstance<AudioManager>
    {
        [Header("SFX Settings")]
        [SerializeField] private AudioClip standartBallHitSFX;
        [SerializeField] private AudioClip winnerBallSFX;

        private AudioSource _audioSource;

        #region UNITY EVENTS

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }
        

        #endregion

        #region PUBLIC METHODS

        public void PlayStandartBallHitSFX(int repeatCount = 1)
        {
            StartCoroutine(ProcessMultipleSFX(standartBallHitSFX, repeatCount));
        }
        
        public void PlayWinnerBallSFX(int repeatCount = 1)
        {
            StartCoroutine(ProcessMultipleSFX(winnerBallSFX, repeatCount));
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator ProcessMultipleSFX(AudioClip sfx, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                _audioSource.PlayOneShot(sfx);
                yield return Helpers.BetterWaitForSeconds(0.05f);
            }
        }

        #endregion
    }
}