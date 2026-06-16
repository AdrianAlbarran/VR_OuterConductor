using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Instrument : MonoBehaviour
{
    [SerializeField] public InstrumentEnum instrumentType = InstrumentEnum.None;
    [SerializeField] private float assignRadius = 1.0f;

    private InstrumentHolder currentHolder;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        currentHolder?.RemoveInstrument();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        InstrumentHolder nearest = FindNearestFreeHolder();
        if (nearest != null)
            nearest.AssignInstrument(this);
    }

    public void SetHolder(InstrumentHolder holder) => currentHolder = holder;
    public void ClearHolder() => currentHolder = null;

    private InstrumentHolder FindNearestFreeHolder()
    {
        var holders = FindObjectsByType<InstrumentHolder>(FindObjectsSortMode.None);
        InstrumentHolder nearest = null;
        float minDist = assignRadius;

        foreach (var holder in holders)
        {
            if (holder.HasInstrument) continue;
            float dist = Vector3.Distance(transform.position, holder.transform.position);
            if (dist < minDist) { minDist = dist; nearest = holder; }
        }
        return nearest;
    }
}