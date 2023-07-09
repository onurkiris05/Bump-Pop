using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Ball;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Manager
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private int spawnRate = 10;
        [SerializeField] private float spawnBurstForce = 15f;

        public List<BallBase> Balls = new List<BallBase>();

        private BallBase _leadBall;
        private BallBase _lastBall;
        private Vector3 _lastBallPos;
        private float _cooldownTimer;

        private void Awake()
        {
            GameManager.Instance.OnBallSpawned += AddToList;
            GameManager.Instance.OnSpawnBurst += SpawnBurst;
            GameManager.Instance.OnInitializeLevel += UpdateLeadBall;
            GameManager.Instance.OnNextLevel += FrezeeBalls;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnBallSpawned -= AddToList;
            GameManager.Instance.OnSpawnBurst -= SpawnBurst;
            GameManager.Instance.OnInitializeLevel -= UpdateLeadBall;
            GameManager.Instance.OnNextLevel -= FrezeeBalls;
        }

        private void Update()
        {
            if (GameManager.Instance.State == GameState.LevelComplete) return;
            UpdateLeadBall();

            if (GameManager.Instance.State != GameState.BallReleased) return;
            TrackLeadBall(_leadBall);
        }

        private void TrackLeadBall(BallBase ball)
        {
            if (ball.transform.position.z < _lastBallPos.z - 1f)
            {
                ball.Stop();
                GameManager.Instance.ChangeState(GameState.LevelFailed);
                return;
            }

            _lastBallPos = ball.transform.position;

            TrackMagnitude(ball);
        }

        private void TrackMagnitude(BallBase ball)
        {
            if (ball.GetMagnitude() < 1f)
            {
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer < 0.5f) return;

                FrezeeBalls();
                GameManager.Instance.ChangeState(GameState.BallReady);
            }
            else
            {
                _cooldownTimer = 0f;
            }
        }

        private void SpawnBurst(BallBase ball)
        {
            var spawnPos = ball.transform.position;
            var dir = ball.transform.forward;

            for (int i = 0; i < spawnRate; i++)
            {
                BallBase newBall = Instantiate(ball, spawnPos, Quaternion.identity);
                newBall.transform.DOScale(Vector3.zero, 0.5f).From();
                newBall.Launch(dir, spawnBurstForce);
                spawnPos += RandomizeDirection(dir) / 10f;

                GameManager.Instance.InvokeOnBallSpawned(newBall);
            }

            ball.Launch(dir, spawnBurstForce);
        }

        private void AddToList(BallBase ball)
        {
            Balls.Add(ball);
        }

        private void UpdateLeadBall()
        {
            _leadBall = GetLeadBall();
            if (_lastBall != _leadBall)
            {
                _lastBall = _leadBall;
                GameManager.Instance.InvokeOnLeadBallUpdate(_leadBall);
            }
        }

        private BallBase GetLeadBall()
        {
            var leadBall = Balls
                .Where(ball => !ball.CanSpawn)
                .OrderByDescending(ball => ball.transform.position.z)
                .FirstOrDefault();

            return leadBall;
        }

        private Vector3 RandomizeDirection(Vector3 dir)
        {
            var randomAngle = Random.Range(-20f, 20f);
            var randomRotation = Quaternion.Euler(0f, randomAngle, 0f);
            return randomRotation * dir;
        }

        private void FrezeeBalls()
        {
            foreach (var ball in Balls)
            {
                ball.Stop();
            }
        }
    }
}