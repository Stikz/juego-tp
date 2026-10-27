using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip taserSound;

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    public void PlayTaser()
    {
        if (audioSource == null || taserSound == null) return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(taserSound);
    }
}
