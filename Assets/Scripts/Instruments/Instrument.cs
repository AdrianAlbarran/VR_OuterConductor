using System;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    // Wwise track (change data type)
    public int track;
    
    // Callbacks
    public Action onTaken;            // Called in "Select" at "XR Grab Interactable" component (WIP)
    public Action onDropped;          // Called in "Select Exited" at "XR Grab Interactable" component (WIP)


    public void Play()
    {
        // TODO: ADD WWISE CONNECTION
    }

    public void Stop()
    {
        // TODO: ADD WWISE CONNECTION
    }

    public void Restart()
    {
        // TODO: ADD WWISE CONNECTION
    }

    public void Taken()
    {
        onTaken.Invoke();
    }

    public void Dropped()
    {
        onDropped.Invoke();
    }


}
