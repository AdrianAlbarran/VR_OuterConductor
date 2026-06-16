using UnityEngine;

public class InstrumentTrembler : MonoBehaviour
{
    [SerializeField] private float posAmplitude = 0.005f;
    [SerializeField] private float rotAmplitude = 1.5f;
    [SerializeField] private float speed = 12f;

    private bool active = false;
    private Vector3 baseLocalPos;
    private Quaternion baseLocalRot;
    private float offset;

    public void StartTrembling()
    {
        baseLocalPos = transform.localPosition;
        baseLocalRot = transform.localRotation;
        offset = Random.Range(0f, 100f);
        active = true;
    }

    public void StopTrembling()
    {
        active = false;
        transform.localPosition = baseLocalPos;
        transform.localRotation = baseLocalRot;
    }

    void Update()
    {
        if (!active) return;
        float t = Time.time * speed + offset;
        transform.localPosition = baseLocalPos + new Vector3(
            Mathf.Sin(t * 1.3f) * posAmplitude,
            Mathf.Sin(t * 0.9f) * posAmplitude,
            Mathf.Sin(t * 1.7f) * posAmplitude
        );
        transform.localRotation = baseLocalRot * Quaternion.Euler(
            Mathf.Sin(t * 1.1f) * rotAmplitude,
            Mathf.Sin(t * 0.8f) * rotAmplitude,
            Mathf.Sin(t * 1.4f) * rotAmplitude
        );
    }
}