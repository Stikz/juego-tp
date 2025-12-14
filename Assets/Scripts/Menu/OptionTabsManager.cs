using UnityEngine;
using UnityEngine.UI;

public class OptionsTabsManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    bool _loading;

    [SerializeField] private GameObject audioContent;
    [SerializeField] private GameObject videoContent;

    public void ShowAudio()
    {
        if (audioContent) audioContent.SetActive(true);
        if (videoContent) videoContent.SetActive(false);
    }

    public void ShowVideo()
    {
        if (audioContent) audioContent.SetActive(false);
        if (videoContent) videoContent.SetActive(true);
    }
    void OnEnable()
    {
        _loading = true;
        SetupSlider(masterSlider);
        SetupSlider(sfxSlider);
        SetupSlider(musicSlider);

        float master = PlayerPrefs.GetFloat(SettingsManager.MASTER_KEY, 1f);
        float sfx = PlayerPrefs.GetFloat(SettingsManager.SFX_KEY, 1f);
        float music = PlayerPrefs.GetFloat(SettingsManager.MUSIC_KEY, 1f);

        if (masterSlider) masterSlider.SetValueWithoutNotify(master);
        if (sfxSlider) sfxSlider.SetValueWithoutNotify(sfx);
        if (musicSlider) musicSlider.SetValueWithoutNotify(music);

        if (SettingsManager.Instance)
        {
            SettingsManager.Instance.SetMaster(master, save: false);
            SettingsManager.Instance.SetSfx(sfx, save: false);
            SettingsManager.Instance.SetMusic(music, save: false);
        }

        _loading = false;
        ShowAudio();
    }

    void SetupSlider(Slider s)
    {
        if (!s) return;
        s.minValue = 0f;
        s.maxValue = 1f;
        s.wholeNumbers = false;
    }

    public void OnMasterChanged(float v)
    {
        if (_loading) return;
        if (SettingsManager.Instance) SettingsManager.Instance.SetMaster(v);

    }

    public void OnSfxChanged(float v)
    {
        if (_loading) return;
        if (SettingsManager.Instance) SettingsManager.Instance.SetSfx(v);
    }

    public void OnMusicChanged(float v)
    {
        if (_loading) return;
        if (SettingsManager.Instance) SettingsManager.Instance.SetMusic(v);
    }
}
