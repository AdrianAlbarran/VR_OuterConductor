using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InstrumentSocket : MonoBehaviour
{
    [SerializeField] private MusicianInstrument musicianInstrument;

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
        Debug.Log($"[Socket] Placed: {args.interactableObject.transform.name} | instrument null: {instrument == null} | type: {instrument?.instrumentType} | musicianInstrument null: {musicianInstrument == null}");
        if (instrument == null) return;
        StartCoroutine(DelayedTrembler(instrument));
        musicianInstrument.SetInstrument(instrument.instrumentType);
    }

    private System.Collections.IEnumerator DelayedTrembler(Instrument instrument)
    {
        yield return null;
        instrument.GetComponent<InstrumentTrembler>()?.StartTrembling();
    }

    private void OnRemoved(SelectExitEventArgs args)
    {
        var instrument = args.interactableObject.transform.GetComponent<Instrument>();
        if (instrument == null) return;
        instrument.GetComponent<InstrumentTrembler>()?.StopTrembling();
        musicianInstrument.SetInstrument(InstrumentEnum.None);
    }
}