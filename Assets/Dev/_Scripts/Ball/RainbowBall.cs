using Game.Manager;
using UnityEngine;

namespace Game.Ball
{
    public class RainbowBall : BallBase
    {
        #region PUBLIC METHODS

        public virtual void SetMaterial(Material mat)
        {
            _meshRenderer.material = mat;
        }

        #endregion
        
        #region UNITY EVENTS

        protected override void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out BallBase ball))
            {
                _isTriggered = true;

                if (!canSpawn) return;
                canSpawn = false;
                _rb.isKinematic = false;
                _rb.useGravity = true;

                GameManager.Instance.InvokeOnSpawnBurst(this, BurstType.Rainbow);
                GameManager.Instance.InvokeOnBallKill(this);
                VFXManager.Instance.PlayVFX(VFXType.MysticExplosionRainbow, transform.position);
                Destroy(gameObject, 1f);
            }
        }

        #endregion
    }
}