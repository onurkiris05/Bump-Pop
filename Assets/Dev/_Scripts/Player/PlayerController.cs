using System.Collections;
using UnityEngine;
using Game.Ball;
using Game.Manager;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float lauchForce = 5f;

        private BallBase _currentBall;
        private BallBase _lastBall;
        private InputHandler _inputHandler;
        private TrajectoryLineHandler _trajectoryLineHandler;

        #region UNITY EVENTS

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _trajectoryLineHandler = GetComponent<TrajectoryLineHandler>();

            _inputHandler.Init(this);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnLeadBallUpdate += SetBall;
            GameManager.Instance.OnBallReady += CheckBall;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLeadBallUpdate -= SetBall;
            GameManager.Instance.OnBallReady += CheckBall;
        }

        #endregion

        #region PUBLIC METHODS

        public void OnHold(Vector3 dir)
        {
            if (GameManager.Instance.State != GameState.BallReady) return;

            _trajectoryLineHandler.DrawTrajectoryLine(_currentBall.transform.position, dir);
        }

        public void OnRelease(Vector3 dir)
        {
            if (GameManager.Instance.State != GameState.BallReady) return;

            _currentBall.Launch(dir, lauchForce);
            _trajectoryLineHandler.ResetTrajectoryLine();
            print($"Ball released!");

            StartCoroutine(ProcessOnRelease());
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator ProcessOnRelease()
        {
            yield return Helpers.BetterWaitForSeconds(0.2f);
            GameManager.Instance.ChangeState(GameState.BallReleased);
        }

        private void SetBall(BallBase ball) => _currentBall = ball;

        private void CheckBall()
        {
            if (_currentBall == null) return;

            if (_lastBall != _currentBall)
            {
                _lastBall = _currentBall;
            }
            else if (!_lastBall.IsTriggered)
            {
                _lastBall = null;
                GameManager.Instance.ChangeState(GameState.LevelFailed);
            }
        }

        #endregion
    }
}