using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PianoKey : MonoBehaviour
{
    [Header("Nota")]
    public string noteName = "C4";

    [Header("Rotación")]
    public float maxRotationAngle = 5f;
    public float returnSpeed = 5f;
    public float pressSpeed = 15f;
    public float minPressure = 0.05f;

    private float currentAngle = 0f;
    private bool isPlaying = false;
    private bool isPressed = false;
    private int touchCount = 0;

    private static readonly Dictionary<string, string> NoteMapping =
        new Dictionary<string, string>
    {
        { "A0",  "A0"  }, { "As0", "A0"  }, { "B0",  "C1"  },
        { "C1",  "C1"  }, { "Cs1", "C1"  }, { "D1",  "Ds1" },
        { "Ds1", "Ds1" }, { "E1",  "Ds1" }, { "F1",  "Fs1" },
        { "Fs1", "Fs1" }, { "G1",  "Fs1" }, { "Gs1", "A1"  },
        { "A1",  "A1"  }, { "As1", "A1"  }, { "B1",  "C2"  },
        { "C2",  "C2"  }, { "Cs2", "C2"  }, { "D2",  "Ds2" },
        { "Ds2", "Ds2" }, { "E2",  "Ds2" }, { "F2",  "Fs2" },
        { "Fs2", "Fs2" }, { "G2",  "Fs2" }, { "Gs2", "A2"  },
        { "A2",  "A2"  }, { "As2", "A2"  }, { "B2",  "C3"  },
        { "C3",  "C3"  }, { "Cs3", "C3"  }, { "D3",  "Ds3" },
        { "Ds3", "Ds3" }, { "E3",  "Ds3" }, { "F3",  "Fs3" },
        { "Fs3", "Fs3" }, { "G3",  "Fs3" }, { "Gs3", "A3"  },
        { "A3",  "A3"  }, { "As3", "A3"  }, { "B3",  "C4"  },
        { "C4",  "C4"  }, { "Cs4", "C4"  }, { "D4",  "Ds4" },
        { "Ds4", "Ds4" }, { "E4",  "Ds4" }, { "F4",  "Fs4" },
        { "Fs4", "Fs4" }, { "G4",  "Fs4" }, { "Gs4", "A4"  },
        { "A4",  "A4"  }, { "As4", "A4"  }, { "B4",  "C5"  },
        { "C5",  "C5"  }, { "Cs5", "C5"  }, { "D5",  "Ds5" },
        { "Ds5", "Ds5" }, { "E5",  "Ds5" }, { "F5",  "Fs5" },
        { "Fs5", "Fs5" }, { "G5",  "Fs5" }, { "Gs5", "A5"  },
        { "A5",  "A5"  }, { "As5", "A5"  }, { "B5",  "C6"  },
        { "C6",  "C6"  }, { "Cs6", "C6"  }, { "D6",  "Ds6" },
        { "Ds6", "Ds6" }, { "E6",  "Ds6" }, { "F6",  "Fs6" },
        { "Fs6", "Fs6" }, { "G6",  "Fs6" }, { "Gs6", "A6"  },
        { "A6",  "A6"  }, { "As6", "A6"  }, { "B6",  "C7"  },
        { "C7",  "C7"  }, { "Cs7", "C7"  }, { "D7",  "Ds7" },
        { "Ds7", "Ds7" }, { "E7",  "Ds7" }, { "F7",  "Fs7" },
        { "Fs7", "Fs7" }, { "G7",  "Fs7" }, { "Gs7", "A7"  },
        { "A7",  "A7"  }, { "As7", "A7"  }, { "B7",  "C8"  },
        { "C8",  "C8"  }
    };

    private string GetWwiseNote()
    {
        if (NoteMapping.TryGetValue(noteName, out string wwiseNote))
            return wwiseNote;
        Debug.LogWarning($"[PianoKey] Nota {noteName} no encontrada en el mapeo.");
        return noteName;
    }

    void Update()
    {
        float targetAngle = isPressed ? maxRotationAngle : 0f;
        float speed = isPressed ? pressSpeed : returnSpeed;

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * speed);
        currentAngle = Mathf.Clamp(currentAngle, 0f, maxRotationAngle);

        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);

        float pressure = currentAngle / maxRotationAngle;

        if (isPlaying)
        {
            AkUnitySoundEngine.SetRTPCValue("KeyPressure", pressure, gameObject);

            if (pressure < minPressure)
                StopNote();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInChildren<XRBaseInteractor>() && other.transform.tag != "TrackedHand") return;

        touchCount++;
        if (touchCount == 1)
        {
            isPressed = true;

            var rb = other.attachedRigidbody;
            float speed = rb != null ? rb.linearVelocity.magnitude : 1f;

            int[] validVels = { 1, 5, 9, 13 };
            int velIndex = Mathf.Clamp(Mathf.RoundToInt(speed * 1.5f), 0, 3);
            int vel = validVels[velIndex];

            if (!isPlaying)
                StartNote(vel);
            else
                AkUnitySoundEngine.SetSwitch("PianoVelocity", $"v{vel}", gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.GetComponentInChildren<XRBaseInteractor>() && other.transform.tag != "TrackedHand") return;

        touchCount--;
        if (touchCount <= 0)
        {
            touchCount = 0;
            isPressed = false;
        }
    }

    private void StartNote(int vel)
    {
        string wwiseNote = GetWwiseNote();
        isPlaying = true;
        AkUnitySoundEngine.SetSwitch("PianoNote", wwiseNote, gameObject);
        AkUnitySoundEngine.SetSwitch("PianoVelocity", $"v{vel}", gameObject);
        AkUnitySoundEngine.SetRTPCValue("KeyPressure", 1f, gameObject);
        AkUnitySoundEngine.PostEvent("Play_Piano_Note", gameObject);
    }

    private void StopNote()
    {
        isPlaying = false;
        AkUnitySoundEngine.SetRTPCValue("KeyPressure", 0f, gameObject);
        AkUnitySoundEngine.PostEvent("Stop_Piano_Note", gameObject);
    }
}