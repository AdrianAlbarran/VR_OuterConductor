using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatonGestureTracker : MonoBehaviour
{
    [Header("Tracker config")]
    [SerializeField] private int m_bufferSize = 10; // Number of frames to store
    [SerializeField] private float m_VelocityThreshold = 3f; // Minimum sensibility of the movement
    [SerializeField] private InputActionReference m_recordMovement;

    private List<Vector3> _positionHistory = new List<Vector3>();
    private Vector3 _lastPosition;

    private List<Vector3> _directionHistory = new List<Vector3>();
    private Vector3 _lastDirection = Vector3.zero;

    private void Start()
    {
        _lastPosition = transform.position;

        m_recordMovement.action.started += (CallbackContext) => { _directionHistory.Clear(); };
        m_recordMovement.action.canceled += (CallbackContext) => { foreach (var dir in _directionHistory) { Debug.Log(dir); } };

    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;

        // Calculate current velocity of the Baton
        Vector3 velocity = (currentPosition - _lastPosition) / Time.deltaTime;
        _lastPosition = currentPosition;

        // Update circular buffer
        _positionHistory.Add(currentPosition);
        if(_positionHistory.Count > m_bufferSize)
        {
            _positionHistory.RemoveAt(0);
        }

        // Analyze the direction if the movement is fast enough
        if(velocity.magnitude > m_VelocityThreshold)
        {
            Vector3 dominantDirection = InterpretDirection(velocity);
            Debug.Log($"<color=yellow>[BATUTA]</color> Moviéndose hacia: <b>{dominantDirection}</b> | Velocidad: {velocity.magnitude:F2} m/s");

            if (m_recordMovement.action.IsPressed() && _lastDirection != Vector3.zero)
            {
                if(_lastDirection.y == -dominantDirection.y && _lastDirection.y != 0)
                {
                    Debug.Log($"<color=yellow>[BATUTA]</color> Añado movimiento {dominantDirection} comparado con {_lastDirection}");
                    _directionHistory.Add(dominantDirection);
                }
            }

            _lastDirection = dominantDirection;
        }
    }

    private Vector3 InterpretDirection(Vector3 rawVelocity)
    {
        Vector3 normalized = rawVelocity.normalized;

        // Check which axies has the higher absolute value
        if(Mathf.Abs(normalized.x) > Mathf.Abs(normalized.y) && 
           Mathf.Abs(normalized.x) > Mathf.Abs(normalized.z))
        {
            return normalized.x > 0 ? Vector3.right: Vector3.left;
        }
        else if (Mathf.Abs(normalized.y) > Mathf.Abs(normalized.x) &&
                 Mathf.Abs(normalized.y) > Mathf.Abs(normalized.z))
        {
            return normalized.y > 0 ? Vector3.up: Vector3.down;
        }
        else
        {
            return normalized.z > 0 ? Vector3.forward : Vector3.back;
        }
    }

}
