using UnityEngine;

public class MusicianInstrument : MonoBehaviour
{
    [SerializeField] private InstrumentEnum instrument = InstrumentEnum.None;

    public InstrumentEnum CurrentInstrument => instrument;

    void Start()
    {
        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance?.AssignInstrument(instrument, gameObject);
    }

    public void SetInstrument(InstrumentEnum newInstrument)
    {
        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance?.RemoveInstrument(instrument);

        instrument = newInstrument;

        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance?.AssignInstrument(instrument, gameObject);
    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;
        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance?.RemoveInstrument(instrument);
    }
}