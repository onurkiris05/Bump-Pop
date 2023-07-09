using System.Collections;
using UnityEngine;

namespace Game.Manager
{
    public class VFXManager : StaticInstance<VFXManager>
    {
        [Header("VFX Settings")]
        [SerializeField] private ParticleSystem mysticExplosionRainbow;
        [SerializeField] private ParticleSystem flashExplosionYellow;

        #region PUBLIC METHODS

        public void PlayVFX(VFXType type, Vector3 pos)
        {
            switch (type)
            {
                case VFXType.MysticExplosionRainbow:
                    StartCoroutine(ProcessVFX(mysticExplosionRainbow, pos));
                    break;
                case VFXType.FlashExplosionYellow:
                    StartCoroutine(ProcessVFX(flashExplosionYellow, pos));
                    break;
            }
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator ProcessVFX(ParticleSystem vfx, Vector3 pos)
        {
            var newVFX = Instantiate(vfx, pos, Quaternion.identity, transform);
            yield return Helpers.BetterWaitForSeconds(newVFX.main.duration);
            Destroy(newVFX.gameObject);
        }

        #endregion
    }

    public enum VFXType
    {
        MysticExplosionRainbow,
        FlashExplosionYellow
    }
}