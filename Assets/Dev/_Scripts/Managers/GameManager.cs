using System;
using Game.Ball;
using UnityEngine;

namespace Game.Manager
{
    public enum GameState
    {
        BallReady,
        BallReleased
    }

    public class GameManager : Singleton<GameManager>
    {
        public event Action<BallBase> OnLeadBallUpdate;
        public event Action<BallBase> OnBallSpawned;
        public event Action<BallBase, Vector3> OnSpawnBurst;

        public GameState State { get; private set; }

        private void Start()
        {
            State = GameState.BallReady;
        }

        public void ChangeState(GameState newState)
        {
            if (newState == State) return;
            State = newState;

            switch (newState)
            {
                case GameState.BallReady:
                    break;
                case GameState.BallReleased:
                    break;
            }
        }

        public void InvokeOnLeadBallUpdate(BallBase ball) => OnLeadBallUpdate?.Invoke(ball);

        public void InvokeOnBallSpawned(BallBase ball)
        {
            OnBallSpawned?.Invoke(ball);
        }

        public void InvokeOnSpawnBurst(BallBase ball, Vector3 dir) => OnSpawnBurst?.Invoke(ball, dir);
    }
}