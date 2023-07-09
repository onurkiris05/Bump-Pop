using Game.Manager;
using UnityEngine;

namespace Game.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BallBase : MonoBehaviour
    {
        [Header("Ball Settings")]
        [SerializeField] protected bool canSpawn;

        public bool CanSpawn => canSpawn;
        public bool IsTriggered => _isTriggered;

        protected Rigidbody _rb;
        protected MeshRenderer _meshRenderer;
        protected bool _isTriggered;

        #region UNITY EVENTS

        protected void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        protected void OnEnable()
        {
            GameManager.Instance.InvokeOnBallSpawned(this);
        }

        #endregion

        #region PUBLIC METHODS

        public virtual void Launch(Vector3 direction, float force)
        {
            _isTriggered = false;
            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.AddForce(direction * force, ForceMode.Impulse);
        }

        public virtual void Stop() => _rb.isKinematic = true;
        public float GetMagnitude() => _rb.velocity.magnitude;

        #endregion

        #region ABSTRACT METHODS

        protected abstract void OnCollisionEnter(Collision collision);

        #endregion
    }
}