using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip stepClip;

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    public void PlayStep()
    {
        if (audioSource == null || stepClip == null) return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(stepClip);
    }
}
