using System.Collections.Generic;
using System.Linq;
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

        private void Awake()
        {
            GameManager.Instance.OnBallSpawned += AddToList;
            GameManager.Instance.OnSpawnBurst += SpawnBurst;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnBallSpawned -= AddToList;
            GameManager.Instance.OnSpawnBurst -= SpawnBurst;
        }

        private void Start()
        {
            GameManager.Instance.InvokeOnLeadBallUpdate(GetLeadBall());
        }

        private void Update()
        {
            GameManager.Instance.InvokeOnLeadBallUpdate(GetLeadBall());
        }

        private void SpawnBurst(BallBase ball, Vector3 dir)
        {
            for (int i = 0; i < spawnRate; i++)
            {
                var randomAngle = Random.Range(-10f, 10f);
                var randomRotation = Quaternion.Euler(0f, randomAngle, 0f);
                var randomDir = randomRotation * dir;

                BallBase newBall = Instantiate(ball, ball.transform.position, Quaternion.identity);
                newBall.Launch(randomDir, spawnBurstForce);

                GameManager.Instance.InvokeOnBallSpawned(newBall);
            }

            ball.Launch(dir, spawnBurstForce);
        }

        private void AddToList(BallBase ball)
        {
            Balls.Add(ball);
        }

        private BallBase GetLeadBall()
        {
            var leadBall = Balls
                .Where(ball => !ball.CanSpawn)
                .OrderByDescending(ball => ball.transform.position.z)
                .FirstOrDefault();

            return leadBall;
        }
    }
}