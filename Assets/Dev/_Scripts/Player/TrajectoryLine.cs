using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    
    #region UNITY EVENTS

    private void OnEnable() => lineRenderer = GetComponent<LineRenderer>();

    #endregion

    #region LINE METHODS

    public void Set(Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public void Kill() => Destroy(gameObject);

    #endregion
}