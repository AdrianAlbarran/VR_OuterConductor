using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Musician : MonoBehaviour
{
    [Header("Instrument being hold")]
    [Space][SerializeField] private Instrument instrument;

    public void SaveAttachedInstrument(SelectEnterEventArgs args)
    {
        if(instrument != null )        
            instrument = args.interactableObject.transform.GetComponent<Instrument>();
    }

    public void DropAttachedInstrument()
    {
        instrument = null;
    }



}
