using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRControllerInputFixer : MonoBehaviour
{
    [Header("Acción de Giro a Rescatar")]
    [SerializeField] private InputActionReference m_TurnActionReference;
    [SerializeField] private InputActionReference m_SnapTurnActionReference;

    [Header("Configuración")]
    [Tooltip("Tiempo de espera en segundos tras activarse el mando para reaccionar.")]
    [SerializeField] private float m_DelaySeconds = 0.1f;

    private void OnEnable()
    {
        if (m_TurnActionReference != null && m_TurnActionReference.action != null && m_SnapTurnActionReference != null && m_SnapTurnActionReference.action != null)
        {
            StartCoroutine(RescueTurnActionCoroutine());
        }
    }

    private IEnumerator RescueTurnActionCoroutine()
    {
        yield return new WaitForSeconds(m_DelaySeconds);

        if (m_TurnActionReference.action != null && m_SnapTurnActionReference.action != null)
        {
            m_TurnActionReference.action.Enable();
            m_SnapTurnActionReference.action.Enable();
            Debug.Log($"<color=lime>[MANDO DETECTADO]</color> GameObject del mando activo. Acción forzada a ENABLED tras la inicialización.");
        }
    }
}