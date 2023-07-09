using System;
using Game.Ball;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action<BallBase> OnLeadBallUpdate;
        public event Action<BallBase> OnBallSpawned;
        public event Action<BallBase> OnBallKill;
        public event Action<BallBase, BurstType> OnSpawnBurst;
        public event Action OnBallReady;
        public event Action OnInitializeLevel;
        public event Action<Transform> OnLevelComplete;
        public event Action OnLevelFailed;
        public event Action OnNextLevel;
        public event Action<float> OnLevelProgressUpdate;

        public GameState State { get; private set; }

        #region UNITY EVENTS

        private void Start()
        {
            OnInitializeLevel?.Invoke();
            ChangeState(GameState.BallReady);
        }

        #endregion

        #region PUBLIC METHODS

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

        public void InvokeOnLevelComplete(Transform finalPos)
        {
            ChangeState(GameState.LevelComplete);
            OnLevelComplete?.Invoke(finalPos);
        }

        public void InvokeOnLeadBallUpdate(BallBase ball) => OnLeadBallUpdate?.Invoke(ball);
        public void InvokeOnBallSpawned(BallBase ball) => OnBallSpawned?.Invoke(ball);
        public void InvokeOnBallKill(BallBase ball) => OnBallKill?.Invoke(ball);
        public void InvokeOnSpawnBurst(BallBase ball, BurstType burstType) => OnSpawnBurst?.Invoke(ball, burstType);
        public void InvokeOnNextLevel() => OnNextLevel?.Invoke();
        public void InvokeOnLevelProgressUpdate(float progress) => OnLevelProgressUpdate?.Invoke(progress);

        #endregion
    }

    public enum GameState
    {
        LevelComplete,
        LevelFailed,
        BallReleased,
        BallReady
    }
}