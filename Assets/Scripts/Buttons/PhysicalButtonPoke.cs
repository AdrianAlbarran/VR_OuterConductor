using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PhysicalButtonPoke : MonoBehaviour
{
    [SerializeField] private Transform m_visualButtonObject;
    [SerializeField] private float m_pressDepth = -0.05f;
    
    private XRSimpleInteractable _simpleInteractable;
    private ReturnGrabInteractable _returnGranInteractable;
    private Vector3 _startLocalPosition;

    private void Awake()
    {
        _simpleInteractable = GetComponent<XRSimpleInteractable>();
        _returnGranInteractable = GetComponent<ReturnGrabInteractable>();
    }

    private void Start()
    {
        if (m_visualButtonObject != null)
        {
            _startLocalPosition = m_visualButtonObject.localPosition;
        }
    }

    private void OnEnable()
    {
        _simpleInteractable.selectEntered.AddListener(OnButtonDown);
        _simpleInteractable.selectExited.AddListener(OnButtonUp);
    }
    
    private void OnDisable()
    {
        _simpleInteractable.selectEntered.RemoveListener(OnButtonDown);
        _simpleInteractable.selectExited.RemoveListener(OnButtonUp);
    }

    private void OnButtonDown(SelectEnterEventArgs args)
    {
        if (m_visualButtonObject != null)
        {
            m_visualButtonObject.localPosition = _startLocalPosition + new Vector3(0, m_pressDepth, 0);
        }

        if (_returnGranInteractable != null)
        {
            _returnGranInteractable.InstrumentTpToInteractor();
        }
    }

    private void OnButtonUp(SelectExitEventArgs args)
    {
        if (m_visualButtonObject != null)
        {
            m_visualButtonObject.localPosition = _startLocalPosition;
        }
    }
}
