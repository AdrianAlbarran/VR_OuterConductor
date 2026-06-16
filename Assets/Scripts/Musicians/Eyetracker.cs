using UnityEngine;

public class EyeTracker : MonoBehaviour
{
    [SerializeField] private Transform[] eyes;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;

    void Update()
    {
        if (target == null) return;
        foreach (var eye in eyes)
        {
            if (eye == null) continue;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - eye.position)
                            * Quaternion.Euler(0f, 180f, 0f);
            eye.rotation = Quaternion.Slerp(eye.rotation, targetRotation, Time.deltaTime * speed);
        }
    }
}