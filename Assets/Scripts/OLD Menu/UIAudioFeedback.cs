using UnityEngine;
using UnityEngine.EventSystems;

public class UIAudioFeedback : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private bool enableHoverSound = true;
    [SerializeField] private bool enableClickSound = true;
    [SerializeField] private bool disableAllSounds = false;

    [SerializeField] private float hoverBlockAfterClick = 0.08f; 
    private float blockHoverUntil;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disableAllSounds || !enableHoverSound) return;
        ManageScenes.Instance?.PlayUIHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (disableAllSounds || !enableClickSound) return;
        ManageScenes.Instance?.PlayUIClick();
    }


    public void OnSelect(BaseEventData eventData)
    {
        if (disableAllSounds || !enableHoverSound) return;
        if (Time.unscaledTime < blockHoverUntil) return;

        ManageScenes.Instance?.PlayUIHover();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (disableAllSounds || !enableClickSound) return;

        blockHoverUntil = Time.unscaledTime + hoverBlockAfterClick;
        ManageScenes.Instance?.PlayUIClick();
    }
}
