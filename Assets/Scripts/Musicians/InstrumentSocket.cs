using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InstrumentSocket : MonoBehaviour
{
    [SerializeField] private MusicianInstrument musicianInstrument;
    [SerializeField] private GameObject[] musicianHands;

    private XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnPlaced);
        socket.selectExited.AddListener(OnRemoved);
    }

    void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnPlaced);
        socket.selectExited.RemoveListener(OnRemoved);
    }

    private void OnPlaced(SelectEnterEventArgs args)
    {
        var instrument = args.interactableObject.transform.GetComponent<Instrument>();
        if (instrument == null) return;
        StartCoroutine(DelayedTrembler(instrument));
        musicianInstrument.SetInstrument(instrument.instrumentType);
        SetMusicianHands(false);
        SetInstrumentHands(instrument.transform, true);
    }

    private void OnRemoved(SelectExitEventArgs args)
    {
        var instrument = args.interactableObject.transform.GetComponent<Instrument>();
        if (instrument == null) return;
        instrument.GetComponent<InstrumentTrembler>()?.StopTrembling();
        musicianInstrument.SetInstrument(InstrumentEnum.None);
        SetMusicianHands(true);
        SetInstrumentHands(instrument.transform, false);
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