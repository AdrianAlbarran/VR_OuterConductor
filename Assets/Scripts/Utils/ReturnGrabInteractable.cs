using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ReturnGrabInteractable : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable m_grabInteractable;
    [SerializeField] private XRSocketInteractor m_socketInteractor;

    public void InstrumentTpToInteractor()
    {
        m_grabInteractable.transform.position = m_socketInteractor.transform.position;
    }
}
