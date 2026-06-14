using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("SoundBanks a cargar")]
    [SerializeField] private string[] soundBanks = { "Piano" };

    [Header("Listener")]
    [SerializeField] private Transform vrCamera; 

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSoundBanks();
    }

    private void Start()
    {
        SetupListener();
    }

    private void LoadSoundBanks()
    {
        foreach (string bank in soundBanks)
        {
            AkUnitySoundEngine.LoadBank(bank, out uint bankID);
            Debug.Log($"[SoundManager] SoundBank cargado: {bank}");
        }
    }

    private void SetupListener()
    {
        if (vrCamera == null)
        {
            vrCamera = Camera.main?.transform;
        }

        if (vrCamera != null)
        {
            var existingListener = FindAnyObjectByType<AkAudioListener>();
            if (existingListener != null &&
                existingListener.transform != vrCamera)
            {
                Destroy(existingListener);
            }

            if (vrCamera.GetComponent<AkAudioListener>() == null)
            {
                vrCamera.gameObject.AddComponent<AkAudioListener>();
                Debug.Log("[SoundManager] AkAudioListener movido a la cámara VR.");
            }
        }
    }

    // API pública para las teclas del piano
    public void PlayPianoNote(string noteName, int velocityLayer, GameObject source)
    {
        AkUnitySoundEngine.SetSwitch("PianoNote", noteName, source);
        AkUnitySoundEngine.SetSwitch("PianoVelocity", $"v{velocityLayer}", source);
        AkUnitySoundEngine.PostEvent("Play_Piano_Note", source);
    }

    public void ReleasePianoNote(string noteName, GameObject source)
    {
        AkUnitySoundEngine.SetSwitch("PianoNote", noteName, source);
        AkUnitySoundEngine.PostEvent("Release_Piano_Note", source);
    }

    private void OnDestroy()
    {
        UnloadSoundBanks();
    }

    private void OnApplicationQuit()
    {
        UnloadSoundBanks();
    }


    private void UnloadSoundBanks()
    {
        foreach (string bank in soundBanks)
        {
            try
            {
                AkUnitySoundEngine.UnloadBank(bank, System.IntPtr.Zero);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[SoundManager] No se pudo descargar {bank}: {e.Message}");
            }
        }
    }
}