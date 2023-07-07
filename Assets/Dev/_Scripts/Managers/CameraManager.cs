using Cinemachine;
using Game.Ball;
using UnityEngine;

namespace Game.Manager
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCam;

        private Transform _target;

        private void OnEnable()
        {
            GameManager.Instance.OnLeadBallUpdate += SetTarget;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLeadBallUpdate -= SetTarget;
        }

        private void SetTarget(BallBase ball)
        {
            _target = ball.transform;
            followCam.Follow = _target;
            followCam.LookAt = _target;
        }
    }
}