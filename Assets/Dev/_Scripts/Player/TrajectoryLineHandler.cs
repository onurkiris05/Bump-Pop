using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class TrajectoryLineHandler : MonoBehaviour
    {
        [Header("Trajectory Settings")]
        [SerializeField] private TrajectoryLine trajectoryLinePrefab;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] int maxReflectionCount = 5;
        [SerializeField] float maxDistance = 200;

        private List<TrajectoryLine> _trajectoryLines = new List<TrajectoryLine>();
        private int _currentReflectionCount;
        private float _currentDistance;
        private bool _isTrajectoryDrawn;
        private bool _isEndOfLine;

        #region UNITY EVENTS

        private void Start()
        {
            _currentReflectionCount = maxReflectionCount;
            _currentDistance = maxDistance;
        }

        #endregion

        #region PUBLIC METHODS

        public void DrawTrajectoryLine(Vector3 position, Vector3 direction)
        {
            // If there is old path, delete lines before starting new ones
            ResetTrajectoryLine();

            // Clear drawn lines if reflection count reached or end of line
            if (_currentReflectionCount <= 0 || _isEndOfLine)
            {
                _currentReflectionCount = maxReflectionCount;
                _currentDistance = maxDistance;
                _isTrajectoryDrawn = true;
                _isEndOfLine = false;
                return;
            }

            // Calculate if there is any deflect in direction
            var startingPosition = position;
            var ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.SphereCast(ray, 0.2f, out hit, _currentDistance, targetLayer))
            {
                // Align direction's rotation with trajectory helper object
                direction = new Vector3(direction.x, -hit.normal.y, direction.z);
                position = hit.point;
                _currentDistance -= Vector3.Distance(startingPosition, position);
            }
            else
            {
                // If there is no obstacle, just draw whole line
                position += direction * _currentDistance;
                _isEndOfLine = true;
            }

            // Create line renderer
            TrajectoryLine line = Instantiate(trajectoryLinePrefab, transform);
            line.Set(startingPosition, position);
            _trajectoryLines.Add(line);
            _currentReflectionCount--;

            DrawTrajectoryLine(position, direction);
        }

        public void ResetTrajectoryLine()
        {
            if (_isTrajectoryDrawn)
            {
                for (int i = 0; i < _trajectoryLines.Count; i++) _trajectoryLines[i].Kill();

                _trajectoryLines.Clear();
                _isTrajectoryDrawn = false;
            }
        }

        #endregion
    }
}