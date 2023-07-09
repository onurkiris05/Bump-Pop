using Game.Manager;
using UnityEngine;

namespace Game.Ball
{
    public class GoldenBall : BallBase
    {
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

                GameManager.Instance.InvokeOnSpawnBurst(this, BurstType.Golden);
                GameManager.Instance.InvokeOnBallKill(this);
                VFXManager.Instance.PlayVFX(VFXType.FlashExplosionYellow, transform.position);
                Destroy(gameObject);
            }
        }

        #endregion
    }
}