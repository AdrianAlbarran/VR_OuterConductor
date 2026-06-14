using UnityEngine;
using UnityEngine.InputSystem;

public class MyHandController : MonoBehaviour
{
    [SerializeField] InputActionReference actionGrip;
    [SerializeField] InputActionReference actionTrigger;

    private Animator handAnimator;

    void Start()
    {
        actionGrip.action.performed += GripPress;
        actionTrigger.action.performed += TriggerPress;
        handAnimator = GetComponent<Animator>();
    }

    private void GripPress(InputAction.CallbackContext context)
    {
        handAnimator.SetFloat("Grip", context.ReadValue<float>());
    }

    private void TriggerPress(InputAction.CallbackContext context)
    {
        handAnimator.SetFloat("Trigger", context.ReadValue<float>());
    }
}
