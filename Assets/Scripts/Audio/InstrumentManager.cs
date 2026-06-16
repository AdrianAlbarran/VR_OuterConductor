using System.Collections.Generic;
using UnityEngine;

public enum InstrumentEnum { None, Banjo, Drums, Flute, Harmonica, Piano, Whistle }

public class InstrumentManager : MonoBehaviour
{
    public static InstrumentManager Instance { get; private set; }

    private readonly Dictionary<InstrumentEnum, string> rtpcNames = new()
    {
        { InstrumentEnum.Banjo,     "BanjoVolume"     },
        { InstrumentEnum.Drums,     "DrumsVolume"     },
        { InstrumentEnum.Flute,     "FluteVolume"     },
        { InstrumentEnum.Harmonica, "HarmonicaVolume" },
        { InstrumentEnum.Piano,     "PianoVolume"     },
        { InstrumentEnum.Whistle,   "WhistleVolume"   },
    };

    void Awake() => Instance = this;

    void Start()
    {
        AkUnitySoundEngine.PostEvent("Play_Song", gameObject);
        foreach (var kvp in rtpcNames)
            AkUnitySoundEngine.SetRTPCValue(kvp.Value, 0f, null);
    }

    public void AssignInstrument(InstrumentEnum instrument)
    {
        if (instrument == InstrumentEnum.None || !rtpcNames.ContainsKey(instrument)) return;
        AkUnitySoundEngine.SetRTPCValue(rtpcNames[instrument], 100f, null);
    }

    public void RemoveInstrument(InstrumentEnum instrument)
    {
        if (instrument == InstrumentEnum.None || !rtpcNames.ContainsKey(instrument)) return;
        AkUnitySoundEngine.SetRTPCValue(rtpcNames[instrument], 0f, null);
    }
}