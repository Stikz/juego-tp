using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pasoClip;

    public void PlayStep()
    {
        if (audioSource != null && pasoClip != null)
            audioSource.PlayOneShot(pasoClip);
    }
}
