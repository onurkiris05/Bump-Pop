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
            GameManager.Instance.OnLevelComplete += SetFinalPos;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLeadBallUpdate -= SetTarget;
            GameManager.Instance.OnLevelComplete -= SetFinalPos;
        }

        private void SetTarget(BallBase ball)
        {
            SetCam(ball.transform);
        }

        private void SetFinalPos(Transform finalPos)
        {
            SetCam(finalPos);
        }

        private void SetCam(Transform t)
        {
            _target = t;
            followCam.Follow = _target;
            followCam.LookAt = _target;
        }
    }
}