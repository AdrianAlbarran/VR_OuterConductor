using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private float movementThreshold = 0.01f;

    private Vector3 lastPosition;
    private float stepTimer;

    void Start()
    {
        lastPosition = xrOrigin.position;
        stepTimer = stepInterval;
    }

    void Update()
    {
        Vector3 currentPos = xrOrigin.position;
        float speed = (currentPos - lastPosition).magnitude / Time.deltaTime;
        lastPosition = currentPos;

        if (speed > movementThreshold)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                AkUnitySoundEngine.PostEvent("Play_Walk", gameObject);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepInterval;
        }
    }
}