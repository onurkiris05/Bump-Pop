using Game.Manager;
using UnityEngine;

namespace Game.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallBase : MonoBehaviour
    {
        [SerializeField] private bool canSpawn;

        public bool CanSpawn => canSpawn;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            GameManager.Instance.InvokeOnBallSpawned(this);
        }

        public virtual void Launch(Vector3 direction, float force)
        {
            _rb.AddForce(direction * force, ForceMode.Impulse);
        }

        public virtual void Stop()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        public float GetMagnitude() => _rb.velocity.magnitude;

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out BallBase ball))
            {
                if (!canSpawn) return;
                canSpawn = false;
                _rb.useGravity = true;

                var dir = (transform.position - ball.transform.position).normalized;
                GameManager.Instance.InvokeOnSpawnBurst(this, dir);
            }
        }
    }
}