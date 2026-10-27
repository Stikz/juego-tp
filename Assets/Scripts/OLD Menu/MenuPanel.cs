using System.Collections;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject defaultSelected;

    private void OnEnable()
    {
        StartCoroutine(SelectNextFrame());
    }

    private IEnumerator SelectNextFrame()
    {
        yield return null;

        if (defaultSelected == null) yield break;
        if (NavigationManager.Instance == null) yield break;

        NavigationManager.Instance.EnterMenu(defaultSelected);
    }
}
