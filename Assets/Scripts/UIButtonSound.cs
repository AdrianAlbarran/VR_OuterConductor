using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private string hoverEvent = "Play_Button_Hover";
    [SerializeField] private string clickEvent = "Play_Button_Click";

    public void OnPointerEnter(PointerEventData eventData)
    {
        AkUnitySoundEngine.PostEvent(hoverEvent, gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AkUnitySoundEngine.PostEvent(clickEvent, gameObject);
    }
}