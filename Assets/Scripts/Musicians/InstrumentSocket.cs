using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InstrumentSocket : MonoBehaviour
{
    [SerializeField] private MusicianInstrument musicianInstrument;
    [SerializeField] private GameObject[] musicianHands;

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
        if (InstrumentManager.Instance.IsPlaying)
            StartCoroutine(DelayedTrembler(currentInstrument));
        musicianInstrument.SetInstrument(currentInstrument.instrumentType);
        SetMusicianHands(false);
        SetInstrumentHands(currentInstrument.transform, true);

        if (InstrumentManager.Instance.IsPlaying)
            currentInstrument.GetComponent<InstrumentParticles>()?.StartPlaying();
    }

    private void OnRemoved(SelectExitEventArgs args)
    {
        if (currentInstrument == null) return;
        currentInstrument.GetComponent<InstrumentTrembler>()?.StopTrembling();
        musicianInstrument.SetInstrument(InstrumentEnum.None);
        SetMusicianHands(true);
        SetInstrumentHands(currentInstrument.transform, false);
        currentInstrument.GetComponent<InstrumentParticles>()?.StopPlaying();
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
        instrument.GetComponent<InstrumentTrembler>()?.StartTrembling();
    }
}