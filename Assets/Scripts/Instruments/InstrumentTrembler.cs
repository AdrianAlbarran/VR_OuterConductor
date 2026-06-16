using UnityEngine;

public class InstrumentTrembler : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float posAmplitude = 0.005f;
    [SerializeField] private float rotAmplitude = 1.5f;
    [SerializeField] private float speed = 12f;

    private bool active = false;
    private Vector3 baseLocalPos;
    private Quaternion baseLocalRot;
    private float offset;

    void Awake()
    {
        if (targetTransform == null) targetTransform = transform;
    }

    public void StartTrembling()
    {
        if (targetTransform == null) targetTransform = transform;
        baseLocalPos = targetTransform.localPosition;
        baseLocalRot = targetTransform.localRotation;
        offset = Random.Range(0f, 100f);
        active = true;
    }

    public void StopTrembling()
    {
        active = false;
        if (targetTransform != null)
        {
            targetTransform.localPosition = baseLocalPos;
            targetTransform.localRotation = baseLocalRot;
        }
    }

    void Update()
    {
        if (!active) return;
        float t = Time.time * speed + offset;
        targetTransform.localPosition = baseLocalPos + new Vector3(
            Mathf.Sin(t * 1.3f) * posAmplitude,
            Mathf.Sin(t * 0.9f) * posAmplitude,
            Mathf.Sin(t * 1.7f) * posAmplitude);
        targetTransform.localRotation = baseLocalRot * Quaternion.Euler(
            Mathf.Sin(t * 1.1f) * rotAmplitude,
            Mathf.Sin(t * 0.8f) * rotAmplitude,
            Mathf.Sin(t * 1.4f) * rotAmplitude);
    }
}