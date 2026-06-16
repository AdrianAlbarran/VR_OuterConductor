using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class GrabTooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipCanvas;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private XRBaseInteractor interactor;
    [SerializeField] private string grabText = "Agarrar\nGrip";

    void Awake()
    {
        interactor.hoverEntered.AddListener(OnHoverEntered);
        interactor.hoverExited.AddListener(OnHoverExited);
        tooltipCanvas.SetActive(false);
    }

    void OnDestroy()
    {
        if (interactor == null) return;
        interactor.hoverEntered.RemoveListener(OnHoverEntered);
        interactor.hoverExited.RemoveListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetComponentInParent<Instrument>() != null)
            tooltipCanvas.SetActive(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (!HasInstrumentHover())
            tooltipCanvas.SetActive(false);
    }

    private bool HasInstrumentHover()
    {
        foreach (var interactable in interactor.interactablesHovered)
            if (interactable.transform.GetComponentInParent<Instrument>() != null) return true;
        return false;
    }
}