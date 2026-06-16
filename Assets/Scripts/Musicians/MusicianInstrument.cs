using UnityEngine;

public class MusicianInstrument : MonoBehaviour
{
    [SerializeField] private InstrumentEnum instrument = InstrumentEnum.None;

    public InstrumentEnum CurrentInstrument => instrument;

    void Start()
    {
        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance.AssignInstrument(instrument);
    }

    public void SetInstrument(InstrumentEnum newInstrument)
    {
        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance.RemoveInstrument(instrument);

        instrument = newInstrument;

        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance.AssignInstrument(instrument);
    }

    void OnDestroy()
    {
        if (instrument != InstrumentEnum.None)
            InstrumentManager.Instance?.RemoveInstrument(instrument);
    }
}