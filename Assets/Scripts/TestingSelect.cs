using UnityEngine;
using UnityEngine.InputSystem;

public class TestingSelect : MonoBehaviour
{
    [Header("Configuración de Diagnóstico de Input")]
    [SerializeField] private InputActionReference m_SelectAction;

    private void Start()
    {
        if(m_SelectAction != null && m_SelectAction.action != null)
        {
            m_SelectAction.action.Enable();
            m_SelectAction.action.performed += Foo;
        }
    }


    public void Foo(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        Debug.Log($"<color=yellow>[INPUT SYSTEM]</color> ¡Botón presionado detectado por código! Valor: {value}");
    }
}
