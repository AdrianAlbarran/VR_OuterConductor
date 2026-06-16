using System.Collections.Generic;
using UnityEngine;

public enum InstrumentEnum { None, Banjo, Drums, Flute, Harmonica, Piano, Whistle }

public class InstrumentManager : MonoBehaviour
{
    public static InstrumentManager Instance { get; private set; }
    public bool IsPlaying { get; private set; }

    public event System.Action OnPerformanceStarted;
    public event System.Action OnPerformanceStopped;

    private readonly Dictionary<InstrumentEnum, string> playEvents = new()
    {
        { InstrumentEnum.Banjo,     "Play_Banjo"      },
        { InstrumentEnum.Drums,     "Play_Drums"      },
        { InstrumentEnum.Flute,     "Play_Flute"      },
        { InstrumentEnum.Harmonica, "Play_Harmonica"  },
        { InstrumentEnum.Piano,     "Play_Piano_Stem" },
        { InstrumentEnum.Whistle,   "Play_Whistle"    },
    };

    private readonly Dictionary<InstrumentEnum, string> stopEvents = new()
    {
        { InstrumentEnum.Banjo,     "Stop_Banjo"      },
        { InstrumentEnum.Drums,     "Stop_Drums"      },
        { InstrumentEnum.Flute,     "Stop_Flute"      },
        { InstrumentEnum.Harmonica, "Stop_Harmonica"  },
        { InstrumentEnum.Piano,     "Stop_Piano_Stem" },
        { InstrumentEnum.Whistle,   "Stop_Whistle"    },
    };

    private readonly Dictionary<InstrumentEnum, GameObject> activeSources = new();
    private bool isQuitting = false;

    void Awake() => Instance = this;
    void OnApplicationQuit() => isQuitting = true;

    public void StartPerformance()
    {
        IsPlaying = true;
        foreach (var kvp in activeSources)
            AkUnitySoundEngine.PostEvent(playEvents[kvp.Key], kvp.Value);
        OnPerformanceStarted?.Invoke();
    }

    public void StopPerformance()
    {
        IsPlaying = false;
        foreach (var kvp in activeSources)
            AkUnitySoundEngine.PostEvent(stopEvents[kvp.Key], kvp.Value);
        OnPerformanceStopped?.Invoke();
    }

    public void AssignInstrument(InstrumentEnum instrument, GameObject source)
    {
        if (instrument == InstrumentEnum.None || !playEvents.ContainsKey(instrument)) return;
        activeSources[instrument] = source;
        if (IsPlaying)
            AkUnitySoundEngine.PostEvent(playEvents[instrument], source);
    }

    public void RemoveInstrument(InstrumentEnum instrument)
    {
        if (isQuitting) return;
        if (instrument == InstrumentEnum.None || !stopEvents.ContainsKey(instrument)) return;
        if (IsPlaying && activeSources.TryGetValue(instrument, out var source))
            AkUnitySoundEngine.PostEvent(stopEvents[instrument], source);
        activeSources.Remove(instrument);
    }
}