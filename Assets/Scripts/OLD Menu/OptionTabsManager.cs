using UnityEngine;
using UnityEngine.UI;

public class OptionsTabsManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    [SerializeField] private GameObject audioContent;
    [SerializeField] private GameObject videoContent;
    [SerializeField] private Vector2 cancelButtonOffset = new Vector2(0f, -110f);

    private bool loading;
    private SettingsMemento snapshot;
    private Button backButton;
    private Button cancelButton;

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
        SetupSlider(masterSlider);
        SetupSlider(sfxSlider);
        SetupSlider(musicSlider);
        RefreshSliders();

        // PATRON MEMENTO
        if (SettingsManager.Instance)
            snapshot = SettingsManager.Instance.CreateMemento();

        CreateCancelButton();
        ShowAudio();
    }

    // PATRON MEMENTO
    public void CancelChanges()
    {
        if (SettingsManager.Instance && snapshot != null)
            SettingsManager.Instance.Restore(snapshot);

        RefreshSliders();

        if (backButton != null)
            backButton.onClick.Invoke();
    }

    void RefreshSliders()
    {
        loading = true;

        float master = PlayerPrefs.GetFloat(SettingsManager.MasterKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SettingsManager.SfxKey, 1f);
        float music = PlayerPrefs.GetFloat(SettingsManager.MusicKey, 1f);

        if (masterSlider) masterSlider.SetValueWithoutNotify(master);
        if (sfxSlider) sfxSlider.SetValueWithoutNotify(sfx);
        if (musicSlider) musicSlider.SetValueWithoutNotify(music);

        if (SettingsManager.Instance)
        {
            SettingsManager.Instance.SetMaster(master, save: false);
            SettingsManager.Instance.SetSfx(sfx, save: false);
            SettingsManager.Instance.SetMusic(music, save: false);
        }

        loading = false;
    }

    void CreateCancelButton()
    {
        if (cancelButton != null) return;

        Transform back = transform.Find("BackButton");
        if (back == null) return;

        backButton = back.GetComponent<Button>();
        Sprite normal = Resources.Load<Sprite>("MenuUI/cancel_button");
        Sprite selected = Resources.Load<Sprite>("MenuUI/cancel_button_selected");
        if (backButton == null || normal == null) return;

        GameObject cancel = Instantiate(back.gameObject, back.parent);
        cancel.name = "CancelButton";

        RectTransform backRect = back as RectTransform;
        RectTransform cancelRect = cancel.GetComponent<RectTransform>();
        cancelRect.anchoredPosition = backRect.anchoredPosition + cancelButtonOffset;
        cancelRect.sizeDelta = new Vector2(normal.rect.width, normal.rect.height) * 3f;

        Image image = cancel.GetComponent<Image>();
        image.sprite = normal;
        image.preserveAspect = true;

        cancelButton = cancel.GetComponent<Button>();
        cancelButton.onClick = new Button.ButtonClickedEvent();
        cancelButton.onClick.AddListener(CancelChanges);
        cancelButton.transition = Selectable.Transition.SpriteSwap;
        cancelButton.spriteState = new SpriteState
        {
            highlightedSprite = selected,
            selectedSprite = selected,
            pressedSprite = selected
        };
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
        if (loading) return;
        if (SettingsManager.Instance) SettingsManager.Instance.SetMaster(v);
    }

    public void OnSfxChanged(float v)
    {
        if (loading) return;
        if (SettingsManager.Instance) SettingsManager.Instance.SetSfx(v);
    }

    public void OnMusicChanged(float v)
    {
        if (loading) return;
        if (SettingsManager.Instance) SettingsManager.Instance.SetMusic(v);
    }
}
