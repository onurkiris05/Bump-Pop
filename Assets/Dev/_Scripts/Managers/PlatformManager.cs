using Game.Ball;
using TMPro;
using UnityEngine;

namespace Game.Manager
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private TextMeshPro ballCountText;
        [SerializeField] private Transform finishTransform;
        [SerializeField] private float winnerBallTimeout = 3f;

        private BallBase _currentBall;
        private Vector3 _startPosition;
        private Vector3 _finishPosition;
        private int _winnerBallCount;
        private float _winnerBallTimeoutTimer;
        private bool _isTriggered;
        private bool _isNextLevel;

        private void OnEnable()
        {
            GameManager.Instance.OnLeadBallUpdate += UpdateBall;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLeadBallUpdate -= UpdateBall;
        }

        private void Start()
        {
            ballCountText.text = $"{_winnerBallCount}";
            _finishPosition = finishTransform.position;
        }

        private void Update()
        {
            if (_winnerBallCount <= 0)
            {
                TrackLevelProgress();
            }
            else
            {
                ProcessLevelEnd();
            }
        }

        private void UpdateBall(BallBase ball)
        {
            if (_startPosition == Vector3.zero)
                _startPosition = ball.transform.position;

            _currentBall = ball;
        }

        private void ProcessLevelEnd()
        {
            if (!_isTriggered)
            {
                _isTriggered = true;
                GameManager.Instance.InvokeOnLevelComplete(finishTransform);
                GameManager.Instance.InvokeOnLevelProgressUpdate(1f);
            }

            _winnerBallTimeoutTimer += Time.deltaTime;
            if (_winnerBallTimeoutTimer >= winnerBallTimeout)
            {
                _isNextLevel = true;
                GameManager.Instance.InvokeOnNextLevel();
            }
        }

        private void TrackLevelProgress()
        {
            var progress = (_currentBall.transform.position.z - _startPosition.z)
                           / (_finishPosition.z - _startPosition.z);
            GameManager.Instance.InvokeOnLevelProgressUpdate(progress);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BallBase ball))
            {
                if (_isNextLevel) return;

                _winnerBallCount++;
                _winnerBallTimeoutTimer = 0f;
                ballCountText.text = $"{_winnerBallCount}";
            }
        }
    }
}