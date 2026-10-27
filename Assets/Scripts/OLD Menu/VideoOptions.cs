using UnityEngine;

public class VideoOptions : MonoBehaviour
{
    private const string FullKey = "Video_Fullscreen";

    private void OnEnable()
    {
        bool full = PlayerPrefs.GetInt(FullKey, 1) == 1;
        Apply(full);
    }

    public void SetFullscreen()
    {
        Apply(true);
    }

    public void SetWindowed()
    {
        Apply(false);
    }

    private void Apply(bool full)
    {
        Screen.fullScreenMode = full
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        PlayerPrefs.SetInt(FullKey, full ? 1 : 0);
        PlayerPrefs.Save();
    }
}
