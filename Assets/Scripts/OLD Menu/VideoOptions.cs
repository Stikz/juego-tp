using UnityEngine;

public class VideoOptions : MonoBehaviour
{
    private void OnEnable()
    {
        SettingsManager.SetFullscreen(PlayerPrefs.GetInt(SettingsManager.FullscreenKey, 1) == 1);
    }

    public void SetFullscreen()
    {
        SettingsManager.SetFullscreen(true);
    }

    public void SetWindowed()
    {
        SettingsManager.SetFullscreen(false);
    }
}
