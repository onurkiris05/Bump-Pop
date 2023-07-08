using Game.Manager;
using UnityEngine;

namespace Game.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallBase : MonoBehaviour
    {
        [SerializeField] protected bool canSpawn;

        public bool CanSpawn => canSpawn;
        public bool IsTriggered => _isTriggered;

        protected Rigidbody _rb;
        protected bool _isTriggered;

        protected void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected void OnEnable()
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
                _isTriggered = true;
                
                if (!canSpawn) return;
                canSpawn = false;
                _rb.useGravity = true;

                // Instead of using dir, transform.forward makes gameplay more juicy
                // var dir = (transform.position - ball.transform.position).normalized;
                GameManager.Instance.InvokeOnSpawnBurst(this);
            }
        }
    }
}