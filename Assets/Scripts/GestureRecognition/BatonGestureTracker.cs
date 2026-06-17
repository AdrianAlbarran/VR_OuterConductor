using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class BatonGestureTracker : MonoBehaviour
{
    [Header("Tracker config")]
    [SerializeField] private int m_bufferSize = 10; // Number of frames to store
    [SerializeField] private float m_VelocityThreshold = 3f; // Minimum sensibility of the movement
    [SerializeField] private InputActionReference m_recordMovement;
    [SerializeField] private int m_referenceFrequency;
    [SerializeField] private float m_tempoModifier; // Positive, between 1 and 10

    private List<Vector3> _positionHistory = new List<Vector3>();
    private Vector3 _lastPosition;

    private List<Direction> _directionHistory = new List<Direction>();
    private Vector3 _lastDirection = Vector3.zero;

    private float _transcurredTime;

    public UnityEvent<float> changeTempo;
    

    private struct Direction
    {
        public Vector3 direction;
        public float time;

        public Direction(Vector3 _direction, float _time)
        {
            direction = _direction;
            time = _time;
        }
    }

    private void Start()
    {
        _lastPosition = transform.position;

        m_recordMovement.action.started += (CallbackContext) => { _directionHistory.Clear(); };
        m_recordMovement.action.started += (CallbackContext) => { _transcurredTime = Time.time; };
        m_recordMovement.action.canceled += InterpretDirectionHistory;
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
        if (velocity.magnitude > m_VelocityThreshold)
        {
            Vector3 dominantDirection = InterpretDirection(velocity);

            if (m_recordMovement.action.IsPressed() && _lastDirection != Vector3.zero)
            {
                if (_lastDirection.y == -dominantDirection.y && _lastDirection.y != 0)
                {
                    Direction dir = new Direction(dominantDirection, (Time.time - _transcurredTime));
                    _directionHistory.Add(dir);
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

    private void InterpretDirectionHistory(InputAction.CallbackContext context)
    {
        if (_directionHistory.Count == 0)
            return;

        float delta = 0;
        float lastTime = _directionHistory[0].time;
        int movementsCount = 0;

        for (int i = 0; i < _directionHistory.Count; i++)
        {
            delta = _directionHistory[i].time - lastTime;

            if (delta < 0.8f)
            {
                movementsCount++;
            }
            
            lastTime = _directionHistory[i].time;
        }


        int tempo = movementsCount - m_referenceFrequency;
        float modifyAmount = tempo * m_tempoModifier;
        changeTempo.Invoke(modifyAmount);
    }


}
