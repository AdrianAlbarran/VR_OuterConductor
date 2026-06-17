using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class UpdateUIText : MonoBehaviour
{
    [SerializeField] TMP_Text m_TextMeshPro;
    [SerializeField] BatonGestureTracker m_baton;
    float m_tempo = 100;

    public void Start()
    {
        m_baton.changeTempo.AddListener(UpdateTextMeshPro);    
    }

    private void UpdateTextMeshPro(float value)
    {
        m_tempo = Mathf.Clamp(m_tempo + value, 0, 200);
        m_TextMeshPro.text = $"{m_tempo}%";
    }
}
