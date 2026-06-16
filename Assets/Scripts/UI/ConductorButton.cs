using UnityEngine;
using TMPro;

public class ConductorButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string startText = "Empezar";
    [SerializeField] private string stopText = "Finalizar";

    void Start() => buttonText.text = startText;

    public void OnButtonPressed()
    {
        if (!InstrumentManager.Instance.IsPlaying)
        {
            InstrumentManager.Instance.StartPerformance();
            buttonText.text = stopText;
        }
        else
        {
            InstrumentManager.Instance.StopPerformance();
            buttonText.text = startText;
        }
    }
}