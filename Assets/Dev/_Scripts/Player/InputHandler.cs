using UnityEngine;
using Lean.Touch;

namespace Game.Player
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private float fingerMoveTolerance = 30f;

        private PlayerController _player;
        private LeanFinger _finger;
        private Vector2 _firstPos;

        #region UNITY EVENTS

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += HandleFingerDown;
            LeanTouch.OnFingerUpdate += HandleFingerUpdate;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= HandleFingerDown;
            LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
            LeanTouch.OnFingerUp -= HandleFingerUp;
        }

        #endregion

        #region PUBLIC METHODS

        public void Init(PlayerController player) => _player = player;

        #endregion

        #region PRIVATE METHODS

        private void HandleFingerDown(LeanFinger touchedFinger)
        {
            if (_finger == null)
            {
                _finger = touchedFinger;
                _firstPos = _finger.ScreenPosition;
            }
        }

        private void HandleFingerUpdate(LeanFinger movedFinger)
        {
            if (movedFinger == _finger && IsMoved(movedFinger))
            {
                _player.OnHold(GetDirection());
            }
        }

        private void HandleFingerUp(LeanFinger lostFinger)
        {
            if (lostFinger == _finger)
            {
                _player.OnRelease(GetDirection());

                _finger = null;
                _firstPos = Vector2.zero;
            }
        }

        private Vector3 GetDirection()
        {
            var rawDir = (_firstPos - _finger.ScreenPosition).normalized;
            var dir = new Vector3(rawDir.x, 0f, rawDir.y);
            return dir;
        }

        private bool IsMoved(LeanFinger movedFinger)
        {
            var dist = (movedFinger.LastScreenPosition - movedFinger.StartScreenPosition).sqrMagnitude;
            return dist > fingerMoveTolerance.Sqr();
        }

        #endregion
    }
}