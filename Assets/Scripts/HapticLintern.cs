using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HapticLinterna : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(ActivateFlashlight);
    }

    void ActivateFlashlight(BaseInteractionEventArgs args)
    {
        Debug.Log("ASDASD");
        if(args.interactorObject is XRBaseInputInteractor interactor)
        {
            interactor.SendHapticImpulse(0.5f, 1);
        }
    }
}
