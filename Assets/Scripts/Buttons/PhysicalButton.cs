using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhysicalButton : MonoBehaviour
{
    // Enter only one time in the trigger
    private bool m_pressed = false;
    private bool m_released = true;

    // Pressed position
    [SerializeField] private Transform m_pressedPosition;
    private Vector3 m_startPosition;

    // Unity events for the retrieval of the instrument
    public UnityEvent onPressured, onReleased;



    private void Start()
    {
        m_startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInChildren<XRBaseInteractor>()) return;
        
        if (!m_pressed)
        {
            m_pressed = true;
            m_released = false;

            var rb = other.attachedRigidbody;
            float speed = rb != null ? rb.linearVelocity.magnitude : 1f;

            int[] validVels = { 1, 5, 9, 11 };
            int velIndex = Mathf.Clamp(Mathf.RoundToInt(speed * 1.5f), 0, 3);
            int vel = validVels[velIndex];

            // Push button animation
            StartCoroutine(WaitForDeadTime((float)vel, m_pressedPosition.position, true));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.GetComponentInChildren<XRBaseInteractor>()) return;
        if (!m_released) return;
        StartCoroutine(WaitForDeadTime(1.5f, m_startPosition, false));
    }


    private IEnumerator WaitForDeadTime(float maxSpeed, Vector3 finalPosition, bool enter)
    {
        Vector3 startPosition = transform.position;
        float totalDistance = Vector3.Distance(startPosition, finalPosition);

        if (totalDistance < 0.0001f)
        {
            transform.position = finalPosition;
            yield break; // Free get out of jail card
        }

        float totalDuration = 0.3f;
        float elapsedTime = 0f;

        while(elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / totalDuration;

            transform.position = Vector3.Lerp(startPosition, finalPosition, t);

            yield return null;
        }

        transform.position = finalPosition;

        if(enter)
        {
            onPressured.Invoke();
            yield return new WaitForSeconds(0.4f);
            m_released = true;

        }
        else
        {
            onReleased.Invoke();
            yield return new WaitForSeconds(1f);
            m_pressed = false;
        }
    }

}
