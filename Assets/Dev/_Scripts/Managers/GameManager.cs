using System;
using Game.Ball;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action<BallBase> OnLeadBallUpdate;
        public event Action<BallBase> OnBallSpawned;
        public event Action<BallBase, Vector3> OnSpawnBurst;
        public event Action OnBallReady;
        public event Action OnInitializeLevel;
        public event Action<Transform> OnLevelComplete;
        public event Action OnLevelFailed;
        public event Action OnNextLevel;

        public GameState State { get; private set; }

        private void Start()
        {
            OnInitializeLevel?.Invoke();
            ChangeState(GameState.BallReady);
        }

        public void ChangeState(GameState newState)
        {
            if (newState == State) return;
            State = newState;
            print($"Game state changed to: {State}");

            switch (newState)
            {
                case GameState.BallReady:
                    OnBallReady?.Invoke();
                    break;
                case GameState.BallReleased:
                    break;
                case GameState.LevelComplete:
                    break;
                case GameState.LevelFailed:
                    OnLevelFailed?.Invoke();
                    break;
            }
        }

        public void LoadNextLevel()
        {
            SceneController.Instance.LoadNextScene();
        }

        public void RestartScene()
        {
            SceneController.Instance.RestartScene();
        }

        public void InvokeOnLeadBallUpdate(BallBase ball) => OnLeadBallUpdate?.Invoke(ball);
        public void InvokeOnBallSpawned(BallBase ball) => OnBallSpawned?.Invoke(ball);
        public void InvokeOnSpawnBurst(BallBase ball, Vector3 dir) => OnSpawnBurst?.Invoke(ball, dir);

        public void InvokeOnLevelComplete(Transform finalPos)
        {
            ChangeState(GameState.LevelComplete);
            OnLevelComplete?.Invoke(finalPos);
        }

        public void InvokeOnNextLevel() => OnNextLevel?.Invoke();
    }

    public enum GameState
    {
        LevelComplete,
        LevelFailed,
        BallReleased,
        BallReady
    }
}