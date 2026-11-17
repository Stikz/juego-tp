using UnityEngine;
using UnityEngine.UI;

public class SkinMenuUI : MonoBehaviour
{
    private const float Scale = 5f;
    private const float ButtonScale = 3f;

    private readonly Image[] cardFrames = new Image[3];
    private readonly Button[] cardButtons = new Button[3];
    private Sprite cardSprite;
    private Sprite cardSelectedSprite;
    private Button backButton;

    public GameObject OpenButton { get; private set; }

    public static SkinMenuUI Build(Transform canvas, MainMenuUIManager menu)
    {
        GameObject root = CreateRect("SkinsPanel", canvas);
        Stretch(root.GetComponent<RectTransform>());
        Image overlay = root.AddComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.75f);

        SkinMenuUI ui = root.AddComponent<SkinMenuUI>();
        ui.cardSprite = Load("skin_card");
        ui.cardSelectedSprite = Load("skin_card_selected");

        Sprite windowSprite = Load("skins_window");
        GameObject window = CreateImage("Window", root.transform, windowSprite);
        RectTransform windowRect = window.GetComponent<RectTransform>();
        windowRect.sizeDelta = new Vector2(windowSprite.rect.width, windowSprite.rect.height) * Scale;

        string[] labels = { "label_normal", "label_roja", "label_azul" };
        for (int i = 0; i < 3; i++)
            ui.CreateCard(window.transform, i, (SkinType)i, Load(labels[i]));

        GameObject back = CreateImage("BackButton", window.transform, Load("back_button"));
        Place(back.GetComponent<RectTransform>(), 146f, 80f, 24f, 24f);
        ui.backButton = MakeButton(back, Load("back_button_selected"));
        ui.backButton.onClick.AddListener(menu.ShowMainMenu);

        Sprite openSprite = Load("skins_button");
        GameObject open = CreateImage("SkinsButton", canvas, openSprite);
        RectTransform openRect = open.GetComponent<RectTransform>();
        openRect.anchorMin = openRect.anchorMax = openRect.pivot = Vector2.zero;
        openRect.anchoredPosition = new Vector2(40f, 40f);
        openRect.sizeDelta = new Vector2(openSprite.rect.width, openSprite.rect.height) * ButtonScale;
        MakeButton(open, Load("skins_button_selected")).onClick.AddListener(menu.ShowSkins);
        ui.OpenButton = open;

        root.SetActive(false);
        return ui;
    }

    private void OnEnable()
    {
        Refresh();

        if (NavigationManager.Instance != null)
            NavigationManager.Instance.EnterMenu(cardButtons[(int)SkinSelection.Current].gameObject);
    }

    private void CreateCard(Transform window, int index, SkinType type, Sprite label)
    {
        float x = 16f + index * 52f;
        float y = 26f;

        GameObject card = CreateImage("Card" + type, window, cardSprite);
        Place(card.GetComponent<RectTransform>(), x, y, 40f, 54f);
        cardFrames[index] = card.GetComponent<Image>();

        // PATRON ABSTRACT FACTORY
        Sprite preview = SkinSelection.GetFactory(type).CreatePlayerSprite();
        GameObject player = CreateImage("Preview", card.transform, preview);
        Place(player.GetComponent<RectTransform>(), 4f, 4f, 32f, 32f);
        player.GetComponent<Image>().preserveAspect = true;
        player.GetComponent<Image>().raycastTarget = false;

        GameObject name = CreateImage("Label", card.transform, label);
        Place(name.GetComponent<RectTransform>(), (40f - label.rect.width) / 2f, 41f, label.rect.width, label.rect.height);
        name.GetComponent<Image>().raycastTarget = false;

        Button button = card.AddComponent<Button>();
        button.transition = Selectable.Transition.ColorTint;
        button.onClick.AddListener(() => Select(type));
        card.AddComponent<UIAudioFeedback>();
        cardButtons[index] = button;
    }

    private void Select(SkinType type)
    {
        SkinSelection.Current = type;
        Refresh();
    }

    private void Refresh()
    {
        int current = (int)SkinSelection.Current;
        for (int i = 0; i < cardFrames.Length; i++)
        {
            if (cardFrames[i] != null)
                cardFrames[i].sprite = i == current ? cardSelectedSprite : cardSprite;
        }
    }

    private static Button MakeButton(GameObject go, Sprite selected)
    {
        Button button = go.AddComponent<Button>();
        button.transition = Selectable.Transition.SpriteSwap;
        button.spriteState = new SpriteState
        {
            highlightedSprite = selected,
            selectedSprite = selected,
            pressedSprite = selected
        };
        go.AddComponent<UIAudioFeedback>();
        return button;
    }

    private static void Place(RectTransform rect, float x, float y, float w, float h)
    {
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(x, -y) * Scale;
        rect.sizeDelta = new Vector2(w, h) * Scale;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static GameObject CreateRect(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        go.layer = parent.gameObject.layer;
        return go;
    }

    private static GameObject CreateImage(string name, Transform parent, Sprite sprite)
    {
        GameObject go = CreateRect(name, parent);
        Image image = go.AddComponent<Image>();
        image.sprite = sprite;
        return go;
    }

    private static Sprite Load(string name)
    {
        return Resources.Load<Sprite>("MenuUI/" + name);
    }
}
