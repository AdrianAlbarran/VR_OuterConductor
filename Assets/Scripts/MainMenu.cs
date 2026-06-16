using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Locomotion")]
    [SerializeField] private GameObject locomotion;

    [Header("Options")]
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private float volumeStep = 10f;
    private float currentVolume = 100f;

    void Start()
    {
        if (locomotion) locomotion.SetActive(false);
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        ApplyVolume();
    }

    public void OnPlay()
    {
        if (locomotion) locomotion.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnBack()
    {
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OnExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void VolumeUp()
    {
        currentVolume = Mathf.Clamp(currentVolume + volumeStep, 0f, 100f);
        ApplyVolume();
    }

    public void VolumeDown()
    {
        currentVolume = Mathf.Clamp(currentVolume - volumeStep, 0f, 100f);
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        AkUnitySoundEngine.SetRTPCValue("MasterVolume", currentVolume, null);
        if (volumeText) volumeText.text = $"Volumen: {(int)currentVolume}%";
    }
}