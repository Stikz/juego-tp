using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private bool hasKeycard = false;

    public void CollectKeycard()
    {
        hasKeycard = true;
        Debug.Log("¡Keycard recogida!");
    }

    public bool HasKeycard()
    {
        return hasKeycard;
    }
}
