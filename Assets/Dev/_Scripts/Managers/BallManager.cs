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
        [Header("Ball Settings")]
        [SerializeField] private int spawnRate = 10;
        [SerializeField] private float spawnBurstForce = 15f;
        [SerializeField] private Material[] ballMaterials;

        public List<BallBase> Balls = new List<BallBase>();

        private BallBase _leadBall;
        private BallBase _lastBall;
        private Vector3 _lastBallPos;
        private float _cooldownTimer;

        #region UNITY EVENTS

        private void Awake()
        {
            GameManager.Instance.OnBallSpawned += AddToList;
            GameManager.Instance.OnBallKill += RemoveFromList;
            GameManager.Instance.OnSpawnBurst += ProcessSpawnBurst;
            GameManager.Instance.OnInitializeLevel += UpdateLeadBall;
            GameManager.Instance.OnNextLevel += FrezeeBalls;
            GameManager.Instance.OnLevelFailed += FrezeeBalls;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnBallSpawned -= AddToList;
            GameManager.Instance.OnBallKill -= RemoveFromList;
            GameManager.Instance.OnSpawnBurst -= ProcessSpawnBurst;
            GameManager.Instance.OnInitializeLevel -= UpdateLeadBall;
            GameManager.Instance.OnNextLevel -= FrezeeBalls;
            GameManager.Instance.OnLevelFailed -= FrezeeBalls;
        }

        private void Update()
        {
            if (GameManager.Instance.State == GameState.LevelComplete) return;
            UpdateLeadBall();

            if (GameManager.Instance.State != GameState.BallReleased) return;
            TrackLeadBall(_leadBall);
        }

        #endregion

        #region TRACK METHODS

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

        #endregion

        #region BURST METHODS

        private void ProcessSpawnBurst(BallBase ball, BurstType burstType)
        {
            var spawnPos = ball.transform.position;
            var dir = ball.transform.forward;

            switch (burstType)
            {
                case BurstType.Standart:
                    NormalBurst(ball as StandartBall, spawnPos, dir);
                    break;
                case BurstType.Rainbow:
                    RainbowBurst(ball as RainbowBall, spawnPos, dir);
                    break;
                case BurstType.Golden:
                    GoldenBurst(ball as GoldenBall, spawnPos, dir);
                    break;
            }
        }

        private void NormalBurst(StandartBall ball, Vector3 spawnPos, Vector3 dir)
        {
            if (ball == null) return;

            for (int i = 0; i < spawnRate; i++)
            {
                StandartBall newBall = Instantiate(ball, spawnPos, Quaternion.identity);
                newBall.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear);
                newBall.Launch(dir, spawnBurstForce);
                spawnPos += RandomizeDirection(dir) / 10f;

                GameManager.Instance.InvokeOnBallSpawned(newBall);
            }

            AudioManager.Instance.PlayStandartBallHitSFX(spawnRate / 2);
            ball.Launch(dir, spawnBurstForce);
        }

        private void RainbowBurst(RainbowBall ball, Vector3 spawnPos, Vector3 dir)
        {
            if (ball == null) return;

            for (int i = 0; i < spawnRate * 2; i++)
            {
                RainbowBall newBall = Instantiate(ball, spawnPos, Quaternion.identity);
                newBall.SetMaterial(ballMaterials[i % ballMaterials.Length]);
                newBall.transform.DOScale(Vector3.zero, 0.3f).From().SetEase(Ease.Linear);
                newBall.Launch(dir, spawnBurstForce);
                spawnPos += RandomizeDirection(dir) / 10f;

                GameManager.Instance.InvokeOnBallSpawned(newBall);
            }

            AudioManager.Instance.PlayStandartBallHitSFX(spawnRate / 2);
        }

        private void GoldenBurst(GoldenBall ball, Vector3 spawnPos, Vector3 dir)
        {
            if (ball == null) return;

            for (int i = 0; i < spawnRate; i++)
            {
                GoldenBall newBall = Instantiate(ball, spawnPos, Quaternion.identity);
                newBall.transform.DOScale(newBall.transform.localScale * 1.5f, 0.3f)
                    .SetEase(Ease.Linear);
                newBall.Launch(dir, spawnBurstForce);
                spawnPos += RandomizeDirection(dir) / 10f;

                GameManager.Instance.InvokeOnBallSpawned(newBall);
            }

            AudioManager.Instance.PlayStandartBallHitSFX(spawnRate / 2);
        }

        #endregion

        #region PRIVATE METHODS

        private void AddToList(BallBase ball)
        {
            Balls.Add(ball);
        }

        private void RemoveFromList(BallBase ball)
        {
            Balls.Remove(ball);
        }

        private void UpdateLeadBall()
        {
            _leadBall = GetLeadBall();
            if (_lastBall != _leadBall)
            {
                _lastBall = _leadBall;
                _lastBallPos = _lastBall.transform.position;
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

        #endregion
    }

    public enum BurstType
    {
        Standart,
        Rainbow,
        Golden
    }
}