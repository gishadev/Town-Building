using UnityEngine.EventSystems;
using UnityEngine;

public class BuilderDisableUIArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        WorldManager.Instance.ObjectBuilder.isEnabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WorldManager.Instance.ObjectBuilder.isEnabled = true;
    }
}
