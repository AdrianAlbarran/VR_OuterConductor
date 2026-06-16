using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ReturnInstrument : MonoBehaviour
{
    [SerializeField] private Instrument m_instrument;
    [SerializeField] private XRSocketInteractor m_socketInteractor;

    public void InstrumentTpToInteractor()
    {
        m_instrument.transform.position = m_socketInteractor.transform.position;
    }
}
