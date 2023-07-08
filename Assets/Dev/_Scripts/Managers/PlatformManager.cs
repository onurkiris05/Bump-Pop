using Game.Ball;
using TMPro;
using UnityEngine;

namespace Game.Manager
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private TextMeshPro ballCountText;
        [SerializeField] private float winnerBallTimeout = 3f;

        private int _winnerBallCount;
        private float _winnerBallTimeoutTimer;
        private bool _isTriggered;

        private void Start()
        {
            ballCountText.text = $"{_winnerBallCount}";
        }

        private void Update()
        {
            if (_winnerBallCount <= 0) return;

            if (!_isTriggered)
            {
                _isTriggered = true;
                GameManager.Instance.InvokeOnLevelComplete(ballCountText.transform);
            }

            _winnerBallTimeoutTimer += Time.deltaTime;
            if (_winnerBallTimeoutTimer >= winnerBallTimeout)
            {
                GameManager.Instance.InvokeOnNextLevel();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BallBase ball))
            {
                _winnerBallCount++;
                _winnerBallTimeoutTimer = 0f;
                ballCountText.text = $"{_winnerBallCount}";
            }
        }
    }
}