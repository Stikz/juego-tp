using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;

    private void Start()
    {
        if (audioSource == null || musicClip == null) return;

        audioSource.outputAudioMixerGroup =
            Resources.Load<UnityEngine.Audio.AudioMixer>("Mixer")
            .FindMatchingGroups("Music")[0];

        audioSource.loop = true;
        audioSource.clip = musicClip;
        audioSource.Play();
    }
}
