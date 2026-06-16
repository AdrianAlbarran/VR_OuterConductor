using System;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    public InstrumentEnum instrumentType = InstrumentEnum.None;

    public Action onTaken;
    public Action onDropped;

    public void Play() { InstrumentManager.Instance.AssignInstrument(instrumentType); }
    public void Stop() { InstrumentManager.Instance.RemoveInstrument(instrumentType); }
    public void Restart() { }

    public void Taken() { onTaken?.Invoke(); }
    public void Dropped() { onDropped?.Invoke(); }
}