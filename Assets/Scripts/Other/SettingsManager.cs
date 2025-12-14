using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public AudioMixer mixer;

    public const string MasterKey = "Vol_Master";
    public const string SfxKey = "Vol_Sfx";
    public const string MusicKey = "Vol_Music";

    const string MasterParam = "MasterVolume";
    const string SfxParam = "SfxVolume";
    const string MusicParam = "MusicVolume";

    public const string MASTER_KEY = "Vol_Master";
    public const string SFX_KEY = "Vol_Sfx";
    public const string MUSIC_KEY = "Vol_Music";

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ApplyAllFromPrefs();
    }

    public void ApplyAllFromPrefs()
    {
        SetMaster(PlayerPrefs.GetFloat(MasterKey, 1f), save: false);
        SetSfx(PlayerPrefs.GetFloat(SfxKey, 1f), save: false);
        SetMusic(PlayerPrefs.GetFloat(MusicKey, 1f), save: false);
    }

    public void SetMaster(float v01, bool save = true)
    {
        ApplyMixer01(MasterParam, v01);
        if (save) PlayerPrefs.SetFloat(MasterKey, Mathf.Clamp01(v01));
    }

    public void SetSfx(float v01, bool save = true)
    {
        ApplyMixer01(SfxParam, v01);
        if (save) PlayerPrefs.SetFloat(SfxKey, Mathf.Clamp01(v01));
    }

    public void SetMusic(float v01, bool save = true)
    {
        ApplyMixer01(MusicParam, v01);
        if (save) PlayerPrefs.SetFloat(MusicKey, Mathf.Clamp01(v01));
    }

    void ApplyMixer01(string param, float v01)
    {
        v01 = Mathf.Clamp01(v01);

        if (mixer == null) return;

        if (v01 <= 0.0005f)
        {
            mixer.SetFloat(param, -80f);
            return;
        }

        float db = Mathf.Log10(v01) * 20f;
        mixer.SetFloat(param, db);
    }
}
