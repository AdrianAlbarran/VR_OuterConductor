using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Instrument : MonoBehaviour
{
    [SerializeField] public InstrumentEnum instrumentType = InstrumentEnum.None;
    [SerializeField] public Transform meshRoot;

    [Header("Hold Offset")]
    [SerializeField] public Vector3 holdPositionOffset = Vector3.zero;
    [SerializeField] public Vector3 holdRotationOffset = Vector3.zero;
}