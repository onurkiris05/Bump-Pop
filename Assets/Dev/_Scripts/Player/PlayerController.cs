using UnityEngine;
using Game.Ball;
using Game.Manager;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float lauchForce = 5f;

        private BallBase _ball;
        private InputHandler _inputHandler;
        private TrajectoryLineHandler _trajectoryLineHandler;

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _trajectoryLineHandler = GetComponent<TrajectoryLineHandler>();

            _inputHandler.Init(this);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnLeadBallUpdate += SetBall;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLeadBallUpdate -= SetBall;
        }


        public void OnHold(Vector3 dir)
        {
            _trajectoryLineHandler.DrawTrajectoryLine(_ball.transform.position, dir);
        }

        public void OnRelease(Vector3 dir)
        {
            _ball.Launch(dir, lauchForce);
            _trajectoryLineHandler.ResetTrajectoryLine();
            GameManager.Instance.ChangeState(GameState.BallReleased);
            print($"Released");
        }

        private void SetBall(BallBase ball) => _ball = ball;
    }
}