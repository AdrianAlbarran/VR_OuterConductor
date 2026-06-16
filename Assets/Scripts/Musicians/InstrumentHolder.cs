using UnityEngine;

public class InstrumentHolder : MonoBehaviour
{
    [SerializeField] private Transform instrumentAnchor;
    [SerializeField] private MusicianInstrument musicianInstrument;

    private Instrument heldInstrument;
    public bool HasInstrument => heldInstrument != null;

    public void AssignInstrument(Instrument instrument)
    {
        Debug.Log($"[Holder] instrumentType: {instrument.instrumentType} | musicianInstrument null: {musicianInstrument == null}");

        heldInstrument = instrument;
        instrument.SetHolder(this);

        var rb = instrument.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        instrument.transform.SetParent(instrumentAnchor);
        instrument.transform.localPosition = instrument.holdPositionOffset;
        instrument.transform.localRotation = Quaternion.Euler(instrument.holdRotationOffset);

        instrument.GetComponent<InstrumentTrembler>()?.StartTrembling();
        musicianInstrument.SetInstrument(instrument.instrumentType);
    }

    public void RemoveInstrument()
    {
        if (heldInstrument == null) return;

        heldInstrument.GetComponent<InstrumentTrembler>()?.StopTrembling();
        heldInstrument.transform.SetParent(null);
        heldInstrument.ClearHolder();

        var rb = heldInstrument.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        musicianInstrument.SetInstrument(InstrumentEnum.None);
        heldInstrument = null;
    }
}