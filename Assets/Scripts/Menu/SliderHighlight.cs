using UnityEngine;
using UnityEngine.EventSystems;

public class SliderHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject highlight;

    private void Awake()
    {
        if (highlight) highlight.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (highlight) highlight.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (highlight) highlight.SetActive(false);
    }
}
