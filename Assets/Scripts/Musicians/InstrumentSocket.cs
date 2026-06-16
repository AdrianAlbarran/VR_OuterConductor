using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public struct InstrumentOffset
{
    public InstrumentEnum instrumentType;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
}

public class InstrumentSocket : MonoBehaviour
{
    [SerializeField] private MusicianInstrument musicianInstrument;
    [SerializeField] private GameObject[] musicianHands;
    [SerializeField] private Transform attachPoint;
    [SerializeField] private InstrumentOffset[] instrumentOffsets;

    private XRSocketInteractor socket;
    private Instrument currentInstrument;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnPlaced);
        socket.selectExited.AddListener(OnRemoved);
    }

    void Start()
    {
        if (InstrumentManager.Instance != null)
        {
            InstrumentManager.Instance.OnPerformanceStarted += OnPerformanceStarted;
            InstrumentManager.Instance.OnPerformanceStopped += OnPerformanceStopped;
        }
    }

    void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnPlaced);
        socket.selectExited.RemoveListener(OnRemoved);
        if (InstrumentManager.Instance != null)
        {
            InstrumentManager.Instance.OnPerformanceStarted -= OnPerformanceStarted;
            InstrumentManager.Instance.OnPerformanceStopped -= OnPerformanceStopped;
        }
    }

    private void OnPlaced(SelectEnterEventArgs args)
    {
        currentInstrument = args.interactableObject.transform.GetComponent<Instrument>();
        if (currentInstrument == null) return;

        ApplyOffset(currentInstrument.instrumentType, currentInstrument);
        StartCoroutine(DelayedSetup(currentInstrument));

        musicianInstrument.SetInstrument(currentInstrument.instrumentType);
        SetMusicianHands(false);
        SetInstrumentHands(currentInstrument.transform, true);
    }

    private System.Collections.IEnumerator DelayedSetup(Instrument instrument)
    {
        yield return null;
        ApplyOffset(instrument.instrumentType, instrument);
        if (InstrumentManager.Instance.IsPlaying)
        {
            instrument.GetComponent<InstrumentTrembler>()?.StartTrembling();
            instrument.GetComponent<InstrumentParticles>()?.StartPlaying();
        }
    }

    private void OnRemoved(SelectExitEventArgs args)
    {
        if (currentInstrument == null) return;
        currentInstrument.GetComponent<InstrumentTrembler>()?.StopTrembling();
        currentInstrument.GetComponent<InstrumentParticles>()?.StopPlaying();
        musicianInstrument.SetInstrument(InstrumentEnum.None);
        SetMusicianHands(true);
        SetInstrumentHands(currentInstrument.transform, false);
        ResetOffset();
        currentInstrument = null;
    }

    private void OnPerformanceStarted()
    {
        if (currentInstrument != null)
        {
            StartCoroutine(DelayedTrembler(currentInstrument));
            currentInstrument.GetComponent<InstrumentParticles>()?.StartPlaying();
        }
    }

    private void OnPerformanceStopped()
    {
        if (currentInstrument != null)
        {
            currentInstrument.GetComponent<InstrumentTrembler>()?.StopTrembling();
            currentInstrument.GetComponent<InstrumentParticles>()?.StopPlaying();
        }
    }

    private void ApplyOffset(InstrumentEnum type, Instrument instrument)
    {
        var meshChild = instrument.meshRoot;
        if (meshChild == null) return;

        foreach (var offset in instrumentOffsets)
        {
            if (offset.instrumentType == type)
            {
                meshChild.localPosition = offset.positionOffset;
                meshChild.localRotation = Quaternion.Euler(offset.rotationOffset);
                return;
            }
        }
        meshChild.localPosition = Vector3.zero;
        meshChild.localRotation = Quaternion.identity;
    }

    private void ResetOffset()
    {
        if (attachPoint == null) return;
        attachPoint.localPosition = Vector3.zero;
        attachPoint.localRotation = Quaternion.identity;
    }

    private void SetMusicianHands(bool active)
    {
        foreach (var hand in musicianHands)
            if (hand) hand.SetActive(active);
    }

    private void SetInstrumentHands(Transform instrumentRoot, bool active)
    {
        var hand1 = FindDeep(instrumentRoot, "Hand1");
        var hand2 = FindDeep(instrumentRoot, "Hand2");
        if (hand1) hand1.gameObject.SetActive(active);
        if (hand2) hand2.gameObject.SetActive(active);
    }

    private Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            if (child.name == name) return child;
        return null;
    }

    private System.Collections.IEnumerator DelayedTrembler(Instrument instrument)
    {
        yield return null;
        ApplyOffset(instrument.instrumentType, instrument);
        instrument.GetComponent<InstrumentTrembler>()?.StartTrembling();

    }
}