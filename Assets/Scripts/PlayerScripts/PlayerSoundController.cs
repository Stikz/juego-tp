using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoTaser;

    public void PlayTaser()
    {
        if (audioSource != null && sonidoTaser != null)
            audioSource.PlayOneShot(sonidoTaser);
    }
}
