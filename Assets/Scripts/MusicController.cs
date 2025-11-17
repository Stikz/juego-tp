using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;

    private void Start()
    {
        if (audioSource != null && musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
