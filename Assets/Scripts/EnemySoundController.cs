using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pasoClip;

    // Call this when the enemy step
    public void PlayStep()
    {
        if (audioSource != null && pasoClip != null)
            audioSource.PlayOneShot(pasoClip);
    }
}
