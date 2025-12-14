using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;

    [SerializeField] private InputActionReference navigateAction; 
    private EventSystem es;

    private GameObject lastSelected;      
    private GameObject currentDefault;    
    private float reselectionCooldownUntil;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += (_, __) => HookEventSystem();
        HookEventSystem();
    }

    private void OnEnable()
    {
        if (navigateAction != null) navigateAction.action.Enable();

    }

    private void HookEventSystem()
    {
        es = EventSystem.current;
        if (es == null)
            Debug.LogWarning("[NavigationManager] No hay EventSystem.current en esta escena.");
    }

    private void Update()
    {
        if (es == null || es != EventSystem.current)
            HookEventSystem();

        if (es == null) return;

        if (es.currentSelectedGameObject != null)
            lastSelected = es.currentSelectedGameObject;

        Vector2 nav = Vector2.zero;
        if (navigateAction != null)
            nav = navigateAction.action.ReadValue<Vector2>();

        bool navigating = nav.sqrMagnitude > 0.01f;

        if (es.currentSelectedGameObject == null &&
            navigating &&
            Time.unscaledTime >= reselectionCooldownUntil)
        {
            var target = lastSelected != null ? lastSelected : currentDefault;

            if (target != null && target.activeInHierarchy)
            {
                es.SetSelectedGameObject(target);
                reselectionCooldownUntil = Time.unscaledTime + 0.12f;
            }
        }
    }

    public void EnterMenu(GameObject defaultSelected)
    {
        currentDefault = defaultSelected;

        if (es == null) HookEventSystem();
        if (es == null) return;

        if (defaultSelected != null && defaultSelected.activeInHierarchy)
        {
            lastSelected = defaultSelected;
            es.SetSelectedGameObject(defaultSelected);
        }
    }
}
